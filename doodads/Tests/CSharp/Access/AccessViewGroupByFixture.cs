using System;
using NUnit.Framework;
using MyGeneration.dOOdads;
using System.Data.OleDb;

namespace MyGeneration.dOOdads.Tests.Access
{
	[TestFixture]
	public class AccessViewGroupByFixture
	{
		FullNameView aggTest = new FullNameView();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "AccessConnection";
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
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(1, aggTest.RowCount);
		}

		[Test]
		public void TwoGroupBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhere()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithWhereAndOrderBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(FullNameView.ColumnNames.DepartmentID, WhereParameter.Dir.ASC);
			aggTest.Query.AddOrderBy(FullNameView.ColumnNames.IsActive, WhereParameter.Dir.ASC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		public void GroupByWithOrderByCountAll()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
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
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
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
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
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
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(5, aggTest.RowCount);
		}

		[Test]
		[ExpectedException(typeof(OleDbException))]
		public void GroupByWithRollup()
		{
			aggTest.Query.WithRollup = true;
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.IsActive);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.DepartmentID);
			aggTest.Query.AddOrderBy(aggTest.Query, WhereParameter.Dir.DESC);
			aggTest.Query.Load();
		}

		[Test]
		public void GroupByFullName()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.CountAllAlias = "Count";
			aggTest.Query.AddResultColumn(FullNameView.ColumnNames.FullName);
			aggTest.Query.AddGroupBy(FullNameView.ColumnNames.FullName);
			Assert.IsTrue(aggTest.Query.Load());
			Assert.AreEqual(16, aggTest.RowCount);
		}

	}
}
