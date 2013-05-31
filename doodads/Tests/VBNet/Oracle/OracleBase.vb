Imports System
Imports NUnit.Framework
Imports MyGeneration.dOOdads
Imports Npgsql

Namespace MyGeneration.dOOdads.Tests.Oracle

	Public Class UnitTestBase

		Public Shared Sub RefreshDatabase()

			Dim testData As New AggregateTest
			testData.ConnectionString = Connections.OracleConnection

			testData.LoadAll()
			testData.DeleteAll()
			testData.Save()

			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "David"
			testData.s_LASTNAME = "Doe"
			testData.s_AGE = "16"
			testData.s_HIREDATE = "2000-02-16 00:00:00"
			testData.s_SALARY = "34.71"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "1"
			testData.s_FIRSTNAME = "Sarah"
			testData.s_LASTNAME = "McDonald"
			testData.s_AGE = "28"
			testData.s_HIREDATE = "1999-03-25 00:00:00"
			testData.s_SALARY = "11.06"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "David"
			testData.s_LASTNAME = "Vincent"
			testData.s_AGE = "43"
			testData.s_HIREDATE = "2000-10-17 00:00:00"
			testData.s_SALARY = "10.27"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "2"
			testData.s_FIRSTNAME = "Fred"
			testData.s_LASTNAME = "Smith"
			testData.s_AGE = "15"
			testData.s_HIREDATE = "1999-03-15 00:00:00"
			testData.s_SALARY = "15.15"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Sally"
			testData.s_LASTNAME = "Johnson"
			testData.s_AGE = "30"
			testData.s_HIREDATE = "2000-10-07 00:00:00"
			testData.s_SALARY = "14.36"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "5"
			testData.s_FIRSTNAME = "Jane"
			testData.s_LASTNAME = "Rapaport"
			testData.s_AGE = "44"
			testData.s_HIREDATE = "2002-05-02 00:00:00"
			testData.s_SALARY = "13.56"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "Paul"
			testData.s_LASTNAME = "Gellar"
			testData.s_AGE = "16"
			testData.s_HIREDATE = "2000-09-27 00:00:00"
			testData.s_SALARY = "18.44"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "2"
			testData.s_FIRSTNAME = "John"
			testData.s_LASTNAME = "Jones"
			testData.s_AGE = "31"
			testData.s_HIREDATE = "2002-04-22 00:00:00"
			testData.s_SALARY = "17.65"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Michelle"
			testData.s_LASTNAME = "Johnson"
			testData.s_AGE = "45"
			testData.s_HIREDATE = "2003-11-14 00:00:00"
			testData.s_SALARY = "16.86"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "2"
			testData.s_FIRSTNAME = "David"
			testData.s_LASTNAME = "Costner"
			testData.s_AGE = "17"
			testData.s_HIREDATE = "2002-04-11 00:00:00"
			testData.s_SALARY = "21.74"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "William"
			testData.s_LASTNAME = "Gellar"
			testData.s_AGE = "32"
			testData.s_HIREDATE = "2003-11-04 00:00:00"
			testData.s_SALARY = "20.94"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Sally"
			testData.s_LASTNAME = "Rapaport"
			testData.s_AGE = "39"
			testData.s_HIREDATE = "2002-04-01 00:00:00"
			testData.s_SALARY = "25.82"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "5"
			testData.s_FIRSTNAME = "Jane"
			testData.s_LASTNAME = "Vincent"
			testData.s_AGE = "18"
			testData.s_HIREDATE = "2003-10-25 00:00:00"
			testData.s_SALARY = "25.03"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "2"
			testData.s_FIRSTNAME = "Fred"
			testData.s_LASTNAME = "Costner"
			testData.s_AGE = "33"
			testData.s_HIREDATE = "1998-05-20 00:00:00"
			testData.s_SALARY = "24.24"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "1"
			testData.s_FIRSTNAME = "John"
			testData.s_LASTNAME = "Johnson"
			testData.s_AGE = "40"
			testData.s_HIREDATE = "2003-10-15 00:00:00"
			testData.s_SALARY = "29.12"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Michelle"
			testData.s_LASTNAME = "Rapaport"
			testData.s_AGE = "19"
			testData.s_HIREDATE = "1998-05-10 00:00:00"
			testData.s_SALARY = "28.32"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "Sarah"
			testData.s_LASTNAME = "Doe"
			testData.s_AGE = "34"
			testData.s_HIREDATE = "1999-12-03 00:00:00"
			testData.s_SALARY = "27.53"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "William"
			testData.s_LASTNAME = "Jones"
			testData.s_AGE = "41"
			testData.s_HIREDATE = "1998-04-30 00:00:00"
			testData.s_SALARY = "32.41"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "1"
			testData.s_FIRSTNAME = "Sarah"
			testData.s_LASTNAME = "McDonald"
			testData.s_AGE = "21"
			testData.s_HIREDATE = "1999-11-23 00:00:00"
			testData.s_SALARY = "31.62"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "Jane"
			testData.s_LASTNAME = "Costner"
			testData.s_AGE = "28"
			testData.s_HIREDATE = "1998-04-20 00:00:00"
			testData.s_SALARY = "36.50"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "2"
			testData.s_FIRSTNAME = "Fred"
			testData.s_LASTNAME = "Douglas"
			testData.s_AGE = "42"
			testData.s_HIREDATE = "1999-11-13 00:00:00"
			testData.s_SALARY = "35.71"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Paul"
			testData.s_LASTNAME = "Jones"
			testData.s_AGE = "22"
			testData.s_HIREDATE = "2001-06-07 00:00:00"
			testData.s_SALARY = "34.91"
			testData.s_ISACTIVE = "0"
			testData.AddNew()
			testData.s_DEPARTMENTID = "3"
			testData.s_FIRSTNAME = "Michelle"
			testData.s_LASTNAME = "Doe"
			testData.s_AGE = "29"
			testData.s_HIREDATE = "1999-11-03 00:00:00"
			testData.s_SALARY = "39.79"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.s_DEPARTMENTID = "4"
			testData.s_FIRSTNAME = "Paul"
			testData.s_LASTNAME = "Costner"
			testData.s_AGE = "43"
			testData.s_HIREDATE = "2001-05-28 00:00:00"
			testData.s_SALARY = "39.00"
			testData.s_ISACTIVE = "1"
			testData.AddNew()
			testData.AddNew()
			testData.AddNew()
			testData.AddNew()
			testData.AddNew()
			testData.AddNew()
			testData.s_DEPARTMENTID = "0"
			testData.s_FirstName = ""
			testData.s_LastName = ""
			testData.s_Age = "0"
			testData.s_Salary = "0"

			testData.Save()

		End Sub

	End Class

End Namespace
