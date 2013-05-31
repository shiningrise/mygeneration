/* SQL Insert Script Provided by DbaMgr2k  0.12.0                                                                         */
/*                                                                                                                        */
/* Generated # 9/4/2005 10:54:18 AM #                                                                                     */
/* Original database  # AggregateDb #                                                                                     */
/* Original data table # [dbo].[AggregateTest] #                                                                          */
/*                                                                                                                        */
/* Settings                                                                                                               */
/* Property - Value                                                                                                       */
/* -) SET NOCOUNT ON = True                                                                                               */
/* -) Preserve Identity fields = True                                                                                     */
/* -) Batch size = 1000                                                                                                   */
/* -) SET DATE FORMAT = ymd                                                                                               */
/* -) Date-Time format = {120} ODBC canonical [yyyy-mm-dd hh:mi:ss(24h)]                                                  */
/*                                                                                                                        */
/*                                                                                                                        */
/* You are advised to check the script manually before running it.                                                        */
/* Also, BACK UP YOUR DATABASE before running this script                                                                 */
/* columns of IMAGE and VARIANT data type are not supported for obvious reasons                                           */
/*                                                                                                                        */
SET DATEFORMAT ymd

/* Loading data                                                                                                           */

SET NOCOUNT ON

SET IDENTITY_INSERT [dbo].[AggregateTest] ON

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 1, 1, 'Fred', 'McDonald', 39, '2002-09-20 00:00:00', 36.7200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 2, 2, 'Michelle', 'Johnson', 35, '2004-03-05 00:00:00', 7.2700, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 3, 3, 'Paul', 'Rapaport', 50, '1998-09-29 00:00:00', 6.4800, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 4, 3, 'William', 'Gellar', 22, '2004-02-24 00:00:00', 11.3600, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 5, 4, 'Sarah', 'Jones', 36, '1998-09-18 00:00:00', 10.5700, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 6, 1, 'Sally', 'Johnson', 16, '2000-04-12 00:00:00', 9.7700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 7, 5, 'Fred', 'Costner', 23, '1998-09-08 00:00:00', 14.6500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 8, 2, 'Sally', 'Gellar', 37, '2000-04-02 00:00:00', 13.8600, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 9, 4, 'John', 'Jones', 17, '2001-10-26 00:00:00', 13.0700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 10, 3, 'Paul', 'Vincent', 24, '2000-03-23 00:00:00', 17.9500, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 11, 4, 'John', 'Costner', 39, '2001-10-16 00:00:00', 17.1500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 12, 4, 'Sarah', 'Johnson', 46, '2000-03-13 00:00:00', 22.0300, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 13, 1, 'David', 'Rapaport', 25, '2001-10-06 00:00:00', 21.2400, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 14, 3, 'Mary', 'Doe', 40, '2003-05-01 00:00:00', 20.4500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 15, 2, 'Sally', 'Jones', 47, '2001-09-26 00:00:00', 25.3300, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 16, 3, 'Jane', 'McDonald', 26, '2003-04-21 00:00:00', 24.5300, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 17, 5, 'Michelle', 'Rapaport', 41, '2004-11-13 00:00:00', 23.7400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 18, 4, 'John', 'Douglas', 48, '2003-04-11 00:00:00', 28.6200, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 19, 2, 'William', 'Jones', 27, '2004-11-03 00:00:00', 27.8300, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 20, 5, 'David', 'Doe', 34, '2003-04-01 00:00:00', 32.7100, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 21, 2, 'William', 'Costner', 49, '2004-10-24 00:00:00', 31.9200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 22, 4, 'Fred', 'Smith', 28, '1999-05-20 00:00:00', 31.1200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 23, 3, 'Jane', 'Rapaport', 36, '2004-10-14 00:00:00', 36.0000, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 24, 5, 'Michelle', 'Doe', 15, '1999-05-10 00:00:00', 35.2100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 25, 2, 'Paul', 'Gellar', 30, '2000-12-02 00:00:00', 34.4200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 26, 1, 'Michelle', 'McDonald', 37, '1999-04-30 00:00:00', 39.3000, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 27, 3, 'Sarah', 'Vincent', 16, '2000-11-22 00:00:00', 38.5000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 28, 2, 'William', 'Smith', 23, '1999-04-20 00:00:00', 43.3800, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 29, 4, 'Fred', 'Johnson', 38, '2000-11-12 00:00:00', 42.5900, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 30, 1, 'Sally', 'McDonald', 17, '2002-06-07 00:00:00', 41.8000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 31, 4, 'Fred', 'Gellar', 24, '2000-11-01 00:00:00', 46.6800, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 32, 2, 'Paul', 'Smith', 39, '2002-05-27 00:00:00', 45.8900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 33, 3, 'John', 'Johnson', 18, '2003-12-20 00:00:00', 45.0900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 34, 3, 'Sarah', 'Doe', 25, '2002-05-17 00:00:00', 49.9700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 35, 4, 'David', 'Gellar', 40, '2003-12-10 00:00:00', 49.1800, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 36, 3, 'Sarah', 'McDonald', 47, '2002-05-07 00:00:00', 9.0600, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 37, 5, 'Sally', 'Vincent', 27, '2003-11-30 00:00:00', 8.2700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 38, 2, 'Mary', 'Costner', 41, '1998-06-25 00:00:00', 7.4700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 39, 2, 'Paul', 'Johnson', 48, '2003-11-20 00:00:00', 12.3500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 40, 3, 'John', 'Rapaport', 28, '1998-06-15 00:00:00', 11.5600, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 41, 4, 'Michelle', 'Vincent', 42, '2000-01-08 00:00:00', 10.7700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 42, 4, 'David', 'Jones', 49, '1998-06-05 00:00:00', 15.6500, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 43, 1, 'William', 'Johnson', 29, '1999-12-29 00:00:00', 14.8500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 44, 5, 'David', 'Costner', 36, '1998-05-26 00:00:00', 19.7400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 45, 2, 'Mary', 'Gellar', 15, '1999-12-19 00:00:00', 18.9400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 46, 3, 'Fred', 'Jones', 30, '2001-07-13 00:00:00', 18.1500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 47, 3, 'John', 'Doe', 37, '1999-12-09 00:00:00', 23.0300, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 48, 2, 'Paul', 'Douglas', 31, '2003-01-26 00:00:00', 21.4400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 49, 1, 'William', 'Rapaport', 38, '2001-06-23 00:00:00', 26.3200, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 50, 2, 'Sarah', 'Doe', 18, '2003-01-16 00:00:00', 25.5300, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 51, 2, 'Mary', 'Jones', 25, '2001-06-13 00:00:00', 30.4100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 52, 3, 'Fred', 'McDonald', 39, '2003-01-06 00:00:00', 29.6200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 53, 5, 'Sally', 'Vincent', 19, '2004-07-31 00:00:00', 28.8200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 54, 4, 'Michelle', 'Douglas', 26, '2002-12-27 00:00:00', 33.7000, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 55, 1, 'Paul', 'Johnson', 40, '2004-07-21 00:00:00', 32.9100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 56, 3, 'John', 'McDonald', 20, '1999-02-13 00:00:00', 32.1200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 57, 2, 'Sarah', 'Gellar', 27, '2004-07-10 00:00:00', 37.0000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 58, 4, 'David', 'Smith', 41, '1999-02-03 00:00:00', 36.2000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 59, 3, 'Fred', 'Vincent', 48, '2004-06-30 00:00:00', 41.0900, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 60, 4, 'Sally', 'Doe', 28, '1999-01-24 00:00:00', 40.2900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 61, 2, 'Jane', 'Gellar', 43, '2000-08-18 00:00:00', 39.5000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 62, 1, 'Paul', 'McDonald', 50, '1999-01-14 00:00:00', 44.3800, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 63, 3, 'John', 'Vincent', 29, '2000-08-08 00:00:00', 43.5900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 64, 4, 'Michelle', 'Costner', 44, '2002-03-03 00:00:00', 42.7900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 65, 3, 'David', 'Johnson', 16, '2000-07-29 00:00:00', 47.6700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 66, 5, 'William', 'Rapaport', 30, '2002-02-21 00:00:00', 46.8800, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 67, 4, 'Sally', 'Gellar', 37, '2000-07-19 00:00:00', 6.7600, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 68, 2, 'Jane', 'Jones', 17, '2002-02-11 00:00:00', 5.9700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 69, 3, 'Fred', 'Johnson', 31, '2003-09-06 00:00:00', 5.1700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 70, 2, 'John', 'Costner', 38, '2002-02-01 00:00:00', 10.0500, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 71, 4, 'Michelle', 'Gellar', 18, '2003-08-27 00:00:00', 9.2600, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 72, 1, 'Paul', 'Jones', 32, '1998-03-22 00:00:00', 8.4700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 73, 5, 'William', 'Vincent', 39, '2003-08-17 00:00:00', 13.3500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 74, 2, 'Sarah', 'Costner', 19, '1998-03-12 00:00:00', 12.5500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 75, 1, 'Mary', 'Johnson', 26, '2003-08-07 00:00:00', 17.4400, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 76, 3, 'Fred', 'Rapaport', 41, '1998-03-02 00:00:00', 16.6400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 77, 4, 'Sally', 'Doe', 20, '1999-09-25 00:00:00', 15.8500, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 78, 4, 'Michelle', 'Jones', 27, '1998-02-20 00:00:00', 20.7300, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 79, 1, 'Paul', 'McDonald', 42, '1999-09-15 00:00:00', 19.9400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 80, 3, 'John', 'Rapaport', 21, '2001-04-09 00:00:00', 19.1400, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 81, 2, 'Sarah', 'Douglas', 28, '1999-09-05 00:00:00', 24.0200, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 82, 3, 'David', 'Jones', 43, '2001-03-29 00:00:00', 23.2300, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 83, 3, 'Fred', 'Doe', 50, '1999-08-25 00:00:00', 28.1100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 84, 4, 'Sally', 'Costner', 29, '2001-03-19 00:00:00', 27.3200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 85, 2, 'Jane', 'Douglas', 44, '2002-10-12 00:00:00', 26.5200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 86, 5, 'Paul', 'Rapaport', 16, '2001-03-09 00:00:00', 31.4000, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 87, 2, 'John', 'Doe', 31, '2002-10-02 00:00:00', 30.6100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 88, 4, 'William', 'Gellar', 45, '2004-04-26 00:00:00', 29.8200, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 89, 3, 'David', 'McDonald', 17, '2002-09-22 00:00:00', 34.7000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 90, 5, 'William', 'Vincent', 32, '2004-04-16 00:00:00', 33.9100, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 91, 4, 'Sally', 'Smith', 39, '2002-09-12 00:00:00', 38.7900, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 92, 1, 'Jane', 'Johnson', 18, '2004-04-06 00:00:00', 37.9900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 93, 3, 'Michelle', 'McDonald', 33, '1998-10-31 00:00:00', 37.2000, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 94, 2, 'John', 'Gellar', 40, '2004-03-27 00:00:00', 42.0800, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 95, 4, 'Michelle', 'Smith', 19, '1998-10-21 00:00:00', 41.2900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 96, 5, 'Sarah', 'Johnson', 34, '2000-05-15 00:00:00', 40.4900, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 97, 4, 'William', 'Doe', 41, '1998-10-11 00:00:00', 45.3700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 98, 2, 'Fred', 'Gellar', 20, '2000-05-05 00:00:00', 44.5800, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 99, 1, 'Jane', 'McDonald', 28, '1998-10-01 00:00:00', 49.4600, 0 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 100, 3, 'Fred', 'Vincent', 42, '2000-04-25 00:00:00', 48.6700, 1 )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 101, NULL, NULL, NULL, NULL, NULL, NULL, NULL )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 102, NULL, NULL, NULL, NULL, NULL, NULL, NULL )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 103, NULL, NULL, NULL, NULL, NULL, NULL, NULL )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 104, NULL, NULL, NULL, NULL, NULL, NULL, NULL )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 105, NULL, NULL, NULL, NULL, NULL, NULL, NULL )

INSERT INTO [dbo].[AggregateTest] ([ID], [DepartmentID], [FirstName], [LastName], [Age], [HireDate], [Salary], [IsActive] )
 VALUES ( 106, 0, '', '', 0, NULL, 0, NULL )

GO
-- Batch termination

SET IDENTITY_INSERT [dbo].[AggregateTest] OFF

SET NOCOUNT OFF

/* Data load has completed                                                                                                */
/* Sql Insert Script Provided by DbaMgr2k  0.12.0                                                                         */

