Imports System
Imports NUnit.Framework
Imports MyGeneration.dOOdads
Imports Finisar.SQLite

Namespace MyGeneration.dOOdads.Tests.SQLite

	<TestFixture()> _
	Public Class SQLiteViewGroupByFixture

		Dim aggTest As FullNameView = New FullNameView

		<TestFixtureSetUp()> _
		Public Sub Init()
			TransactionMgr.ThreadTransactionMgrReset()
			aggTest.ConnectionString = Connections.SQLiteConnection
		End Sub

		<SetUp()> _
		Public Sub Init2()
			aggTest.FlushData()
		End Sub

		<Test()> _
		Public Sub OneGroupBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(1, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub TwoGroupBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub GroupByWithWhere()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub


		<Test()> _
		Public Sub GroupByWithWhereAndOrderBy()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(FullNameView.ColumnNames.DepartmentID, WhereParameter.Dir.ASC)
			aggTest.Query.AddOrderBy(FullNameView.ColumnNames.IsActive, WhereParameter.Dir.ASC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		Public Sub GroupByWithOrderByCountAll()

			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test()> _
		  Public Sub GroupByWithTop()

			aggTest.Query.Top = 3
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(aggTest.Query.Top, aggTest.RowCount)
		End Sub

		<Test()> _
		  Public Sub GroupByWithDistinct()

			aggTest.Query.Distinct = True
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
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
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			Assert.IsTrue(aggTest.Query.Load())
			Assert.AreEqual(5, aggTest.RowCount)
		End Sub

		<Test(), ExpectedException(GetType(SQLiteException))> _
		Public Sub GroupByWithRollup()
			aggTest.Query.WithRollup = True
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID)
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC)
			aggTest.Query.Load()
		End Sub

		<Test()> _
		Public Sub GroupByFullName()
			aggTest.Query.CountAll = True
			aggTest.Query.CountAllAlias = "Count"
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.FullName)
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.FullName)
			Assert.IsTrue(aggTest.Query.Load())
		End Sub

	End Class

End Namespace
