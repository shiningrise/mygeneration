using System;
using NUnit.Framework;
using MyGeneration.dOOdads;

namespace MyGeneration.dOOdads.Tests.SQL
{
	[TestFixture]
	public class SqlEOFFixture
	{
		AggregateTest aggTest = new AggregateTest();
		int nCounter = 0;
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "SQLConnection";
		}

		[SetUp]
		public void Init2()
		{
			nCounter = 0;
			aggTest.FlushData();
		}

		[Test]
		public void EOFWithNoLoad()
		{
			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(0, nCounter);
		}

		[Test]
		public void EOFWithFalseLoad()
		{
			aggTest.Where.IsActive.Operator = WhereParameter.Operand.Equal;
			aggTest.Where.IsActive.Value = true;
			WhereParameter wp = aggTest.Where.TearOff.IsActive;
			wp.Operator = WhereParameter.Operand.Equal;
			wp.Value = false;
			
			aggTest.Query.Load();

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(0, nCounter);
		}

		[Test]
		public void EOFWithOneRow()
		{
			aggTest.Query.Top = 1;
			aggTest.Query.Load();

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(aggTest.Query.Top, nCounter);
		}

		[Test]
		public void EOFWithTwoRows()
		{
			aggTest.Query.Top = 2;
			aggTest.Query.Load();

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(aggTest.Query.Top, nCounter);
		}

		[Test]
		public void EOFWithLoadAll()
		{
			aggTest.LoadAll();

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(aggTest.RowCount, nCounter);
		}

		[Test]
		public void EOFWithFilter()
		{
			aggTest.LoadAll();
			aggTest.Filter = "DepartmentID = 3";

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(8, nCounter);

			aggTest.Filter = "";
			nCounter = 0;

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(30, nCounter);
		}

		[Test]
		public void EOFWithEmptyFilter()
		{
			aggTest.LoadAll();
			aggTest.Filter = "DepartmentID = 99";

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(0, nCounter);

			aggTest.Filter = "";
			nCounter = 0;

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(30, nCounter);
		}

		[Test]
		public void EOFWithSort()
		{
			aggTest.LoadAll();
			aggTest.Sort = "ID DESC";

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(30, nCounter);

			aggTest.Sort = "";
			nCounter = 0;

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(30, nCounter);
		}

		[Test]
		public void EOFWithGroupBy()
		{
			aggTest.Query.CountAll = true;
			aggTest.Query.AddResultColumn(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.AddGroupBy(AggregateTest.ColumnNames.DepartmentID);
			aggTest.Query.Load();

			while(!aggTest.EOF && nCounter < 35)
			{
				nCounter++;
				aggTest.MoveNext();
			}
			Assert.AreEqual(7, nCounter);
		}

	}
}
