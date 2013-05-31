Imports System
Imports NUnit.Framework
Imports MyGeneration.dOOdads

Namespace MyGeneration.dOOdads.Tests.SQL

	<TestFixture()> _
	Public Class SqlXmlFixture
		
		Dim aggTest As AggregateTest = New AggregateTest
		Dim aggClone As AggregateTest = New AggregateTest

		<TestFixtureSetUp()> _
		Public Sub Init()
			TransactionMgr.ThreadTransactionMgrReset()
			aggTest.ConnectionString = Connections.SQLConnection
		End Sub

		<SetUp()> _
		Public Sub Init2()
			aggTest.FlushData
		End Sub

		<Test()> _
		Public Sub SerializeDeserialize()
			aggTest.LoadAll
			aggTest.LastName = "Griffinski"
			aggTest.GetChanges
			Dim str As String = aggTest.Serialize
			aggClone.Deserialize(str)
			Assert.AreEqual(1, aggClone.RowCount)
			Assert.AreEqual("Modified", aggClone.RowState.ToString)
			Assert.AreEqual("Griffinski", aggClone.s_LastName)
		End Sub

		<Test()> _
		Public Sub ToXmlFromXml()
			aggTest.LoadAll
			aggTest.LastName = "Griffinski"
			aggTest.GetChanges
			Dim str As String = aggTest.ToXml
			aggClone.FromXml(str)
			Assert.AreEqual(1, aggClone.RowCount)
			Assert.AreEqual("Added", aggClone.RowState.ToString)
			Assert.AreEqual("Griffinski", aggClone.s_LastName)
		End Sub
	End Class 
End Namespace
