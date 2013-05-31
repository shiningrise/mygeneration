using System;
using System.Data;
using NUnit.Framework;
using MyGeneration.dOOdads;

namespace MyGeneration.dOOdads.Tests.Oracle
{
	[TestFixture]
	public class OracleAggregateFixture
	{
		Oracle.AggregateTest aggTest = new Oracle.AggregateTest();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "OracleConnection";
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
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Avg;
			aggTest.Aggregate.SALARY.Alias = "Avg";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateCount()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Count;
			aggTest.Aggregate.SALARY.Alias = "Count";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateMin()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Min;
			aggTest.Aggregate.SALARY.Alias = "Min";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateMax()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Max;
			aggTest.Aggregate.SALARY.Alias = "Max";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateSum()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.SALARY.Alias = "Sum";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateStdDev()
		{
			aggTest.Aggregate.AGE.Function = AggregateParameter.Func.StdDev;
			aggTest.Aggregate.AGE.Alias = "Std Dev";
			Assert.IsTrue(aggTest.Query.Load());
		    Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AddAggregateVar()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Var;
			aggTest.Aggregate.SALARY.Alias = "Var";
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
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.SALARY.Alias = "Sum";
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void AggregateWithTearoff()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.SALARY.Alias = "Sum";
			AggregateParameter ap = aggTest.Aggregate.TearOff.SALARY;
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
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = true;
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}
		
		[Test]
		public void EmptyAliasUsesColumnName()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Sum;
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual("SALARY", aggTest.Aggregate.SALARY.Alias);
		}
		
		[Test]
		public void DistinctAggregate()
		{
			aggTest.Aggregate.LASTNAME.Function = AggregateParameter.Func.Count;
			aggTest.Aggregate.LASTNAME.Distinct = true;
			Assert.IsTrue(aggTest.Query.Load());
			DataTable testTable = aggTest.DefaultView.Table;
			DataRow[] currRows = testTable.Select(null, null, DataViewRowState.CurrentRows);
			DataRow testRow = currRows[0];
			Assert.AreEqual(10, testRow[0]);
		}

	}
}
