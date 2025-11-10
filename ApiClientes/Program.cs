using ApiClientes.Data;
using ApiClientes.Middleware;
using ApiClientes.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger con documentación
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gestión de Clientes",
        Version = "v1",
        Description = "API REST para registro y gestión de clientes con archivos asociados",
        Contact = new OpenApiContact
        {
            Name = "Soporte Técnico",
            Email = "alfmonges95@gmail.com"
        }
    });
    
    // Incluir comentarios XML si existen
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Base de datos con Code First
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSqlProduccion"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null
            );
            sqlOptions.CommandTimeout(30);
        }
    );

    // Habilitar logging sensible en desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Servicios personalizados
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ILogService, LogService>();

// Configurar tamaño máximo de archivos
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Agregar compresión de respuestas
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        app.Logger.LogInformation("Migraciones aplicadas exitosamente");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error al aplicar migraciones");
    }
}

// Configurar pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clientes API V1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

app.UseResponseCompression();
app.UseCors("AllowAll");
app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();

// Servir archivos estáticos desde la carpeta uploads
app.UseStaticFiles();
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("Aplicación iniciada correctamente");

app.Run();
