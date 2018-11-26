
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/24/2018 11:28:30
-- Generated from EDMX file: D:\Github-Projects\eProject-Sem3\eProject-Sem3\ZuLuCommerce\ZuLuCommerce\Models\eCommerceDb.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [eCommerce];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Accounts_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_Accounts_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_CartItems_Carts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartItems] DROP CONSTRAINT [FK_CartItems_Carts];
GO
IF OBJECT_ID(N'[dbo].[FK_CartItems_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartItems] DROP CONSTRAINT [FK_CartItems_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Carts_Accounts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Carts] DROP CONSTRAINT [FK_Carts_Accounts];
GO
IF OBJECT_ID(N'[dbo].[FK_Categories_Topics]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_Categories_Topics];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Accounts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Accounts];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Comments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Comments];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_Levels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Levels];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderDetails_Orders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK_OrderDetails_Orders];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderDetails_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK_OrderDetails_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_OrderStatuses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_OrderStatuses];
GO
IF OBJECT_ID(N'[dbo].[FK_Orders_Shipments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Shipments];
GO
IF OBJECT_ID(N'[dbo].[FK_Pictures_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pictures] DROP CONSTRAINT [FK_Pictures_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_Categories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Categories];
GO
IF OBJECT_ID(N'[dbo].[FK_Products_Suppliers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_Products_Suppliers];
GO
IF OBJECT_ID(N'[dbo].[FK_Ratings_Accounts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_Ratings_Accounts];
GO
IF OBJECT_ID(N'[dbo].[FK_Ratings_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ratings] DROP CONSTRAINT [FK_Ratings_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Shipments_ShipmentStatuses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipments_ShipmentStatuses];
GO
IF OBJECT_ID(N'[dbo].[FK_Shipments_Shippers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipments_Shippers];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[CartItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartItems];
GO
IF OBJECT_ID(N'[dbo].[Carts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Carts];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Levels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Levels];
GO
IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[OrderStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderStatuses];
GO
IF OBJECT_ID(N'[dbo].[Pictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pictures];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ratings];
GO
IF OBJECT_ID(N'[dbo].[Shipments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shipments];
GO
IF OBJECT_ID(N'[dbo].[ShipmentStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShipmentStatuses];
GO
IF OBJECT_ID(N'[dbo].[Shippers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shippers];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[Table]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Table];
GO
IF OBJECT_ID(N'[dbo].[Topics]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Topics];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(200)  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'CartItems'
CREATE TABLE [dbo].[CartItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CartId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- Creating table 'Carts'
CREATE TABLE [dbo].[Carts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [TopicId] int  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [ParentId] int  NULL,
    [Content] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [DateComment] datetime  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [Phone] varchar(50)  NOT NULL,
    [Address] nvarchar(200)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Birthday] datetime  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [Phone] varchar(50)  NOT NULL,
    [Address] nvarchar(200)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Birthday] datetime  NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(200)  NOT NULL,
    [LevelId] int  NOT NULL,
    [LastLogin] datetime  NULL
);
GO

-- Creating table 'Levels'
CREATE TABLE [dbo].[Levels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LevelName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] int  NOT NULL,
    [ProductId] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [ProductName] nvarchar(50)  NULL,
    [Description] nvarchar(max)  NULL,
    [Price] decimal(19,4)  NULL
);
GO

-- Creating table 'OrderStatuses'
CREATE TABLE [dbo].[OrderStatuses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Pictures'
CREATE TABLE [dbo].[Pictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [URL] varchar(500)  NOT NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [UserId] int  NOT NULL,
    [Rating1] int  NOT NULL
);
GO

-- Creating table 'Shipments'
CREATE TABLE [dbo].[Shipments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StatusId] int  NOT NULL,
    [ShippedDate] datetime  NULL,
    [ShippingAddress] nvarchar(500)  NOT NULL,
    [ShippingCity] nvarchar(50)  NOT NULL,
    [ShippingFee] decimal(19,4)  NOT NULL,
    [ShipperId] int  NOT NULL
);
GO

-- Creating table 'ShipmentStatuses'
CREATE TABLE [dbo].[ShipmentStatuses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Shippers'
CREATE TABLE [dbo].[Shippers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [Phone] varchar(50)  NOT NULL,
    [Address] nvarchar(500)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Birthday] datetime  NOT NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Phone] varchar(50)  NOT NULL,
    [Address] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Tables'
CREATE TABLE [dbo].[Tables] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Topics'
CREATE TABLE [dbo].[Topics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TopicName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [Stock] int  NOT NULL,
    [CategoryId] int  NOT NULL,
    [SupplierId] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Thumbnail] varchar(500)  NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [StatusId] int  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [PaymentType] varchar(20)  NULL,
    [CustomerId] int  NULL,
    [EmployeeId] int  NULL,
    [ShipmentId] int  NULL,
    [Discount] decimal(18,2)  NULL,
    [Tax] decimal(18,2)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [PK_CartItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [PK_Carts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Levels'
ALTER TABLE [dbo].[Levels]
ADD CONSTRAINT [PK_Levels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderStatuses'
ALTER TABLE [dbo].[OrderStatuses]
ADD CONSTRAINT [PK_OrderStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [PK_Pictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [PK_Shipments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShipmentStatuses'
ALTER TABLE [dbo].[ShipmentStatuses]
ADD CONSTRAINT [PK_ShipmentStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shippers'
ALTER TABLE [dbo].[Shippers]
ADD CONSTRAINT [PK_Shippers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tables'
ALTER TABLE [dbo].[Tables]
ADD CONSTRAINT [PK_Tables]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Topics'
ALTER TABLE [dbo].[Topics]
ADD CONSTRAINT [PK_Topics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Accounts_Customers]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Accounts_Customers'
CREATE INDEX [IX_FK_Accounts_Customers]
ON [dbo].[Accounts]
    ([CustomerId]);
GO

-- Creating foreign key on [UserId] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [FK_Carts_Accounts]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Carts_Accounts'
CREATE INDEX [IX_FK_Carts_Accounts]
ON [dbo].[Carts]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Accounts]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Accounts'
CREATE INDEX [IX_FK_Comments_Accounts]
ON [dbo].[Comments]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_Ratings_Accounts]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ratings_Accounts'
CREATE INDEX [IX_FK_Ratings_Accounts]
ON [dbo].[Ratings]
    ([UserId]);
GO

-- Creating foreign key on [CartId] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [FK_CartItems_Carts]
    FOREIGN KEY ([CartId])
    REFERENCES [dbo].[Carts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CartItems_Carts'
CREATE INDEX [IX_FK_CartItems_Carts]
ON [dbo].[CartItems]
    ([CartId]);
GO

-- Creating foreign key on [TopicId] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_Categories_Topics]
    FOREIGN KEY ([TopicId])
    REFERENCES [dbo].[Topics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Categories_Topics'
CREATE INDEX [IX_FK_Categories_Topics]
ON [dbo].[Categories]
    ([TopicId]);
GO

-- Creating foreign key on [ParentId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Comments]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Comments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Comments'
CREATE INDEX [IX_FK_Comments_Comments]
ON [dbo].[Comments]
    ([ParentId]);
GO

-- Creating foreign key on [LevelId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_Levels]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[Levels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_Levels'
CREATE INDEX [IX_FK_Employees_Levels]
ON [dbo].[Employees]
    ([LevelId]);
GO

-- Creating foreign key on [StatusId] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_Shipments_ShipmentStatuses]
    FOREIGN KEY ([StatusId])
    REFERENCES [dbo].[ShipmentStatuses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Shipments_ShipmentStatuses'
CREATE INDEX [IX_FK_Shipments_ShipmentStatuses]
ON [dbo].[Shipments]
    ([StatusId]);
GO

-- Creating foreign key on [ShipperId] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_Shipments_Shippers]
    FOREIGN KEY ([ShipperId])
    REFERENCES [dbo].[Shippers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Shipments_Shippers'
CREATE INDEX [IX_FK_Shipments_Shippers]
ON [dbo].[Shipments]
    ([ShipperId]);
GO

-- Creating foreign key on [ProductId] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [FK_CartItems_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CartItems_Products'
CREATE INDEX [IX_FK_CartItems_Products]
ON [dbo].[CartItems]
    ([ProductId]);
GO

-- Creating foreign key on [CategoryId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_Categories]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_Categories'
CREATE INDEX [IX_FK_Products_Categories]
ON [dbo].[Products]
    ([CategoryId]);
GO

-- Creating foreign key on [ProductId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Products'
CREATE INDEX [IX_FK_Comments_Products]
ON [dbo].[Comments]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_OrderDetails_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDetails_Products'
CREATE INDEX [IX_FK_OrderDetails_Products]
ON [dbo].[OrderDetails]
    ([ProductId]);
GO

-- Creating foreign key on [ProductId] in table 'Pictures'
ALTER TABLE [dbo].[Pictures]
ADD CONSTRAINT [FK_Pictures_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pictures_Products'
CREATE INDEX [IX_FK_Pictures_Products]
ON [dbo].[Pictures]
    ([ProductId]);
GO

-- Creating foreign key on [SupplierId] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_Products_Suppliers]
    FOREIGN KEY ([SupplierId])
    REFERENCES [dbo].[Suppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_Suppliers'
CREATE INDEX [IX_FK_Products_Suppliers]
ON [dbo].[Products]
    ([SupplierId]);
GO

-- Creating foreign key on [ProductId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [FK_Ratings_Products]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ratings_Products'
CREATE INDEX [IX_FK_Ratings_Products]
ON [dbo].[Ratings]
    ([ProductId]);
GO

-- Creating foreign key on [CustomerId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Customers]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Customers'
CREATE INDEX [IX_FK_Orders_Customers]
ON [dbo].[Orders]
    ([CustomerId]);
GO

-- Creating foreign key on [EmployeeId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Employees]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Employees'
CREATE INDEX [IX_FK_Orders_Employees]
ON [dbo].[Orders]
    ([EmployeeId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_OrderDetails_Orders]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderDetails_Orders'
CREATE INDEX [IX_FK_OrderDetails_Orders]
ON [dbo].[OrderDetails]
    ([OrderId]);
GO

-- Creating foreign key on [StatusId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_OrderStatuses]
    FOREIGN KEY ([StatusId])
    REFERENCES [dbo].[OrderStatuses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_OrderStatuses'
CREATE INDEX [IX_FK_Orders_OrderStatuses]
ON [dbo].[Orders]
    ([StatusId]);
GO

-- Creating foreign key on [ShipmentId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Orders_Shipments]
    FOREIGN KEY ([ShipmentId])
    REFERENCES [dbo].[Shipments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Orders_Shipments'
CREATE INDEX [IX_FK_Orders_Shipments]
ON [dbo].[Orders]
    ([ShipmentId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------