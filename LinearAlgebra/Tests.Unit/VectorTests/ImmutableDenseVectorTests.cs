using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tests.Unit.VectorTests
{
	[TestFixture]
    class ImmutableDenseVectorTests
    {
	    [OneTimeSetUp]
	    public void SetUp()
	    {
		    var intOpDef = new RuntimeRingOperationDefiner<int>();
		    Assert.Throws<NullReferenceException>(() => intOpDef.Add(1, 2));
		    Assert.False(RuntimeRingOperationDefiner<int>.IsOperationsSet);
		    Assert.True(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x + y, (x, y) => x * y, 0, x => -x, 1)));
		    Assert.True(RuntimeRingOperationDefiner<int>.IsOperationsSet);
		    Assert.False(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x * y, (x, y) => x + y, 0, x => -x, 1)));

		    var doubleOpDef = new RuntimeRingOperationDefiner<double>();
		    Assert.Throws<NullReferenceException>(() => doubleOpDef.Add(1, 2));
		    Assert.False(RuntimeRingOperationDefiner<double>.IsOperationsSet);
		    Assert.True(RuntimeRingOperationDefiner<double>.TrySetOperations(
			    new RingOperationsDefinition<double>((x, y) => x == y, (x, y) => x + y, (x, y) => x * y, 0, x => -x, 1)));
		    Assert.True(RuntimeRingOperationDefiner<double>.IsOperationsSet);
		    Assert.False(RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x * y, (x, y) => x + y, 0, x => -x, 1)));
	    }
    }
}
