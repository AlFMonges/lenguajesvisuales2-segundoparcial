# 📚 lenguajesvisuales2-segundoparcial

## 🚀 API de Gestión y Archivo de Clientes (ApiClientes)

### 📋 Descripción General

[cite_start]Esta es una **API RESTful** desarrollada con **ASP.NET Core Web API (.NET 8.0)** [cite: 11] [cite_start]y **SQL Server** bajo el enfoque **Code First** de Entity Framework Core[cite: 11]. [cite_start]El objetivo principal es gestionar el registro de clientes y sus archivos asociados[cite: 13].

[cite_start]El proyecto implementa buenas prácticas de desarrollo, control de versiones y despliegue en un entorno de hosting[cite: 14].

### 🌟 Requerimientos Implementados

| Requisito | Funcionalidad | Descripción |
| :--- | :--- | :--- |
| **R1** | Registro de Clientes | [cite_start]Permite registrar datos básicos (CI, Nombres, Dirección, Teléfono) y las tres fotos de casa, almacenadas en la base de datos[cite: 15, 16, 18]. |
| **R2** | Carga Múltiple de Archivos | [cite_start]Servicio para subir un archivo **.zip**, que se descomprime para guardar múltiples archivos en el servidor y registrar su metadata[cite: 22, 25]. |
| **R3** | Logging y Errores | [cite_start]Implementación de *middleware* para registrar errores y eventos de seguimiento en la tabla `LogApi`, permitiendo su consulta mediante un *endpoint* GET[cite: 26, 28, 30]. |
| **R4** | Publicación en Hosting | [cite_start]La API ha sido publicada en un servidor de hosting, con la base de datos configurada y verificada[cite: 31, 33, 34]. |
| **R5** | Repositorio GitHub | [cite_start]El código fuente completo está disponible en este repositorio público, incluyendo el archivo `README.md`[cite: 35, 37]. |
| **R6** | Documentación de Pruebas | [cite_start]Se adjunta la documentación de pruebas con evidencias y casos de prueba ejecutados[cite: 38, 39, 40]. |

---

## 🛠️ Tecnologías Utilizadas

| Categoría | Tecnología | Notas |
| :--- | :--- | :--- |
| **Backend** | C# / ASP.NET Core (.NET 8.0) | [cite_start]Core del proyecto[cite: 11]. |
| **Base de Datos** | SQL Server | [cite_start]Usado con Code First[cite: 11]. |
| **Acceso a Datos** | Entity Framework Core | Gestión de migraciones. |
| **Documentación** | Swagger / OpenAPI | Documentación interactiva en la ruta raíz. |
| **Archivos** | `varbinary(max)` y Disco Local | [cite_start]Fotos de casa en DB, documentos y videos en `/uploads`[cite: 18]. |

---

## ⚙️ Configuración e Instalación Local

### 1. Requisitos Previos

* **.NET SDK** (versión compatible con .NET 8.0 o superior).
* **SQL Server** (o acceso a una instancia de SQL Server).

### 2. Ejecución del Proyecto

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/](https://github.com/)[TU_USUARIO]/lenguajesvisuales2-segundoparcial.git
    cd ApiClientes
    ```
2.  **Configurar Cadena de Conexión:**
    Asegúrate de que la cadena de conexión `ConexionSqlProduccion` en `appsettings.json` apunte a tu instancia local de SQL Server.
3.  **Restaurar dependencias y Ejecutar:**
    ```bash
    dotnet restore
    dotnet run
    ```
    La aplicación intentará **aplicar las migraciones** de la base de datos automáticamente al iniciar.

---

## 🧭 Endpoints Principales

La documentación completa de los *endpoints* está disponible en **Swagger UI** en la ruta raíz de la aplicación local (ej: `https://localhost:XXXX/`).

| Funcionalidad | Método | Ruta (Base: `/api/`) |
| :--- | :--- | :--- |
| **Registro de Cliente** | `POST` | `/Clientes` |
| **Subir Archivos ZIP** | `POST` | `/Clientes/{ci}/Archivos` |
| **Consulta de Logs** | `GET` | `/Logs` |
| **Obtener Cliente por CI** | `GET` | `/Clientes/{ci}` |
| **Listar Todos Clientes** | `GET` | `/Clientes` |
| **Descarga Archivos** | `GET` | `/uploads/{ci}/{nombreArchivo}` |

---

## 🌎 Despliegue y Acceso Público (R4)

La API ha sido publicada en un servidor de *hosting*.

### 1. URL Base del Entorno Publicado

> **URL de la API:** `[COLOCAR AQUÍ LA URL DEL SERVIDOR WEB, EJ: https://api.tudominio.com]`

### 2. Acceso a la Documentación (Swagger UI)

La documentación interactiva y los *endpoints* están disponibles en la raíz del entorno publicado:

> **URL de Swagger:** `[URL_DEL_SERVIDOR]/`

---