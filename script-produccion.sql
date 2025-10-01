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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904165249_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250904165249_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904165650_DescripcionAddedToCursoEntity'
)
BEGIN
    ALTER TABLE [Cursos] ADD [Descripcion] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250904165650_DescripcionAddedToCursoEntity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250904165650_DescripcionAddedToCursoEntity', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250914231315_OrdenyArchivado_columnas'
)
BEGIN
    ALTER TABLE [Videos] ADD [Orden] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250914231315_OrdenyArchivado_columnas'
)
BEGIN
    ALTER TABLE [Cursos] ADD [Archivado] bit NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250914231315_OrdenyArchivado_columnas'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250914231315_OrdenyArchivado_columnas', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250923213420_EvaluacionesRevisadas entity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250923213420_EvaluacionesRevisadas entity', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250923221147_add EvaluacionesRevisadas entity'
)
BEGIN
    CREATE TABLE [EvaluacionesRevisadas] (
        [IdRevision] int NOT NULL IDENTITY,
        [KEmpleado] int NOT NULL,
        [IdEvaluacion] int NOT NULL,
        [Fecha] datetime2 NOT NULL,
        [Calificacion] decimal(18,2) NOT NULL,
        [IdEvaluacionNavigationIdEvaluacion] int NULL,
        CONSTRAINT [PK_EvaluacionesRevisadas] PRIMARY KEY ([IdRevision]),
        CONSTRAINT [FK_EvaluacionesRevisadas_Evaluaciones_IdEvaluacionNavigationIdEvaluacion] FOREIGN KEY ([IdEvaluacionNavigationIdEvaluacion]) REFERENCES [Evaluaciones] ([idEvaluacion])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250923221147_add EvaluacionesRevisadas entity'
)
BEGIN
    CREATE INDEX [IX_EvaluacionesRevisadas_IdEvaluacionNavigationIdEvaluacion] ON [EvaluacionesRevisadas] ([IdEvaluacionNavigationIdEvaluacion]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250923221147_add EvaluacionesRevisadas entity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250923221147_add EvaluacionesRevisadas entity', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250927183944_EmpleadoRegistrado new entity'
)
BEGIN
    CREATE TABLE [EmpleadosRegistrados] (
        [IdRegistro] int NOT NULL IDENTITY,
        [IdEmpleado] int NOT NULL,
        [IdTipoUsuario] int NOT NULL,
        CONSTRAINT [PK_EmpleadosRegistrados] PRIMARY KEY ([IdRegistro])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250927183944_EmpleadoRegistrado new entity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250927183944_EmpleadoRegistrado new entity', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250929230356_add RespuestasPreguntasOpciones'
)
BEGIN
    ALTER TABLE [RespuestasPreguntas] ADD [IdRevision] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250929230356_add RespuestasPreguntasOpciones'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250929230356_add RespuestasPreguntasOpciones', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250929230857_add RespuestasPreguntasOpcion'
)
BEGIN
    CREATE TABLE [RespuestasPreguntaOpcion] (
        [IdRespuesta] int NOT NULL IDENTITY,
        [IdPregunta] int NOT NULL,
        [IdOpcionElegida] int NOT NULL,
        [IdRevision] int NOT NULL,
        [IdEvaluacionRevisadaNavigationIdRevision] int NULL,
        CONSTRAINT [PK_RespuestasPreguntaOpcion] PRIMARY KEY ([IdRespuesta]),
        CONSTRAINT [FK_RespuestasPreguntaOpcion_EvaluacionesRevisadas_IdEvaluacionRevisadaNavigationIdRevision] FOREIGN KEY ([IdEvaluacionRevisadaNavigationIdRevision]) REFERENCES [EvaluacionesRevisadas] ([IdRevision])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250929230857_add RespuestasPreguntasOpcion'
)
BEGIN
    CREATE INDEX [IX_RespuestasPreguntaOpcion_IdEvaluacionRevisadaNavigationIdRevision] ON [RespuestasPreguntaOpcion] ([IdEvaluacionRevisadaNavigationIdRevision]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250929230857_add RespuestasPreguntasOpcion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250929230857_add RespuestasPreguntasOpcion', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001194011_VerificacionFinal'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251001194011_VerificacionFinal', N'8.0.0');
END;
GO

COMMIT;
GO

