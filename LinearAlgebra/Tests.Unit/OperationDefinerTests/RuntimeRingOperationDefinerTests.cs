using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerImplementations.RuntimeOperationsDefiners;
using Tests.Utils;

namespace Tests.Unit.OperationDefinerTests
{
	[TestFixture]
    class RuntimeRingOperationDefinerTests
    {
	    [OneTimeSetUp]
	    public void SetUp()
	    {
		    var intOpDef = new RuntimeRingOperationDefiner<int>();
		    Assert.Throws<NullReferenceException>(() => intOpDef.Add(1,2));
		    Assert.False(RuntimeRingOperationDefiner<int>.IsOperationsSet);
		    Assert.True(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x + y, (x, y) => x * y, 0, x => -x, (x, y) => x - y, 1)));
		    Assert.True(RuntimeRingOperationDefiner<int>.IsOperationsSet);
		    Assert.False(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x * y, (x, y) => x + y, 0, x => -x, (x, y) => x - y, 1)));

            var doubleOpDef = new RuntimeRingOperationDefiner<double>();
            Assert.Throws<NullReferenceException>(() => doubleOpDef.Add(1, 2));
		    Assert.False(RuntimeRingOperationDefiner<double>.IsOperationsSet);
		    Assert.True(RuntimeRingOperationDefiner<double>.TrySetOperations(
			    new RingOperationsDefinition<double>((x, y) => x == y, (x, y) => x + y, (x, y) => x * y, 0, x => -x, (x, y) => x - y, 1)));
		    Assert.True(RuntimeRingOperationDefiner<double>.IsOperationsSet);
		    Assert.False(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x * y, (x, y) => x + y, 0, x => -x, (x, y) => x - y, 1)));
        }

		[Test]
	    public void TestRuntimeRingOperationDefiner()
	    {
		    var intOpDef = new RuntimeRingOperationDefiner<int>();
		    var ints = GetRandom.GetRandomInts(10000,42,100000).ToList();

            Assert.AreEqual(ints.Aggregate(0, (x,y) => intOpDef.Add(x,y)), ints.Sum());

		    Assert.AreEqual(ints.Aggregate(1, (x, y) => intOpDef.Multiply(x, y)), ints.Aggregate(1, (x, y) => x * y));

		    Assert.AreEqual(intOpDef.Zero, 0);

		    Assert.AreEqual(intOpDef.One, 1);

		    Assert.AreEqual(ints.Select(intOpDef.Negative), ints.Select(x=> -x));
        }
    }
}
