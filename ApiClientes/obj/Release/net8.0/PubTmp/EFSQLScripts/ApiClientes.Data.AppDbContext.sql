IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE TABLE [Clientes] (
        [CI] nvarchar(50) NOT NULL,
        [Nombres] nvarchar(200) NOT NULL,
        [Direccion] nvarchar(500) NOT NULL,
        [Telefono] nvarchar(20) NOT NULL,
        [FotoCasa1] varbinary(max) NULL,
        [FotoCasa2] varbinary(max) NULL,
        [FotoCasa3] varbinary(max) NULL,
        [FechaRegistro] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([CI])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE TABLE [LogsApi] (
        [IdLog] int NOT NULL IDENTITY,
        [DateTime] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        [TipoLog] nvarchar(50) NOT NULL,
        [RequestBody] nvarchar(max) NULL,
        [ResponseBody] nvarchar(max) NULL,
        [UrlEndpoint] nvarchar(500) NULL,
        [MetodoHttp] nvarchar(10) NULL,
        [DireccionIp] nvarchar(50) NULL,
        [Detalle] nvarchar(max) NULL,
        [CodigoEstado] int NULL,
        CONSTRAINT [PK_LogsApi] PRIMARY KEY ([IdLog])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE TABLE [ArchivosCliente] (
        [IdArchivo] int NOT NULL IDENTITY,
        [CICliente] nvarchar(50) NOT NULL,
        [NombreArchivo] nvarchar(255) NOT NULL,
        [UrlArchivo] nvarchar(1000) NOT NULL,
        [TamanoBytes] bigint NOT NULL,
        [TipoMime] nvarchar(100) NULL,
        [FechaSubida] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
        CONSTRAINT [PK_ArchivosCliente] PRIMARY KEY ([IdArchivo]),
        CONSTRAINT [FK_ArchivosCliente_Clientes_CICliente] FOREIGN KEY ([CICliente]) REFERENCES [Clientes] ([CI]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ArchivosCliente_CICliente] ON [ArchivosCliente] ([CICliente]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ArchivosCliente_FechaSubida] ON [ArchivosCliente] ([FechaSubida]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Clientes_FechaRegistro] ON [Clientes] ([FechaRegistro]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Clientes_Nombres] ON [Clientes] ([Nombres]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_LogsApi_CodigoEstado] ON [LogsApi] ([CodigoEstado]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_LogsApi_DateTime] ON [LogsApi] ([DateTime] DESC);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_LogsApi_TipoLog] ON [LogsApi] ([TipoLog]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109031230_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251109031230_InitialCreate', N'9.0.10');
END;

COMMIT;
GO

