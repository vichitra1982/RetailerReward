CREATE TABLE [dbo].[Customers]
(
	[CustomerId] INT NOT NULL PRIMARY KEY, 
    [CustomerName] NVARCHAR(50) NOT NULL, 
    [ContactName] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(400) NOT NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [State] NVARCHAR(50) NOT NULL, 
    [Country] NVARCHAR(50) NOT NULL, 
    [PostalCode] NVARCHAR(10) NOT NULL
)
