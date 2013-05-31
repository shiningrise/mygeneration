using System;
using MyGeneration.dOOdads;

namespace CSharp
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if(!SQL())				Console.WriteLine("SQL Failed");
			if(!ACCESS())			Console.WriteLine("ACCESS Failed");
			if(!ORACLE())			Console.WriteLine("ORACLE Failed");	
			if(!VISTADB())			Console.WriteLine("VISTADB Failed");
			if(!FIREBIRD_1())		Console.WriteLine("FIREBIRD_1 Failed");
			if(!FIREBIRD_3())		Console.WriteLine("FIREBIRD_3 Failed");
			if(!FIREBIRD_3_TRUE())	Console.WriteLine("FIREBIRD_3_TRUE Failed");
			if(!POSTGRESQL())		Console.WriteLine("POSTGRESQL Failed");
			if(!MYSQL4())			Console.WriteLine("MYSQL4 Failed");
			if(!SQLITE())			Console.WriteLine("SQLITE Failed");

			Console.WriteLine("DONE");
			Console.ReadLine();
		}


		static bool SQL()
		{
			try
			{
				// LoadAll
				CSharp.SQL.Employees emps = new CSharp.SQL.Employees();
				emps.FlushData();
				emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EmployeeID;
				emps = new CSharp.SQL.Employees();
				emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.SQL.Employees();
				emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.SQL.Employees();
				emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.SQL.Employees emps1 = new CSharp.SQL.Employees();
				emps1.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.SQL.Employees emps2 = new CSharp.SQL.Employees();
				emps2.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.SQL.Employees();
				emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool ACCESS()
		{
			try
			{
				// LoadAll
				CSharp.ACCESS.Employees emps = new CSharp.ACCESS.Employees();
				emps.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EmployeeID;
				emps = new CSharp.ACCESS.Employees();
				emps.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.ACCESS.Employees();
				emps.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.ACCESS.Employees();
				emps.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.ACCESS.Employees emps1 = new CSharp.ACCESS.Employees();
				emps1.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.ACCESS.Employees emps2 = new CSharp.ACCESS.Employees();
				emps2.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.ACCESS.Employees();
				emps.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool ORACLE()
		{
			try
			{
				// LoadAll
				CSharp.ORACLE.EMPLOYEES emps = new CSharp.ORACLE.EMPLOYEES();
				emps.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				decimal id = emps.EMPLOYEE_ID;
				emps = new CSharp.ORACLE.EMPLOYEES();
				emps.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.ORACLE.EMPLOYEES();
				emps.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				emps.AddNew();
				emps.FIRST_NAME = "trella1";
				emps.LAST_NAME  = "trella1";
				emps.AddNew();
				emps.FIRST_NAME = "trella2";
				emps.LAST_NAME  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.ORACLE.EMPLOYEES();
				emps.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LAST_NAME = emps.LAST_NAME + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.ORACLE.EMPLOYEES emps1 = new CSharp.ORACLE.EMPLOYEES();
				emps1.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				emps1.AddNew();
				emps1.FIRST_NAME = "trella1_tx1";
				emps1.LAST_NAME  = "trella1_tx1";

				CSharp.ORACLE.EMPLOYEES emps2 = new CSharp.ORACLE.EMPLOYEES();
				emps2.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				emps2.AddNew();
				emps2.FIRST_NAME = "trella1_tx2";
				emps2.LAST_NAME  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.ORACLE.EMPLOYEES();
				emps.ConnectionString = @"Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool FIREBIRD_1()
		{
			try
			{
				// LoadAll
				CSharp.FIREBIRD.DIALECT1.EMPLOYEES emps = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EMPLOYEE_ID;
				emps = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				emps.AddNew();
				emps.FIRST_NAME = "trella1";
				emps.LAST_NAME  = "trella1";
				emps.AddNew();
				emps.FIRST_NAME = "trella2";
				emps.LAST_NAME  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LAST_NAME = emps.LAST_NAME + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.FIREBIRD.DIALECT1.EMPLOYEES emps1 = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps1.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				emps1.AddNew();
				emps1.FIRST_NAME = "trella1_tx1";
				emps1.LAST_NAME  = "trella1_tx1";

				CSharp.FIREBIRD.DIALECT1.EMPLOYEES emps2 = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps2.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				emps2.AddNew();
				emps2.FIRST_NAME = "trella1_tx2";
				emps2.LAST_NAME  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.FIREBIRD.DIALECT1.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool FIREBIRD_3()
		{
			try
			{
				// LoadAll
				CSharp.FIREBIRD.DIALECT3.EMPLOYEES emps = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EMPLOYEE_ID;
				emps = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.AddNew();
				emps.FIRST_NAME = "trella1";
				emps.LAST_NAME  = "trella1";
				emps.AddNew();
				emps.FIRST_NAME = "trella2";
				emps.LAST_NAME  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LAST_NAME = emps.LAST_NAME + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.FIREBIRD.DIALECT3.EMPLOYEES emps1 = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps1.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps1.AddNew();
				emps1.FIRST_NAME = "trella1_tx1";
				emps1.LAST_NAME  = "trella1_tx1";

				CSharp.FIREBIRD.DIALECT3.EMPLOYEES emps2 = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps2.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps2.AddNew();
				emps2.FIRST_NAME = "trella1_tx2";
				emps2.LAST_NAME  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.FIREBIRD.DIALECT3.EMPLOYEES();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.Where.FIRST_NAME.Value = "trella%";
				emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like;
				emps.Where.LAST_NAME.Value = "trella%";
				emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool FIREBIRD_3_TRUE()
		{
			try
			{
				// LoadAll
				CSharp.FIREBIRD.DIALECT3_TRUE.Employees emps = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				long id = emps.EmployeeID;
				emps = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.FIREBIRD.DIALECT3_TRUE.Employees emps1 = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps1.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.FIREBIRD.DIALECT3_TRUE.Employees emps2 = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps2.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.FIREBIRD.DIALECT3_TRUE.Employees();
				emps.ConnectionString = @"Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool POSTGRESQL()
		{
			try
			{
				// LoadAll
				CSharp.POSTGRESQL.employees emps = new CSharp.POSTGRESQL.employees();
				emps.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EmployeeID;
				emps = new CSharp.POSTGRESQL.employees();
				emps.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.POSTGRESQL.employees();
				emps.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.POSTGRESQL.employees();
				emps.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.POSTGRESQL.employees emps1 = new CSharp.POSTGRESQL.employees();
				emps1.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.POSTGRESQL.employees emps2 = new CSharp.POSTGRESQL.employees();
				emps2.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.POSTGRESQL.employees();
				emps.ConnectionString = @"Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool VISTADB()
		{
			try
			{
				// LoadAll
				CSharp.VISTADB.Employees emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps.Query.AddResultColumn(CSharp.VISTADB.Employees.ColumnNames.EmployeeID);
				emps.Query.AddResultColumn(CSharp.VISTADB.Employees.ColumnNames.LastName);
				emps.Query.AddOrderBy(CSharp.VISTADB.Employees.ColumnNames.LastName, WhereParameter.Dir.ASC);
				emps.Query.Top = 2;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				// LoadAll
				emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				int id = emps.EmployeeID;
				emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.VISTADB.Employees emps1 = new CSharp.VISTADB.Employees();
				emps1.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.VISTADB.Employees emps2 = new CSharp.VISTADB.Employees();
				emps2.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.VISTADB.Employees();
				emps.ConnectionString = @"DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool MYSQL4()
		{
			try
			{
				// LoadAll
				CSharp.MySQL4.employee emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps.Query.AddResultColumn(CSharp.MySQL4.employee.ColumnNames.EmployeeID);
				emps.Query.AddResultColumn(CSharp.MySQL4.employee.ColumnNames.LastName);
				emps.Query.AddOrderBy(CSharp.MySQL4.employee.ColumnNames.LastName, WhereParameter.Dir.ASC);
				emps.Query.Top = 2;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}


				// LoadAll
				emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				uint id = emps.EmployeeID;
				emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.MySQL4.employee emps1 = new CSharp.MySQL4.employee();
				emps1.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.MySQL4.employee emps2 = new CSharp.MySQL4.employee();
				emps2.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.MySQL4.employee();
				emps.ConnectionString = @"Database=Test;Data Source=Griffo;User Id=anonymous;";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}

		static bool SQLITE()
		{
			try
			{
				// LoadAll
				CSharp.SQLite.vwEmployees vemps = new CSharp.SQLite.vwEmployees();
				vemps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3;";
				vemps.Query.AddResultColumn(CSharp.SQLite.Employees.ColumnNames.EmployeeID);
				vemps.Query.AddResultColumn(CSharp.SQLite.Employees.ColumnNames.LastName);
				vemps.Query.AddOrderBy(CSharp.SQLite.Employees.ColumnNames.LastName, WhereParameter.Dir.ASC);
				vemps.Query.Top = 2;
				if(!vemps.Query.Load())
				{
					return false; // ERROR
				}

				// LoadAll
				CSharp.SQLite.Employees emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3;";
				emps.Query.AddResultColumn(CSharp.SQLite.Employees.ColumnNames.EmployeeID);
				emps.Query.AddResultColumn(CSharp.SQLite.Employees.ColumnNames.LastName);
				emps.Query.AddOrderBy(CSharp.SQLite.Employees.ColumnNames.LastName, WhereParameter.Dir.ASC);
				emps.Query.Top = 2;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				// LoadAll
				emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3;";
				if(!emps.LoadAll())
				{
					return false; // ERROR
				}

				// LoadByPrimaryKey
				long id = emps.EmployeeID;
				emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3;";
				if(!emps.LoadByPrimaryKey(id))
				{
					return false; // ERROR
				}

				// AddNew/Save
				emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3";
				emps.AddNew();
				emps.FirstName = "trella1";
				emps.LastName  = "trella1";
				emps.AddNew();
				emps.FirstName = "trella2";
				emps.LastName  = "trella2";
				emps.Save();

				// Query.Load/Update/Save
				emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				do
					emps.LastName = emps.LastName + ":new";
				while(emps.MoveNext());

				emps.Save();

				// Transaction
				CSharp.SQLite.Employees emps1 = new CSharp.SQLite.Employees();
				emps1.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3";
				emps1.AddNew();
				emps1.FirstName = "trella1_tx1";
				emps1.LastName  = "trella1_tx1";

				CSharp.SQLite.Employees emps2 = new CSharp.SQLite.Employees();
				emps2.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3";
				emps2.AddNew();
				emps2.FirstName = "trella1_tx2";
				emps2.LastName  = "trella1_tx2";

				TransactionMgr.ThreadTransactionMgr().BeginTransaction();
				emps1.Save();
				emps2.Save();
				TransactionMgr.ThreadTransactionMgr().CommitTransaction();

				// Query.Load/MarkAsDeleted/Save
				emps = new CSharp.SQLite.Employees();
				emps.ConnectionString = @"Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3";
				emps.Where.FirstName.Value = "trella%";
				emps.Where.FirstName.Operator = WhereParameter.Operand.Like;
				emps.Where.LastName.Value = "trella%";
				emps.Where.LastName.Operator = WhereParameter.Operand.Like;
				if(!emps.Query.Load())
				{
					return false; // ERROR
				}

				emps.DeleteAll();
				emps.Save();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			finally
			{
				TransactionMgr.ThreadTransactionMgrReset();
			}

			return true;
		}
	}
}
