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
    WHERE [MigrationId] = N'20240503154419_MangaApp'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] uniqueidentifier NOT NULL,
        [UserName] nvarchar(max) NULL,
        [UserEmail] nvarchar(max) NULL,
        [Avatar] nvarchar(max) NULL,
        [HashPassword] varbinary(max) NULL,
        [SaltPassword] varbinary(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240503154419_MangaApp'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240503154419_MangaApp', N'9.0.0-preview.3.24172.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504111005_MangaAppUpdate'
)
BEGIN
    CREATE TABLE [UserMangas] (
        [Id] uniqueidentifier NOT NULL,
        [MangaId] uniqueidentifier NOT NULL,
        [Slug] nvarchar(max) NOT NULL,
        [MangaName] nvarchar(max) NOT NULL,
        [MangaImage] nvarchar(max) NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_UserMangas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserMangas_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504111005_MangaAppUpdate'
)
BEGIN
    CREATE INDEX [IX_UserMangas_UserId] ON [UserMangas] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504111005_MangaAppUpdate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240504111005_MangaAppUpdate', N'9.0.0-preview.3.24172.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504120404_update_fix'
)
BEGIN
    ALTER TABLE [UserMangas] DROP CONSTRAINT [PK_UserMangas];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504120404_update_fix'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserMangas]') AND [c].[name] = N'Id');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserMangas] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [UserMangas] DROP COLUMN [Id];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504120404_update_fix'
)
BEGIN
    ALTER TABLE [UserMangas] ADD CONSTRAINT [PK_UserMangas] PRIMARY KEY ([MangaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240504120404_update_fix'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240504120404_update_fix', N'9.0.0-preview.3.24172.4');
END;
GO

COMMIT;
GO

