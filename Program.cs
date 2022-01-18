using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RetailerRewardsContext"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlCommandText = @"--Given a date criteria we will work backwards
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
    SELECT  cus.CustomerId, 
            cus.CustomerName, 
            cus.ContactName, 
            ord.OrderId, 
            ord.OrderDate,
            SUM(CASE WHEN (ord.TotalPrice - 100) > 0 
                THEN ((ord.TotalPrice - 100) * 2) 
                ELSE 0
            END +
            CASE WHEN (ord.TotalPrice - 50) > 0 
                --THEN CAST(FLOOR((ord.TotalPrice - 50) / 50) as float) * 50
                THEN 50
                ELSE 0
            END)
            as Rewards
            --SUM(CASE WHEN (ord.TotalPrice - 100) > 0 
            --    THEN ((ord.TotalPrice - 100) * 2) 
            --    ELSE 0
            --END) as RewardsLHS,
            --SUM(CASE WHEN (ord.TotalPrice - 50) > 0 
            --    THEN CAST(FLOOR((ord.TotalPrice - 50) / 50) as float) * 50
            --    ELSE 0
            --END) as RewardsRHS
            
    --FROM MonthsCTE cte,
    FROM    Customers cus,
	        Orders ord
    where cus.CustomerId = ord.CustomerId
    Group By cus.CustomerId, cus.CustomerName, cus.ContactName, ord.OrderId, ord.OrderDate
    )
SELECT  cus.CustomerId, 
        cus.CustomerName, 
        cus.ContactName, 
        cte.MonthStart, 
        cte.MonthEnd, 
        Sum(ord.TotalPrice) as Total, 
        Sum(Rewards) as TotalRewards
        --Sum(RewardsLHS) as TotalRewardsLHS,
        --Sum(RewardsRHS) as TotalRewardsRHS
FROM    CustomerData cus,
        Orders ord,
        MonthsCTE cte
where   cus.OrderDate between cte.MonthStart and cte.MonthEnd
and     ord.CustomerId = cus.CustomerId
and     ord.OrderDate = cus.OrderDate
Group by cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd;";
                using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine();
                        while (reader.Read())
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetValue(i) + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            //Console.WriteLine("Connection string from app.config is " + ConfigurationManager.ConnectionStrings["RetailerRewardsContext"].ConnectionString);
            Console.Read();
        }
    }
}
