using System;
using NUnit.Framework;
using MyGeneration.dOOdads;
using VistaDB;

using MyGeneration.dOOdads.Tests.VistaDB;

namespace MyGeneration.dOOdads.Tests.VistaDB
{
	[TestFixture]
	public class VistaDBGroupByFixture
	{
		AggregateTest aggTest = new AggregateTest();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "VistaDBConnection";
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
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(3, aggTest.RowCount);
		}

		[Test]
		public void TwoGroupBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(12, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhere()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhereAndOrderBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.DepartmentID, WhereParameter.Dir.ASC);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.IsActive, WhereParameter.Dir.ASC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithOrderByCountAll()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
//			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithTop()
		{
			aggTest.Query.Top = 3;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
//			aggTest.Query.AddGroupBy("Count(*)");
//			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Console.WriteLine(aggTest.Query.LastQuery);
			Assert.AreEqual(aggTest.Query.Top, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithDistinct()
		{
			aggTest.Query.Distinct = true;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
//			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load(), aggTest.Query.LastQuery);
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithTearoff()
		{
			aggTest.Aggregate.Salary.Function = AggregateParameter.Func.Sum;
			aggTest.Aggregate.Salary.Alias = "Sum";
			AggregateParameter ap = aggTest.Aggregate.TearOff.Salary;
			ap.Function = AggregateParameter.Func.Min;
			ap.Alias = "Min";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.DepartmentID, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
//		[ExpectedException(typeof(VistaDBException))]
		public void GroupByWithRollup()
		{
			aggTest.Query.WithRollup = true;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(AggregateTest.ColumnNames.DepartmentID, WhereParameter.Dir.DESC);
			aggTest.Query.Load();
		}


	}
}
