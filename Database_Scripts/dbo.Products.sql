CREATE TABLE [dbo].[Products] (
    [ProductId]    INT             NOT NULL,
    [ProductName]  NVARCHAR (100)  NOT NULL,
    [UnitPrice]    DECIMAL (18, 2) NOT NULL,
    [UnitsInStock] INT             NOT NULL,
    [ReorderLevel] INT             NOT NULL,
    [Discontinued] BINARY (50)     DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

