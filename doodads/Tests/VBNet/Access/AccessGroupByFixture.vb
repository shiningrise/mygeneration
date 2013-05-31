Imports System
Imports NUnit.Framework
Imports MyGeneration.dOOdads
Imports System.Data.OleDb

Namespace MyGeneration.dOOdads.Tests.Access

	<TestFixture()> _
	Public Class AccessGroupByFixture

		Dim aggTest As AggregateTest = New AggregateTest

		<TestFixtureSetUp()> _
		Public Sub Init()
			TransactionMgr.ThreadTransactionMgrReset()
			aggTest.ConnectionString = Connections.AccessConnection
		End Sub

		<SetUp()> _
		Public Sub Init2()
			aggTest.FlushData()
		End Sub

		<Test()> _
		Public Sub OneGroupBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(2, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub TwoGroupBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(12, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub GroupByWithWhere()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub


		<Test()> _
		Public Sub GroupByWithWhereAndOrderBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.DepartmentID, WhereParameter.Dir.ASC)
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.IsActive, WhereParameter.Dir.ASC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub GroupByWithOrderByCountAll()

			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		  Public Sub GroupByWithTop()

			aggTest.Query.Top = 3
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(aggTest.Query.Top, aggTest.RowCount)
		End Sub

		<Test()> _
		  Public Sub GroupByWithDistinct()

			aggTest.Query.Distinct = True
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub GroupByWithTearoff()
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum
			aggTest.Aggregate.Salary.Alias = "Sum"
			Dim ap As AggregateParameter = aggTest.Aggregate.TearOff.Salary
			ap.Function = AggregateParameter.Func.Min
			ap.Alias = "Min"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test(), ExpectedException(GetType(OleDbException))> _
		Public Sub GroupByWithRollup()
			aggTest.Query.WithRollup = True
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			aggTest.Query.Load()
		End Sub

	End Class

End Namespace
