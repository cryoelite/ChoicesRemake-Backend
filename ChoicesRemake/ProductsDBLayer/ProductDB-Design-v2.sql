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

CREATE TABLE [Category] (
    [cat_id] bigint NOT NULL IDENTITY,
    [value] varchar(512) NOT NULL,
    CONSTRAINT [PK_38] PRIMARY KEY ([cat_id])
);
GO

CREATE TABLE [Color] (
    [color_id] bigint NOT NULL IDENTITY,
    [value] varchar(50) NOT NULL,
    CONSTRAINT [PK_Color] PRIMARY KEY ([color_id])
);
GO

CREATE TABLE [Description] (
    [desc_id] bigint NOT NULL IDENTITY,
    [title] nvarchar(4000) NOT NULL,
    [long_description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_27] PRIMARY KEY ([desc_id])
);
GO

CREATE TABLE [Image] (
    [image_id] bigint NOT NULL IDENTITY,
    [name] nvarchar(50) NOT NULL,
    [location] nvarchar(512) NOT NULL,
    [mini_desc] nvarchar(4000) NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY ([image_id])
);
GO

CREATE TABLE [Mass] (
    [mass_id] bigint NOT NULL IDENTITY,
    [massInKg] float NULL,
    CONSTRAINT [PK_Mass] PRIMARY KEY ([mass_id])
);
GO

CREATE TABLE [Misc_Detail] (
    [detail_id] bigint NOT NULL IDENTITY,
    [key] nvarchar(512) NULL,
    [value] nvarchar(1024) NULL,
    CONSTRAINT [PK_42] PRIMARY KEY ([detail_id])
);
GO

CREATE TABLE [Size] (
    [size_id] bigint NOT NULL IDENTITY,
    [WidthInMM] float NULL,
    [LengthInMM] float NULL,
    [HeightInMM] float NULL,
    CONSTRAINT [PK_Size] PRIMARY KEY ([size_id])
);
GO

CREATE TABLE [Product] (
    [prod_id] bigint NOT NULL IDENTITY,
    [image_id] bigint NOT NULL,
    [size_id] bigint NOT NULL,
    [cat_id] bigint NOT NULL,
    [color_id] bigint NOT NULL,
    [mass_id] bigint NOT NULL,
    [name] nvarchar(512) NOT NULL,
    [brand] nvarchar(512) NOT NULL,
    [designer] nvarchar(512) NOT NULL,
    [price] money NOT NULL,
    [desc_id] bigint NOT NULL,
    [detail_id] bigint NOT NULL,
    CONSTRAINT [PK_69] PRIMARY KEY ([prod_id], [image_id], [size_id], [cat_id], [color_id], [mass_id]),
    CONSTRAINT [FK_48] FOREIGN KEY ([image_id]) REFERENCES [Image] ([image_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_51] FOREIGN KEY ([desc_id]) REFERENCES [Description] ([desc_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_54] FOREIGN KEY ([size_id]) REFERENCES [Size] ([size_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_57] FOREIGN KEY ([cat_id]) REFERENCES [Category] ([cat_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_60] FOREIGN KEY ([mass_id]) REFERENCES [Mass] ([mass_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_63] FOREIGN KEY ([color_id]) REFERENCES [Color] ([color_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_66] FOREIGN KEY ([detail_id]) REFERENCES [Misc_Detail] ([detail_id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [fkIdx_50] ON [Product] ([image_id]);
GO

CREATE INDEX [fkIdx_53] ON [Product] ([desc_id]);
GO

CREATE INDEX [fkIdx_56] ON [Product] ([size_id]);
GO

CREATE INDEX [fkIdx_59] ON [Product] ([cat_id]);
GO

CREATE INDEX [fkIdx_62] ON [Product] ([mass_id]);
GO

CREATE INDEX [fkIdx_65] ON [Product] ([color_id]);
GO

CREATE INDEX [fkIdx_68] ON [Product] ([detail_id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20211021161305_init', N'5.0.11');
GO

COMMIT;
GO

