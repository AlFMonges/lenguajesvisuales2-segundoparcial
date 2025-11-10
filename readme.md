# 📋 API Clientes

API REST desarrollada en ASP.NET Core 8.0 para el registro y gestión de clientes con almacenamiento de fotografías y archivos asociados.
Incluye sistema de logging automático para seguimiento de operaciones.

**Demo en producción:** [http://apiclientes.runasp.net](http://apiclientes.runasp.net)

---

## 📄 Descripción General

Sistema que permite:
- **Registro de clientes** con información básica (CI, nombres, dirección, teléfono) y hasta 3 fotografías de su vivienda
- **Carga de múltiples archivos** mediante archivos ZIP que se descomprimen automáticamente
- **Seguimiento completo** de todas las operaciones mediante sistema de logs

Las fotografías se almacenan en la base de datos, mientras que los archivos adicionales se guardan en el servidor con registro de metadata.

---

## 🛠️ Tecnologías Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework principal
- **Entity Framework Core 8.0** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional

### Arquitectura
- **Code First** - Migraciones automáticas de base de datos
- **Service Layer Pattern** - Separación de lógica de negocio
- **Dependency Injection** - Inyección de dependencias nativa
- **Middleware Pattern** - Captura automática de logs

---

## 🚀 Instrucciones de Ejecución Local

### Requisitos Previos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) o superior
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)

### Pasos de Instalación

#### 1. Clonar el repositorio
```bash
git clone  https://github.com/AlFMonges/lenguajesvisuales2-segundoparcial.git

cd api-clientes
```

#### 2. Restaurar paquetes NuGet
```bash
dotnet restore
```

#### 3. Configurar la conexión a base de datos

Editar el archivo `appsettings.json` y actualizar la cadena de conexión:
```json
{
  "ConnectionStrings": {
    "ConexionSqlProduccion": "Server=localhost\\SQLEXPRESS;Database=ClientesDB;User ID=sa;Password=xxxxxxxxx;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False"
  }
}
```

**Nota:** Ajusta `localhost\\SQLEXPRESS` según tu instalación de SQL Server.

#### 4. Aplicar migraciones (crear base de datos)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Esto creará automáticamente la base de datos `ClientesDB` con todas las tablas necesarias.

#### 5. Ejecutar la aplicación
```bash
dotnet run
```

La aplicación estará disponible en:
- http://localhost:5289

---

## 📌 Endpoints Principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `POST` | `/api/clientes/registrar` | Registrar nuevo cliente con fotos |
| `GET` | `/api/clientes` | Obtener todos los clientes |
| `GET` | `/api/clientes/{ci}` | Obtener cliente específico |
| `POST` | `/api/clientes/{ci}/subir-archivos` | Subir archivos ZIP para cliente |
| `GET` | `/api/logs` | Consultar logs del sistema |

---

## 🗄️ Estructura de la Base de Datos

El sistema crea automáticamente 3 tablas:

- **Clientes**: Información básica y fotografías (almacenadas como VARBINARY)
- **ArchivosCliente**: Metadata de archivos subidos por cada cliente
- **LogsApi**: Registro de todas las operaciones del sistema

---

## 📧 Contacto

**Desarrollador:** Alcides Monges  
**Email:** alfmonges95@gmail.com

---
