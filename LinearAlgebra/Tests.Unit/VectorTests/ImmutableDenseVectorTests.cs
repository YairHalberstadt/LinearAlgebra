using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerImplementations.RuntimeOperationsDefiners;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors.GenericImplementations;
using Tests.Utils;
using Types;
using Vectors;

namespace Tests.Unit.VectorTests
{
	[TestFixture]
    class ImmutableDenseVectorTests
    {
        private IEnumerable<IVector<int, IntRingOperationDefiner>> _vectors;
	    private IRingOperationDefiner<int> opDef;

	    [OneTimeSetUp]
	    public void SetUp()
	    {
		    opDef = new IntRingOperationDefiner();
			_vectors = GetRandom.GetRandomVectors(x => new ImmutableDenseVector<int, IntRingOperationDefiner>(x), x => x, 1000, new[]{0,1,2,3,4,5,6,7,100,1000});
	    }

		[Test]
	    public void RunIVectorTest()
		{
			IVectorTests.TestLength(_vectors);
            IVectorTests.TestAddition(_vectors);
        }
    }
}
