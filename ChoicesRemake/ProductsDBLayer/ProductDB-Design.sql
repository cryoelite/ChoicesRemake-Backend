-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Category]
CREATE TABLE [dbo].[Category]
(
 [cat_id] bigint NOT NULL ,
 [value]  varchar(512) NOT NULL ,


 CONSTRAINT [PK_38] PRIMARY KEY CLUSTERED ([cat_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Color]
CREATE TABLE [dbo].[Color]
(
 [color_id] bigint NOT NULL ,
 [value]    varchar(50) NOT NULL ,


 CONSTRAINT [PK_16] PRIMARY KEY CLUSTERED ([color_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Description]
CREATE TABLE [dbo].[Description]
(
 [desc_id]          bigint NOT NULL ,
 [title]            nvarchar(4000) NOT NULL ,
 [long_description] nvarchar(max) NOT NULL ,


 CONSTRAINT [PK_27] PRIMARY KEY CLUSTERED ([desc_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Image]
CREATE TABLE [dbo].[Image]
(
 [image_id]  bigint NOT NULL ,
 [name]      nvarchar(50) NOT NULL ,
 [location]  nvarchar(512) NOT NULL ,
 [mini_desc] nvarchar(4000) NULL ,


 CONSTRAINT [PK_21] PRIMARY KEY CLUSTERED ([image_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Mass]
CREATE TABLE [dbo].[Mass]
(
 [mass_id]  bigint NOT NULL ,
 [massInKg] float NULL ,


 CONSTRAINT [PK_12] PRIMARY KEY CLUSTERED ([mass_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Misc_Detail]
CREATE TABLE [dbo].[Misc_Detail]
(
 [detail_id] bigint NOT NULL ,
 [key]       nvarchar(512) NULL ,
 [value]     nvarchar(1024) NULL ,


 CONSTRAINT [PK_42] PRIMARY KEY CLUSTERED ([detail_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Size]
CREATE TABLE [dbo].[Size]
(
 [size_id]    bigint NOT NULL ,
 [WidthInMM]  float NULL ,
 [LengthInMM] float NULL ,
 [HeightInMM] float NULL ,


 CONSTRAINT [PK_32] PRIMARY KEY CLUSTERED ([size_id] ASC)
);
GO

-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

-- ************************************** [dbo].[Product]
CREATE TABLE [dbo].[Product]
(
 [prod_id]   bigint NOT NULL ,
 [image_id]  bigint NOT NULL ,
 [size_id]   bigint NOT NULL ,
 [cat_id]    bigint NOT NULL ,
 [color_id]  bigint NOT NULL ,
 [mass_id]   bigint NOT NULL ,
 [name]      nvarchar(512) NOT NULL ,
 [brand]     nvarchar(512) NOT NULL ,
 [designer]  nvarchar(512) NOT NULL ,
 [price]     money NOT NULL ,
 [desc_id]   bigint NOT NULL ,
 [detail_id] bigint NOT NULL ,


 CONSTRAINT [PK_69] PRIMARY KEY CLUSTERED ([prod_id] ASC, [image_id] ASC, [size_id] ASC, [cat_id] ASC, [color_id] ASC, [mass_id] ASC),
 CONSTRAINT [FK_48] FOREIGN KEY ([image_id])  REFERENCES [dbo].[Image]([image_id]),
 CONSTRAINT [FK_51] FOREIGN KEY ([desc_id])  REFERENCES [dbo].[Description]([desc_id]),
 CONSTRAINT [FK_54] FOREIGN KEY ([size_id])  REFERENCES [dbo].[Size]([size_id]),
 CONSTRAINT [FK_57] FOREIGN KEY ([cat_id])  REFERENCES [dbo].[Category]([cat_id]),
 CONSTRAINT [FK_60] FOREIGN KEY ([mass_id])  REFERENCES [dbo].[Mass]([mass_id]),
 CONSTRAINT [FK_63] FOREIGN KEY ([color_id])  REFERENCES [dbo].[Color]([color_id]),
 CONSTRAINT [FK_66] FOREIGN KEY ([detail_id])  REFERENCES [dbo].[Misc_Detail]([detail_id])
);
GO


CREATE NONCLUSTERED INDEX [fkIdx_50] ON [dbo].[Product] 
 (
  [image_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_53] ON [dbo].[Product] 
 (
  [desc_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_56] ON [dbo].[Product] 
 (
  [size_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_59] ON [dbo].[Product] 
 (
  [cat_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_62] ON [dbo].[Product] 
 (
  [mass_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_65] ON [dbo].[Product] 
 (
  [color_id] ASC
 )

GO

CREATE NONCLUSTERED INDEX [fkIdx_68] ON [dbo].[Product] 
 (
  [detail_id] ASC
 )

GO



