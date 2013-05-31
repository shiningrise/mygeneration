Public Class Connections

	Public Shared PostgreSQLConnection = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=griffinski;Database=Aggregates;"
	Public Shared VistaDBConnection = "DataSource=C:\EntitySpaces\dOOdads\dOOdads\Tests\CSharp\VistaDb\Aggregates.vdb;Cypher= None;Password=;Exclusive=False;Readonly=False;"
	Public Shared SQLConnection = "Data Source=VPR_DNP;Initial Catalog=AggregateDb;Integrated Security=sspi"
	Public Shared AccessConnection = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Databases\AggregateDb.mdb;Persist Security Info=False"
	Public Shared SQLiteConnection = "Data Source=C:\Databases\SQLite\AggregateDb.db3;New=False;Compress=True;Synchronous=Off;Version=3"
	Public Shared MySql4Connection = "Database=aggregatedb;Data Source=localhost;User Id=TestUser;Password=TestUser;"
	Public Shared FirebirdConnection = "Database=C:\EntitySpaces\dOOdads\dOOdads\Tests\CSharp\FireBird\AGGREGATES.FDB;User=SYSDBA;Password=;Dialect=3;Server=localhost"
	Public Shared OracleConnection = "Password=retina;Persist Security Info=True;User ID=MyGeneration;Data Source=orcl"

End Class
