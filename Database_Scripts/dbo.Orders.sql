CREATE TABLE [dbo].[Orders]
(
	[OrderId] INT NOT NULL PRIMARY KEY, 
    [OrderDate] DATETIME NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [RequiredDate] DATETIME NOT NULL, 
    [ShippedDate] DATETIME NULL, 
    [ShippingAddress1] NVARCHAR(200) NOT NULL, 
    [ShippingAddress2] NVARCHAR(200) NULL, 
    [ShippingCity] NVARCHAR(50) NOT NULL, 
    [ShippingState] NVARCHAR(50) NOT NULL, 
    [ShippingZip] NVARCHAR(10) NOT NULL, 
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId])
)

GO

CREATE INDEX [IX_Orders_OrderDate] ON [dbo].[Orders] ([OrderDate])
