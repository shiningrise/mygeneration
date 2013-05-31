using System;
using NUnit.Framework;
using MyGeneration.dOOdads;

namespace MyGeneration.dOOdads.Tests.Oracle
{
	[TestFixture]
	public class OracleGroupByFixture
	{
		AggregateTest aggTest = new AggregateTest();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "OracleConnection";
		}

		[SetUp]
		public void Init2()
		{
			aggTest.FlushData();
		}

		[Test]
		public void OneGroupBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(3, aggTest.RowCount);
		}

		[Test]
		public void TwoGroupBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(12, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhere()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhereAndOrderBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.DEPARTMENTID, WhereParameter.Dir.ASC);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.ISACTIVE, WhereParameter.Dir.ASC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithOrderByCountAll()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithTop()
		{
			aggTest.Query.Top = 3;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(aggTest.Query.Top, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithDistinct()
		{
			aggTest.Query.Distinct = true;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithTearoff()
		{
			aggTest.Aggregate.SALARY.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.SALARY.Alias = "Sum";
			AggregateParameter ap = aggTest.Aggregate.TearOff.SALARY;
			ap.Function = AggregateParameter.Func.Min;
			ap.Alias = "Min";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Where.ISACTIVE.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.ISACTIVE.Value = 1;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithRollup()
		{
			aggTest.Query.WithRollup = true;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.ISACTIVE);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DEPARTMENTID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			aggTest.Query.Load();
		}
	}
}
