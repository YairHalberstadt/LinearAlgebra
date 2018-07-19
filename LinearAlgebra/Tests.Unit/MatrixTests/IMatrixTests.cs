using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matrixes;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Tests.Utils;
using Vectors;

namespace Tests.Unit.MatrixTests
{
    public static class IMatrixTests
    {
	    public static void TestItemCount<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    Assert.True(matrices.All(x => x.ItemCount == x.RowCount * x.ColumnCount));
	    }

        public static void TestRows<S, T>(IEnumerable<IMatrix<S, T>> matrices)
            where T : IRingOperationDefiner<S>, new()
        {
            var opDef = new T();
            Assert.True(matrices.All(x => x.Rows.All(y => y.Length.Equals(x.ColumnCount))));
            Assert.True(matrices.All(x => x.Rows.SelectMany(y => y).AreEqual(x.Items, opDef)));
        }

        public static void TestColumns<S, T>(IEnumerable<IMatrix<S, T>> matrices)
            where T : IRingOperationDefiner<S>, new()
        {
            var opDef = new T();
            Assert.True(matrices.All(x => x.Columns.All(y => y.Length.Equals(x.RowCount))));
            Assert.True(matrices.All(x => x.Columns.ToArray().ZipMany().AreEqual(x.Items, opDef)));
        }

        public static void RunIMatrixTestSuite<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    matrices = matrices.ToList();
            TestItemCount(matrices);
	    }

	    private static bool AreEqual<S, T>(this IEnumerable<S> matrix1, IEnumerable<S> matrix2, T opDef)
		    where T : IRingOperationDefiner<S>
	    {
		    return matrix1.Zip(matrix2, (x, y) => (x, y)).All(p => opDef.Equals(p.x, p.y));
	    }
	}
}
