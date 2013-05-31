using System;
using System.Data;
using NUnit.Framework;
using MyGeneration.dOOdads;
using MySql.Data.MySqlClient;

namespace MyGeneration.dOOdads.Tests.MySql
{
	[TestFixture]
	public class MySqlAggregateFixture
	{
		AggregateTest aggTest = new AggregateTest();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "MySqlConnection";
			UnitTestBase.RefreshDatabase();
		}

		[SetUp]
		public void Init2()
		{
			aggTest.FlushData();
		}

		[Test]
		public void EmptyQueryReturnsSELLECTAll()
		{
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(30, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateAvg()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Avg;
			aggTest.Aggregate.Salary.Alias = "Avg";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateCount()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Count;
			aggTest.Aggregate.Salary.Alias = "Count";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateMin()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Min;
			aggTest.Aggregate.Salary.Alias = "Min";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateMax()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Max;
			aggTest.Aggregate.Salary.Alias = "Max";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateSum()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.Salary.Alias = "Sum";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateStdDev()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.StdDev;
			aggTest.Aggregate.Salary.Alias = "Std Dev";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateVar()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Var;
			aggTest.Aggregate.Salary.Alias = "Var";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateCountAll()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Total";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddTwoAggregates()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Total";
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.Salary.Alias = "Sum";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AggregateWithTearoff()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.Salary.Alias = "Sum";
			AggregateParameter ap = aggTest.Aggregate.TearOff.Salary;
			ap.Function = AggregateParameter.Func.Min;
			ap.Alias = "Min";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AggregateWithWhere()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Total";
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}
		
		[Test]
		public void EmptyAliasUsesColumnName()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum;
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual("Salary", aggTest.Aggregate.Salary.Alias);
		}
		
		[Test]
		public void DistinctAggregate()
		{
			aggTest.Aggregate.LastName.Function = AggregateParameter.Func.Count;
			aggTest.Aggregate.LastName.Distinct = true;
			Assert.IsTrue(aggTest.Query.Load());
			DataTable testTable = aggTest.DefaultView.Table;
			DataRow[] currRows = testTable.Select(null, null, DataViewRowState.CurrentRows);
			DataRow testRow = currRows[0];
			Assert.AreEqual(10, testRow[0]);
		}

	}
}
