===============================================================================
              Welcome to the MyGeneration dOOdads .NET Architecture
                             Copyright (c) 2004, 2005 
                             by MyGeneration Software

              http://www.mygenerationsoftware.com/dOOdads/dOOdads.aspx
===============================================================================
     
The dOOdads architecture is free and available in C# and VB.NET. Currently
dOOdads supports these databases:

1) Microsoft SQL
2) Microsoft Access
3) Oracle
4) PostgreSQL
5) Firebird
6) VistaDB
7) SQLite
8) MySQL

*******************************************************************************
** 1) Compiling dOOdads - A 1 minute exercise
*******************************************************************************

1) Create your Visual Studio Solution.
2) Choose 'Add - Existing Project' and add the dOOdads project to your solution.
3) On your project choose 'Add Reference' and switch to 'Projects' tab and
   choose "MyGeneration.dOOdads".
4) Next drill down into the MyGeneration.dOOdads project and with the DbAdapters
   folder, right mouse on the two files for your DBMS system, choose 'Properties'
   and set the 'Build Action' to 'Compile'.
5) Add the proper ADO.NET managed provider to the dOOdads references section.

   For Example, if you were working with PostgreSQL:
   
   You would select PostgreSqlDynamicQuery.cs and PostgreSqlEntity.cs and set them
   to compile. Next, you would add the Npgsql ADO.NET Managed provider to the 
   'MyGeneration.dOOdads' project references section and compile, it's that easy.
   
   DBMS                  FILES                                     NAMESPACE
   ----------------------------------------------------------------------------
   SQL        (SqlClientDynamicQuery.cs, SqlClientEntity.cs)       System.Data.SqlClient
   Access     (OleDbDynamicQuery.cs, OleDbEntity.cs)               System.Data.OleDb
   Oracle     (OracleClientDynamicQuery.cs, OracleClientEntity.cs) System.Data.OracleClient
   PostgreSql (PostgreSqlDynamicQuery.cs, PostgreSqlEntity.cs)     Npgsql   
   Firebird   (FirebirdSqlDynamicQuery.cs, FirebirdSqlEntity.cs)   FirebirdSql.Data.Firebird
   VistaDB    (VistaDBDynamicQuery.cs, VistaDBEntity.cs)           VistaDB
   SQLite     (SQLiteDynamicQuery.cs, SQLiteEntity.cs)             Finisar.SQLite
   MySQL      (MySQL4DynamicQuery.cs, MySQL4Entity.cs)             MySql.Data.MySqlClient
   
   
*******************************************************************************
** 2) Syntax
******************************************************************************* 

The MyGeneration dOOdads architecture serves up your database data in a highly
intuitive API and is a very simple and straightforward architecture. Below is
a C# sample of the dOOdads API working with Northind's Employee table.


                  //------------------------
                  // Load and Save
                  //------------------------
                  Employees emps = new Employees();
                  if(emps.LoadByPrimaryKey(42))
                  {
                      emps.LastName = "Just Got Married";
                      emps.Save();
                  }

                  //------------------------
                  // Add a new record
                  //------------------------              
                  emps = new Employees();
                  emps.AddNew();
                  emps.FirstName = "Mr.";
                  emps.LastName = "dOOdad";
                  emps.Save();

                  // After save the identity column is already here for me.
                  int i = emps.EmployeeID;

                  //------------------------
                  // Dynamic Query Ability
                  //------------------------             
                  emps = new Employees();

                  emps.Where.LastName.Value = "%A%";
                  emps.Where.LastName.Operator = WhereParameter.Operand.Like;

                  emps.Query.Load();
                  
*******************************************************************************
** 3) Generation
*******************************************************************************

At this point we strongly urge you to go to the dOOdads home page and look at 
the visual of the dOOdads architecture, this will help you in this process.
http://www.mygenerationsoftware.com/dOOdads/dOOdads.aspx

There are three steps (in most cases)

1) Generate your stored procedures
2) Generate your business entity ( _Employees.cs )
3) Generate your concrete class  (  Employees.cs - inherits from _Employees.cs ) 

** Stored Procedures **

  All but the VistaDB, SQLite, and MySQL4 dOOdads work with stored procedures.
  The first step is to generate the stored procedures for your tables/views.
  The best thing to do is generate a dOOdad for one of your tables and follow
  the process, then begin your project once you "get it".

  These are the templates that will generate your stored procedures as found in
  MyGeneration's template browser window.
  
  1) Microsoft SQL      - Microsoft SQL Server.dOOdads Stored Procedures
  2) Microsoft Access   - Microsoft Access.Access Stored Procedures
  3) Oracle             - Oracle.Oracle Stored Procedures
  4) PostgreSQL         - PostgreSQL.PostgreSQL Stored Procedures
  5) Firebird           - Firebird.StoredProcedures.Firebird Stored Procedures
  6) VistaDB            - <none>
  7) SQLite             - <none>
  8) MySQL4             - <none>
  
  Simply open the correct template and execute it.
  
** The Business Entity

  These are the templates that will generate your business entities as found in
  MyGeneration's template browser window. Notice that sometimes the same template
  is used for many DBMS systems. Business Entities are abstract classes. 

  1) Microsoft SQL      - dOOdads.C#.dOOdads Business Entity
  2) Microsoft Access   - dOOdads.C#.dOOdads Business Entity
  3) Oracle             - dOOdads.C#.dOOdads Business Entity
  4) PostgreSQL         - dOOdads.C#.dOOdads Business Entity
  5) Firebird           - dOOdads.C#.dOOdads Business Entity
  6) VistaDB            - dOOdads.VistaDB.C#.VistaDB C# dOOdads Business Entity
  7) SQLite             - dOOdads.SQLite.C#.SQLite C# dOOdads Business Entity
  8) MySQL              - dOOdads.MySQL4.C#.MySQL4 C# dOOdads Business Entity
  
  NOTE: The above tempates are for generating a business entity from a Table, 
        the tempate for generating from a view is 'dOOdads Business View'.
        
** The Concrete Class

  All DBMS sytems used the same tempate, dOOdads.C#.dOOdads Concrete Class.
  The concrete class is where you put all of your business logic and custom
  code.
  
*******************************************************************************
** 4) Handling AutoKeys, Indentity Columns, Sequences, and Generators
*******************************************************************************  

MyGeneration dOOdads works hard to make your DBMS access appear as seemless
as possible, the area of AutoKeys is covered here. The nice thing about dOOdads
is that after you call Save() your autokeys are automatically brought back 
and present in your business entity, in most cases there is enought meta data
for dOOdads to do this without your intervention.


  1) Microsoft SQL      - Automatically detected (no extra work to do)
  2) Microsoft Access   - Automatically detected (no extra work to do)
  3) VistaDB            - Automatically detected (no extra work to do)
  4) PostgreSQL         - Automatically detected (no extra work to do)
  5) SQLite             - Automatically detected (no extra work to do)
  6) MySQL              - Automatically detected (no extra work to do)
      
  7) Oracle - Sequences
  
           These are supported by placing "entries" in the 'Local User Meta' data
           (via "L" icon) at the table level. For instance, if you have a table
           named "EMPLOYEES" and want a sequence to update the EMPLOYEE_ID in that
           table you simply browse to that table in the MyMeta Browser and click
           on it. This will present the the local user meta data in that window
           (open it first). Here are the entries you would make.
           
           Remember, you make these entries at the "Table" level.
           
           Key = SEQ:I:<COLUMN NAME>   // 'I' for INSERT
           Key = SEQ:U:<COLUMN NAME>   // 'U' for UPDATE
           
           Value = <SEQUENCE NAME>           
                      
           Key                Value            Explanation
           ---------------------------------   --------------------------------------
           SEQ:I:EMPLOYEE_ID  EMPLOYEES_SEQ    Will assign EMPLOYEE_ID during INSERT
           SEQ:U:EMPLOYEE_ID  EMPLOYEES_SEQ    Will assign EMPLOYEE_ID during UPDATE
           
           NOTE: Both the stored procedure templates and business entity templates 
                 will obey these entries, so if you add them you would need to regenerate
                 both the stored procedures and business entity.
           
  
  8) Firebird - Generators
  
           These are supported by placing "entries" in the 'Local User Meta' data
           (via "L" icon) at the table level. For instance, if you have a table
           named "EMPLOYEES" and want a generator to update the EMPLOYEE_ID in that
           table you simply browse to that table in the MyMeta Browser and click
           on it. This will present the the local user meta data in that window
           (open it first). Here are the entries you would make.
           
           Remember, you make these entries at the "Table" level.           
           
           Key = GEN:I:<COLUM NAME>     // 'I' for INSERT
           Key = GEN:U:<COLUM NAME>     // 'U' for UPDATE           
           Key = GEN:I:T:<COLUMN NAME>  // 'I' for INSERT, a trigger assigns generator       
           Key = GEN:U:T:<COLUMN NAME>  // 'U' for UPDATE, a trigger assigns generator                
           
           Value = <GENERATOR NAME>
                      
           Key                  Value            Explanation
           ---------------------------------     --------------------------------------
           GEN:I:EMPLOYEE_ID    EMPLOYEES_GEN    Will assign EMPLOYEE_ID during INSERT
           GEN:U:EMPLOYEE_ID    EMPLOYEES_GEN    Will assign EMPLOYEE_ID during UPDATE
           GEN:I:T:EMPLOYEE_ID  EMPLOYEES_GEN    Indicates that a trigger does the update
           GEN:U:T:EMPLOYEE_ID  EMPLOYEES_GEN    Indicates that a trigger does the update         
           
           NOTE: Both the stored procedure templates and business entity templates 
                 will obey these entries, so if you add them you would need to regenerate
                 both the stored procedures and business entity.  

*******************************************************************************
** 4) More Information
******************************************************************************* 

VB.NET Master Sample
http://www.mygenerationsoftware.com/portal/dOOdads/VBNETUsage/tabid/54/Default.aspx

C# Master Sample
http://www.mygenerationsoftware.com/portal/dOOdads/CUsage/tabid/53/Default.aspx

Support Forums (you must register but your information is secure, and never given out)
Post any questions in the 'dOOdads - MyGeneration's .NET Architecture' forum.
http://www.mygenerationsoftware.com/phpbb2/
