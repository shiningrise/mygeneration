using System;
using NUnit.Framework;
using MyGeneration.dOOdads;

namespace MyGeneration.dOOdads.Tests.SQL
{
	[TestFixture]
	public class SqlXmlFixture
	{
		AggregateTest aggTest = new AggregateTest();
		AggregateTest aggClone = new AggregateTest();
		
		[TestFixtureSetUp]
		public void Init()
		{
			aggTest.ConnectionStringConfig = "SQLConnection";
		}

		[SetUp]
		public void Init2()
		{
			aggTest.FlushData();
		}

		[Test]
		public void SerializeDeserialize()
		{
			aggTest.LoadAll();
			aggTest.LastName = "Griffinski";
			aggTest.GetChanges();
			string str = aggTest.Serialize();
			
			aggClone.Deserialize(str);
			Assert.AreEqual(1, aggClone.RowCount);
			Assert.AreEqual("Modified", aggClone.RowState().ToString());
			Assert.AreEqual("Griffinski", aggClone.s_LastName);
		}

		[Test]
		public void ToXmlFromXml()
		{
			aggTest.LoadAll();
			aggTest.LastName = "Griffinski";
			aggTest.GetChanges();
			string str = aggTest.ToXml();
			
			aggClone.FromXml(str);
			Assert.AreEqual(1, aggClone.RowCount);
			Assert.AreEqual("Added", aggClone.RowState().ToString());
			Assert.AreEqual("Griffinski", aggClone.s_LastName);
		}

	}
}
