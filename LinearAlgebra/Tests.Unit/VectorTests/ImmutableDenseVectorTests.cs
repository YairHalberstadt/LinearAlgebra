using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerImplementations.RuntimeOperationsDefiners;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors.GenericImplementations;
using Tests.Utils;
using Vectors;

namespace Tests.Unit.VectorTests
{
	[TestFixture]
    class ImmutableDenseVectorTests
    {
        private IEnumerable<IVector<int, RuntimeRingOperationDefiner<int>>> _vectors;
	    private IRingOperationDefiner<int> opDef;

	    [OneTimeSetUp]
	    public void SetUp()
	    {
		    RuntimeRingOperationDefiner<int>.TrySetOperations(
			    new RingOperationsDefinition<int>((x, y) => x == y, (x, y) => x + y, (x, y) => x * y, 0, x => -x, 1));
		    opDef = new RuntimeRingOperationDefiner<int>();
			_vectors = GetRandom.GetRandomVectors(x => new ImmutableDenseVector<int, RuntimeRingOperationDefiner<int>>(x), x => x, 1000, new int[]{0,1,2,3,4,5,6,7,100,1000});
	    }

		[Test]
	    public void AddTest()
		{
			IVectorTests.TestAddition(_vectors);
        }
    }
}
