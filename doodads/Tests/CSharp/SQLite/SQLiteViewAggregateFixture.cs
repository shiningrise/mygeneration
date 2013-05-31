using System;
using NUnit.Framework;
using MyGeneration.dOOdads;
using Finisar.SQLite;

namespace MyGeneration.dOOdads.Tests.SQLite
{
	[TestFixture]
	public class SQLiteViewAggregateFixture
	{
		FullNameView aggTest = new FullNameView();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "SQLiteConnection";
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
			Assert.AreEqual(16, aggTest.RowCount);
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
		[ExpectedException(typeof(SQLiteException))]
		public void AddAggregateStdDev()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.StdDev;
			aggTest.Aggregate.Salary.Alias = "Std Dev";
			aggTest.Query.Load();
		}

		[Test]
		[ExpectedException(typeof(SQLiteException))]
		public void AddAggregateVar()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Var;
			aggTest.Aggregate.Salary.Alias = "Var";
			aggTest.Query.Load();
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
		
	}
}
