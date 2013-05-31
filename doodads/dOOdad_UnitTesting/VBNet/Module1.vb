Imports MyGeneration.dOOdads

Module Module1

    Sub Main()

        'If Not POSTGRESQL() Then Console.WriteLine("PostgreSQL failed")
        'If Not ORACLE() Then Console.WriteLine("ORACLE failed")
        'If Not ACCESS() Then Console.WriteLine("ACCESS failed")
        'If Not VISTADB() Then Console.WriteLine("VISTADB failed")
        If Not SQL() Then Console.WriteLine("SQL failed")
        'If Not FIREBIRD1() Then Console.WriteLine("FIREBIRD1 failed")
        'If Not FIREBIRD3() Then Console.WriteLine("FIREBIRD3 failed")
        'If Not FIREBIRD3_TRUE() Then Console.WriteLine("FIREBIRD3_TRUE failed")
        'If Not MYSQL() Then Console.WriteLine("MYSQL failed")
        'If Not SQLite() Then Console.WriteLine("SQLite failed")

        Console.WriteLine("Done")
        Console.ReadLine()

    End Sub

    Function SQL() As Boolean

        Try

            ' LoadAll
            Dim emps As New SQL.Employees
            emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"

            Dim i As Integer
            i = emps.TestQuery()
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EmployeeID
            emps = New SQL.Employees
            emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New SQL.Employees
            emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New SQL.Employees
            emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New SQL.Employees
            emps1.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New SQL.Employees
            emps2.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New SQL.Employees
            emps.ConnectionString = "User ID=sa;Initial Catalog=Northwind;Data Source=griffo"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function ACCESS() As Boolean

        Try

            ' LoadAll
            Dim emps As New ACCESS.Employees
            emps.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EmployeeID
            emps = New ACCESS.Employees
            emps.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New ACCESS.Employees
            emps.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New ACCESS.Employees
            emps.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New ACCESS.Employees
            emps1.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New ACCESS.Employees
            emps2.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New ACCESS.Employees
            emps.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=C:\Access\NewNorthwind.mdb;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function ORACLE() As Boolean

        Try

            ' LoadAll
            Dim emps As New ORACLE.Employees
            emps.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EMPLOYEE_ID
            emps = New ORACLE.Employees
            emps.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New ORACLE.Employees
            emps.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            emps.AddNew()
            emps.FIRST_NAME = "trella1"
            emps.LAST_NAME = "trella1"
            emps.AddNew()
            emps.FIRST_NAME = "trella2"
            emps.LAST_NAME = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New ORACLE.Employees
            emps.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LAST_NAME = emps.LAST_NAME + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New ORACLE.Employees
            emps1.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            emps1.AddNew()
            emps1.FIRST_NAME = "trella1_tx1"
            emps1.LAST_NAME = "trella1_tx1"

            Dim emps2 As New ORACLE.Employees
            emps2.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            emps2.AddNew()
            emps2.FIRST_NAME = "trella1_tx2"
            emps2.LAST_NAME = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New ORACLE.Employees
            emps.ConnectionString = "Password=sa;User ID=GRIFFO;Data Source=dbMeta;"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function POSTGRESQL() As Boolean

        Try

            ' LoadAll
            Dim emps As New POSTGRESQL.Employees
            emps.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.Employeeid
            emps = New POSTGRESQL.Employees
            emps.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New POSTGRESQL.Employees
            emps.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            emps.AddNew()
            emps.Firstname = "trella1"
            emps.Lastname = "trella1"
            emps.AddNew()
            emps.Firstname = "trella2"
            emps.Lastname = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New POSTGRESQL.Employees
            emps.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            emps.Where.Firstname.Value = "trella%"
            emps.Where.Firstname.Operator = WhereParameter.Operand.Like_
            emps.Where.Lastname.Value = "trella%"
            emps.Where.Lastname.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.Lastname = emps.Lastname + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New POSTGRESQL.Employees
            emps1.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            emps1.AddNew()
            emps1.Firstname = "trella1_tx1"
            emps1.Lastname = "trella1_tx1"

            Dim emps2 As New POSTGRESQL.Employees
            emps2.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            emps2.AddNew()
            emps2.Firstname = "trella1_tx2"
            emps2.Lastname = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New POSTGRESQL.Employees
            emps.ConnectionString = "Server=www.punktech.com;User Id=postgres;Password=password;Database=griffo;port=5432;"
            emps.Where.Firstname.Value = "trella%"
            emps.Where.Firstname.Operator = WhereParameter.Operand.Like_
            emps.Where.Lastname.Value = "trella%"
            emps.Where.Lastname.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function VISTADB() As Boolean

        Try
            ' LoadAll
            Dim emps As New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps.Query.AddResultColumn(emps.ColumnNames.EmployeeID)
            emps.Query.AddResultColumn(emps.ColumnNames.LastName)
            emps.Query.AddOrderBy(emps.ColumnNames.LastName, WhereParameter.Dir.ASC)
            emps.Query.Top = 2
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            ' LoadAll
            emps = New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EmployeeID
            emps = New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New VISTADB.Employees
            emps1.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New VISTADB.Employees
            emps2.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New VISTADB.Employees
            emps.ConnectionString = "DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function FIREBIRD1() As Boolean

        Try

            ' LoadAll
            Dim emps As New FIREBIRD1.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EMPLOYEE_ID
            emps = New FIREBIRD1.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New FIREBIRD1.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            emps.AddNew()
            emps.FIRST_NAME = "trella1"
            emps.LAST_NAME = "trella1"
            emps.AddNew()
            emps.FIRST_NAME = "trella2"
            emps.LAST_NAME = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New FIREBIRD1.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LAST_NAME = emps.LAST_NAME + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New FIREBIRD1.Employees
            emps1.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            emps1.AddNew()
            emps1.FIRST_NAME = "trella1_tx1"
            emps1.LAST_NAME = "trella1_tx1"

            Dim emps2 As New FIREBIRD1.Employees
            emps2.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            emps2.AddNew()
            emps2.FIRST_NAME = "trella1_tx2"
            emps2.LAST_NAME = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New FIREBIRD1.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP1.FBD;User=SYSDBA;Password=masterkey;Dialect=1;Server=griffo"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function FIREBIRD3() As Boolean

        Try

            ' LoadAll
            Dim emps As New FIREBIRD3.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EMPLOYEE_ID
            emps = New FIREBIRD3.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New FIREBIRD3.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.AddNew()
            emps.FIRST_NAME = "trella1"
            emps.LAST_NAME = "trella1"
            emps.AddNew()
            emps.FIRST_NAME = "trella2"
            emps.LAST_NAME = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New FIREBIRD3.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LAST_NAME = emps.LAST_NAME + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New FIREBIRD3.Employees
            emps1.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps1.AddNew()
            emps1.FIRST_NAME = "trella1_tx1"
            emps1.LAST_NAME = "trella1_tx1"

            Dim emps2 As New FIREBIRD3.Employees
            emps2.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps2.AddNew()
            emps2.FIRST_NAME = "trella1_tx2"
            emps2.LAST_NAME = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New FIREBIRD3.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMP3.FBD;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.Where.FIRST_NAME.Value = "trella%"
            emps.Where.FIRST_NAME.Operator = WhereParameter.Operand.Like_
            emps.Where.LAST_NAME.Value = "trella%"
            emps.Where.LAST_NAME.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function FIREBIRD3_TRUE() As Boolean

        Try

            ' LoadAll
            Dim emps As New FIREBIRD3_TRUE.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EmployeeID
            emps = New FIREBIRD3_TRUE.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New FIREBIRD3_TRUE.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New FIREBIRD3_TRUE.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New FIREBIRD3_TRUE.Employees
            emps1.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New FIREBIRD3_TRUE.Employees
            emps2.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New FIREBIRD3_TRUE.Employees
            emps.ConnectionString = "Database=C:\dOOdad_UnitTesting\EMPTRUE3.FDB;User=SYSDBA;Password=masterkey;Dialect=3;Server=griffo"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function MYSQL()

        Try
            ' LoadAll
            Dim emps As New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps.Query.AddResultColumn(emps.ColumnNames.EmployeeID)
            emps.Query.AddResultColumn(emps.ColumnNames.LastName)
            emps.Query.AddOrderBy(emps.ColumnNames.LastName, WhereParameter.Dir.ASC)
            emps.Query.Top = 2
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If



            ' LoadAll
            emps = New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As System.UInt32 = emps.EmployeeID
            emps = New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New MYSQL4.employee
            emps1.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New MYSQL4.employee
            emps2.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New MYSQL4.employee
            emps.ConnectionString = "Database=Test;Data Source=Griffo;User Id=anonymous;"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

    Function SQLite() As Boolean

        Try
            ' LoadAll
            Dim vemps As New SQLite.vwEmployees
            vemps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            vemps.Query.AddResultColumn(vemps.ColumnNames.EmployeeID)
            vemps.Query.AddResultColumn(vemps.ColumnNames.LastName)
            vemps.Query.AddOrderBy(vemps.ColumnNames.LastName, WhereParameter.Dir.ASC)
            vemps.Query.Top = 2
            If Not vemps.Query.Load() Then
                Return False ' ERROR
            End If

            ' LoadAll
            Dim emps As New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps.Query.AddResultColumn(emps.ColumnNames.EmployeeID)
            emps.Query.AddResultColumn(emps.ColumnNames.LastName)
            emps.Query.AddOrderBy(emps.ColumnNames.LastName, WhereParameter.Dir.ASC)
            emps.Query.Top = 2
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            ' LoadAll
            emps = New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            If Not emps.LoadAll() Then
                Return False ' ERROR
            End If

            ' LoadByPrimaryKey
            Dim id As Integer = emps.EmployeeID
            emps = New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            If Not emps.LoadByPrimaryKey(id) Then
                Return False ' ERROR
            End If

            ' AddNew/Save
            emps = New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps.AddNew()
            emps.FirstName = "trella1"
            emps.LastName = "trella1"
            emps.AddNew()
            emps.FirstName = "trella2"
            emps.LastName = "trella2"
            emps.Save()

            ' Query.Load/Update/Save
            emps = New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            Do
                emps.LastName = emps.LastName + ":new"
            Loop Until Not emps.MoveNext

            emps.Save()

            ' Transaction
            Dim emps1 As New SQLite.Employees
            emps1.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps1.AddNew()
            emps1.FirstName = "trella1_tx1"
            emps1.LastName = "trella1_tx1"

            Dim emps2 As New SQLite.Employees
            emps2.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps2.AddNew()
            emps2.FirstName = "trella1_tx2"
            emps2.LastName = "trella1_tx2"

            TransactionMgr.ThreadTransactionMgr().BeginTransaction()
            emps1.Save()
            emps2.Save()
            TransactionMgr.ThreadTransactionMgr().CommitTransaction()

            ' Query.Load/MarkAsDeleted/Save
            emps = New SQLite.Employees
            emps.ConnectionString = "Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3"
            emps.Where.FirstName.Value = "trella%"
            emps.Where.FirstName.Operator = WhereParameter.Operand.Like_
            emps.Where.LastName.Value = "trella%"
            emps.Where.LastName.Operator = WhereParameter.Operand.Like_
            If Not emps.Query.Load() Then
                Return False ' ERROR
            End If

            emps.DeleteAll()
            emps.Save()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return False
        End Try

        TransactionMgr.ThreadTransactionMgrReset()
        Return True

    End Function

End Module
