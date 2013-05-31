=============================================================================   
   This demo is targeted for Microsoft SQL's Northwind Database.
=============================================================================   

To run it only takes a few seconds. But first you need to do a few simple
tasks.

1) See the README.TXT file in the MyGeneration.dOOdads project contained
   in MyGeneration\Architectures. You need to enable the SqlClient classes.
  
2) You shoud check the App.Config contained in this solution and make sure it
   is correct for your SQL setup.
   
Query 1 demonstrates the new Aggregate features in dOOdads.
Query 2 demonstrates many of the DynamicQuery capabilities.

You can also use this app to test your own DynamicQueries:

1) Change tmpEnt1 or tmpEnt2 to instantiate your Business Entity.

		'*** Change these to test different Business Entities
		Private tmpEnt1 As Products = New Products
		Private tmpEnt2 As Employees = New Employees

2) Change SetupQuery1() or SetupQuery2() with your query.

If your query succeeds, but does not return the result set you
expected, you can click back and forth between the query and
LoadAll. This may help you determine where the query logic
is failing.

Once you have the query just the way you want it, copy/paste
the code into your app and Search/Replace tmpEnt1 or tmpEnt2
with the instance variable for your Business Entity (e.g., emps).
