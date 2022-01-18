CREATE TABLE [dbo].[OrderDetails]
(
	[OrderId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    [UnitPrice] DECIMAL(18, 2) NOT NULL, 
    [Quantity] INT NOT NULL, 
    PRIMARY KEY ([OrderId], [ProductId]), 
    CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([OrderId]), 
    CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
)
