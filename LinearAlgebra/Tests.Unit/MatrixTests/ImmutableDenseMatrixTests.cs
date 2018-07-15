using System.Collections.Generic;
using Matrixes;
using NUnit.Framework;
using Tests.Utils;
using Types.Integers.Int32;
using Matrixes.GenericImplementations;

namespace Tests.Unit.MatrixTests
{
    [TestFixture]
    class ImmutableDenseMatrixTests
    {
        private IEnumerable<IMatrix<int, Int32RingOperationDefiner>> _matrices;

        [OneTimeSetUp]
        public void SetUp()
        {
            _matrices = GetRandom.GetRandomMatrices(
                (x, y) => new ImmutableDenseMatrix<int, Int32RingOperationDefiner>(x, y.rows, y.columns), x => x, 1000,
                new[] {0, 1, 2, 3, 4, 5, 6, 7, 100, 1000});
        }

        [Test]
        public void RunIMatrixTest()
        {
            IMatrixTests.RunIMatrixTestSuite(_matrices);
        }
    }
}