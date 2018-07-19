using System.Collections.Generic;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors.GenericImplementations;
using Tests.Utils;
using Types.Integers.Int32;
using Vectors;

namespace Tests.Unit.VectorTests
{
	[TestFixture]
    class ImmutableDenseVectorTests
    {
        private IEnumerable<IVector<int, Int32RingOperationDefiner>> _vectors;

	    [OneTimeSetUp]
	    public void SetUp()
	    {
			_vectors = GetRandom.GetRandomVectors(x => new ImmutableDenseVector<int, Int32RingOperationDefiner>(x), x => x, 1000, new[]{0,1,2,3,4,5,6,7,100,1000});
	    }

		[Test]
	    public void RunIVectorTest()
		{
			IVectorTests.RunIVectorTestSuite(_vectors);
        }
    }
}
