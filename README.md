# RetailerReward

A retailer offers a rewards program to its customers, awarding points based on each recorded purchase.

A customer receives 2 points for every dollar spent over $100 in each transaction, plus 1 point for every dollar spent over $50 in each transaction

(e.g. a $120 purchase = 2x$20 + 1x$50 = 90 points).

Given a record of every transaction during a three month period, calculate the reward points earned for each customer per month and total.

• Make up a data set to best demonstrate your solution
• Check solution into GitHub

Since we are using a SQL Express database this app will not connect to APP_DATA folder when it comes to a console application, for this reason we will have to copy the "RetailRewards.mdf and .ldf files to "C:\DataBase" directory for this solution to work as expected.

The DML and work out queries are placed in the "Database_Scripts" folder.  The "Insert Scripts for RetailRewards database.sql" was used to create the data for this solution.

This project is heavily SQL dependent, means all the business logic has been handled in the SQL with the help of CTE (Common Table Expression)

This app will create a UI which will display records hierarchically for each transactional records for a given month.  Since we have data for the year ranging from 1/1/2021 till 3/31/2021, for the query to show up results we have hardcoded a date in CTE.