IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201012053131_CreateDatabase')
BEGIN
    CREATE TABLE [Especie] (
        [IdEspecie] int NOT NULL IDENTITY,
        [NombreEspecie] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Especie] PRIMARY KEY ([IdEspecie])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201012053131_CreateDatabase')
BEGIN
    CREATE TABLE [User] (
        [IdUser] int NOT NULL IDENTITY,
        [NombreUser] nvarchar(max) NOT NULL,
        [Clave] nvarchar(max) NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [Apellido] nvarchar(max) NOT NULL,
        [Dni] int NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([IdUser])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201012053131_CreateDatabase')
BEGIN
    CREATE TABLE [Animal] (
        [IdAnimal] int NOT NULL IDENTITY,
        [Nombre] nvarchar(max) NOT NULL,
        [FecNac] datetime2 NOT NULL,
        [EspecieId] int NOT NULL,
        CONSTRAINT [PK_Animal] PRIMARY KEY ([IdAnimal]),
        CONSTRAINT [FK_Animal_Especie_EspecieId] FOREIGN KEY ([EspecieId]) REFERENCES [Especie] ([IdEspecie]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201012053131_CreateDatabase')
BEGIN
    CREATE INDEX [IX_Animal_EspecieId] ON [Animal] ([EspecieId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20201012053131_CreateDatabase')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20201012053131_CreateDatabase', N'3.1.8');
END;

GO

