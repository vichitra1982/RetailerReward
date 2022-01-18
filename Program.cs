using System;
using System.Configuration;
using System.Data.SqlClient;

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
                string sqlCommandText = @"
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
--customerdata holds the data from Customers and Orders
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
                THEN 50
                ELSE 0
            END)
            as Rewards
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
FROM    CustomerData cus,
        Orders ord,
        MonthsCTE cte
where   cus.OrderDate between cte.MonthStart and cte.MonthEnd
and     ord.CustomerId = cus.CustomerId
and     ord.OrderDate = cus.OrderDate
Group by cus.CustomerId, cus.CustomerName, cus.ContactName, cte.MonthStart, cte.MonthEnd;";
                //Creating a sql command to execute the above query
                using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Displaying the headers for customers record
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == 0)
                                Console.Write("\t" + reader.GetName(i) + "\t");
                            else
                                Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine("\n\t_____________________________________________________________________________________________________\n");
                        //Displaying the records for each customer for a month's timeframe
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i == 0)
                                    Console.Write("\t" + reader.GetValue(i) + "\t");
                                else
                                    Console.Write(reader.GetValue(i) + "\t");
                            }
                            Console.WriteLine("\n\t_____________________________________________________________________________________________________\n");
                            int customerId = reader.GetInt32(0);
                            DateTime startDate = reader.GetDateTime(3);
                            DateTime endDate = reader.GetDateTime(4);
                            //Creating a sub-query to loop through each transactions
                            //Assuming there will be an API which will create the json for us in an hierarchical manner.
                            //Because of time crunch I have handled it this way
                            string sqlText = @"
                                select OrderDate, TotalPrice, (CASE WHEN (TotalPrice - 100) > 0 
                                    THEN ((TotalPrice - 100) * 2) 
                                    ELSE 0
                                END +
                                CASE WHEN (TotalPrice - 50) > 0 
                                    THEN 50
                                    ELSE 0
                                END)
                                as Rewards from Orders
                                where CustomerId = @customerId
                                and OrderDate between @startDate and @endDate";
                            //Executing the above query for each customer for the given date range (a month)
                            using (SqlCommand sqlCommand = new SqlCommand(sqlText, connection))
                            {
                                //sqlCommand.Parameters.Clear();
                                sqlCommand.Parameters.Add(new SqlParameter("customerId", customerId));
                                sqlCommand.Parameters.Add(new SqlParameter("startDate", startDate));
                                sqlCommand.Parameters.Add(new SqlParameter("endDate", endDate));
                                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                                {
                                    Console.Write("\t\t\t");
                                    //Displaying the header table for each order details for a given transaction
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        if (i == 0)
                                            Console.Write("\t" + dataReader.GetName(i) + "\t\t");
                                        else
                                            Console.Write(dataReader.GetName(i) + "\t");
                                    }
                                    Console.WriteLine("\n\t_____________________________________________________________________________________________________\n");
                                    //Displaying the rows for each order details for a given transaction
                                    while (dataReader.Read())
                                    {
                                        Console.Write("\t\t\t");
                                        for (int i = 0; i < dataReader.FieldCount; i++)
                                        {
                                            if (i == 0)
                                                Console.Write("\t" + dataReader.GetValue(i) + "\t\t");
                                            else
                                                Console.Write(dataReader.GetValue(i) + "\t\t");
                                        }
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine("\n\t_____________________________________________________________________________________________________\n");
                                }
                            }
                        }
                    }
                }
            }
            Console.Read();
        }
    }
}
