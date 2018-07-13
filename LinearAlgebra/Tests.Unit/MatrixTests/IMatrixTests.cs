using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matrixes;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Tests.Unit.MatrixTests
{
    public static class IMatrixTests
    {
	    public static void TestItemCount<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    matrices.All(x => x.ItemCount == x.RowCount * x.ColumnCount);
	    }

	    public static void RunIVectorTestSuite<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    matrices = matrices.ToList();
	    }

	    private static bool AreEqual<S, T>(this IEnumerable<S> matrix1, IEnumerable<S> matrix2, T opDef)
		    where T : IRingOperationDefiner<S>
	    {
		    return matrix1.Zip(matrix2, (x, y) => (x, y)).All(p => opDef.Equals(p.x, p.y));
	    }
	}
}
