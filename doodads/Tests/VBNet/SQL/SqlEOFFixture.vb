Imports System
Imports NUnit.Framework
Imports MyGeneration.dOOdads

Namespace MyGeneration.dOOdads.Tests.SQL

	<TestFixture()> _
	Public Class SqlEOFFixture
		
		Dim aggTest As AggregateTest = New AggregateTest
		Dim nCounter As Integer = 0

		<TestFixtureSetUp()> _
		Public Sub Init()
			TransactionMgr.ThreadTransactionMgrReset()
			aggTest.ConnectionString = Connections.SQLConnection
		End Sub

		<SetUp()> _
		Public Sub Init2()
			nCounter = 0
			aggTest.FlushData
		End Sub

		<Test()> _
		Public Sub EOFWithNoLoad()
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(0, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithFalseLoad()
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal
			aggTest.Where.IsActive.Value = True
			Dim wp As WhereParameter = aggTest.Where.TearOff.IsActive
			wp.Operator = WhereParameter.Operand.Equal
			wp.Value = False
			aggTest.Query.Load
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(0, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithOneRow()
			aggTest.Query.Top = 1
			aggTest.Query.Load
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(aggTest.Query.Top, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithTwoRows()
			aggTest.Query.Top = 2
			aggTest.Query.Load
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(aggTest.Query.Top, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithLoadAll()
			aggTest.LoadAll
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(aggTest.RowCount, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithFilter()
			aggTest.LoadAll
			aggTest.Filter = "DepartmentID = 3"
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(8, nCounter)
			aggTest.Filter = ""
			nCounter = 0
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(30, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithEmptyFilter()
			aggTest.LoadAll
			aggTest.Filter = "DepartmentID = 99"
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(0, nCounter)
			aggTest.Filter = ""
			nCounter = 0
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(30, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithSort()
			aggTest.LoadAll
			aggTest.Sort = "ID DESC"
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(30, nCounter)
			aggTest.Sort = ""
			nCounter = 0
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(30, nCounter)
		End Sub

		<Test()> _
		Public Sub EOFWithGroupBy()
			aggTest.Query.CountAll = True
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID)
			aggTest.Query.Load
			While Not aggTest.EOF AndAlso nCounter < 35
				nCounter = nCounter + 1
				aggTest.MoveNext
			End While
			Assert.AreEqual(7, nCounter)
		End Sub
	End Class 
End Namespace
