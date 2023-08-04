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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [RoomStatuses] (
        [Id] int NOT NULL IDENTITY,
        [Status] nvarchar(max) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_RoomStatuses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [SeatStatuses] (
        [Id] int NOT NULL IDENTITY,
        [Status] nvarchar(max) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_SeatStatuses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [TypeOfFilms] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TypeOfFilms] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [UserName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Rooms] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [RoomStatusId] int NOT NULL,
        CONSTRAINT [PK_Rooms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rooms_RoomStatuses_RoomStatusId] FOREIGN KEY ([RoomStatusId]) REFERENCES [RoomStatuses] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Films] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [OpeningDay] datetime2 NOT NULL,
        [Director] nvarchar(max) NULL,
        [Time] nvarchar(max) NULL,
        [Image] nvarchar(max) NULL,
        [TypeOfFilmId] int NOT NULL,
        CONSTRAINT [PK_Films] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Films_TypeOfFilms_TypeOfFilmId] FOREIGN KEY ([TypeOfFilmId]) REFERENCES [TypeOfFilms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Bills] (
        [Id] int NOT NULL IDENTITY,
        [PriceTotal] int NOT NULL,
        [CreateDate] datetime2 NOT NULL,
        [Quantity] int NOT NULL,
        [CustomerId] int NULL,
        [UserId] int NULL,
        CONSTRAINT [PK_Bills] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Bills_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]),
        CONSTRAINT [FK_Bills_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [RefreshToken] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] int NOT NULL,
        [Token] nvarchar(max) NOT NULL,
        [JwtId] nvarchar(max) NOT NULL,
        [IsUsed] bit NOT NULL,
        [IsRevoked] bit NOT NULL,
        [IssuedAt] datetime2 NOT NULL,
        [ExpireAt] datetime2 NOT NULL,
        CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RefreshToken_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([RoleId], [UserId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Seats] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Status] int NULL,
        [Price] int NOT NULL,
        [RoomId] int NOT NULL,
        [SeatStatusId] int NOT NULL,
        CONSTRAINT [PK_Seats] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Seats_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Rooms] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Seats_SeatStatuses_SeatStatusId] FOREIGN KEY ([SeatStatusId]) REFERENCES [SeatStatuses] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [ShowTimes] (
        [Id] int NOT NULL IDENTITY,
        [Time] datetime2 NOT NULL,
        [FilmId] int NOT NULL,
        [RoomId] int NOT NULL,
        CONSTRAINT [PK_ShowTimes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ShowTimes_Films_FilmId] FOREIGN KEY ([FilmId]) REFERENCES [Films] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ShowTimes_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Rooms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE TABLE [Tickets] (
        [Id] int NOT NULL IDENTITY,
        [ShowTimeId] int NULL,
        [SeatId] int NULL,
        [BillId] int NOT NULL,
        CONSTRAINT [PK_Tickets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tickets_Bills_BillId] FOREIGN KEY ([BillId]) REFERENCES [Bills] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Tickets_Seats_SeatId] FOREIGN KEY ([SeatId]) REFERENCES [Seats] ([Id]),
        CONSTRAINT [FK_Tickets_ShowTimes_ShowTimeId] FOREIGN KEY ([ShowTimeId]) REFERENCES [ShowTimes] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Bills_CustomerId] ON [Bills] ([CustomerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Bills_UserId] ON [Bills] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Films_TypeOfFilmId] ON [Films] ([TypeOfFilmId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_RefreshToken_UserId] ON [RefreshToken] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Rooms_RoomStatusId] ON [Rooms] ([RoomStatusId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Seats_RoomId] ON [Seats] ([RoomId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Seats_SeatStatusId] ON [Seats] ([SeatStatusId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_ShowTimes_FilmId] ON [ShowTimes] ([FilmId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_ShowTimes_RoomId] ON [ShowTimes] ([RoomId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Tickets_BillId] ON [Tickets] ([BillId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Tickets_SeatId] ON [Tickets] ([SeatId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_Tickets_ShowTimeId] ON [Tickets] ([ShowTimeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    CREATE INDEX [IX_UserRoles_UserId] ON [UserRoles] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230804070613_DbInit')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230804070613_DbInit', N'7.0.9');
END;
GO

COMMIT;
GO

