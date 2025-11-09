using Microsoft.EntityFrameworkCore;
using ApiClientes.Models;

namespace ApiClientes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ArchivoCliente> ArchivosCliente { get; set; }
        public DbSet<LogApi> LogsApi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CI);

                entity.Property(e => e.CI)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FechaRegistro)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Relación con Archivos
                entity.HasMany(c => c.Archivos)
                    .WithOne(a => a.Cliente)
                    .HasForeignKey(a => a.CICliente)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índices adicionales
                entity.HasIndex(e => e.Nombres)
                    .HasDatabaseName("IX_Clientes_Nombres");

                entity.HasIndex(e => e.FechaRegistro)
                    .HasDatabaseName("IX_Clientes_FechaRegistro");
            });

            // Configuración de ArchivoCliente
            modelBuilder.Entity<ArchivoCliente>(entity =>
            {
                entity.HasKey(e => e.IdArchivo);

                entity.Property(e => e.IdArchivo)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FechaSubida)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.NombreArchivo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UrlArchivo)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            // Configuración de LogApi
            modelBuilder.Entity<LogApi>(entity =>
            {
                entity.HasKey(e => e.IdLog);

                entity.Property(e => e.IdLog)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DateTime)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.TipoLog)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Datos semilla (opcional)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Datos iniciales
            /*
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    CI = "1234567",
                    Nombres = "Cliente Demo",
                    Direccion = "Dirección de prueba",
                    Telefono = "+595981234567",
                    FechaRegistro = DateTime.UtcNow
                }
            );
            */
        }
    }
}
