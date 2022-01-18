--Given a date criteria we will work backwards
DECLARE @date DATE = '3/20/2021';

--using a recurssive Common Table expression to create the date ranges for the past 3 months
--with startDate of that month and endDate of that month calculated from the above @date variable
WITH MonthsCTE AS (
    SELECT 1 [Month], DATEADD(DAY, -DATEPART(DAY, @date)+1, @date) as 'MonthStart', EOMonth(@date) as 'MonthEnd'
    UNION ALL
    SELECT [Month] + 1, DATEADD(MONTH, -1, MonthStart) as 'MonthStart', EOMonth(DATEADD(MONTH, -1, MonthStart)) as 'MonthEnd'
    FROM MonthsCTE 
    WHERE [Month] < 3 ),
--customerdata holds the data from Customers, Orders and OrderDetails
--based on the list of start and end ranges available
CustomerData as (
    SELECT cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd, Sum(orddet.UnitPrice * orddet.Quantity) as Total 
    FROM MonthsCTE cte,
        Customers cus,
	    Orders ord,
	    OrderDetails orddet
    where cus.CustomerId = ord.CustomerId
    and ord.OrderId = orddet.OrderId
    and ord.OrderDate between cte.MonthStart and cte.MonthEnd
    Group By cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd
    )
SELECT  CustomerId, 
        CustomerName, 
        ContactName, 
        MonthStart, 
        MonthEnd, 
        Total, 
        ((Total - 100) * 2) + CAST(FLOOR((Total - 50) / 50) as float) * 50 as Rewards,
        ((Total - 100) * 2) as RewardsLHS,
        CAST(FLOOR((Total - 50) / 50) as float) * 50 as RewardsRHS
FROM    CustomerData;
--SELECT cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd, Sum(orddet.UnitPrice * orddet.Quantity) as Total 
--FROM MonthsCTE cte,F
--    Customers cus,
--	Orders ord,
--	OrderDetails orddet
--where cus.CustomerId = ord.CustomerId
--and ord.OrderId = orddet.OrderId
--and ord.OrderDate between cte.MonthStart and cte.MonthEnd
--Group By cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd