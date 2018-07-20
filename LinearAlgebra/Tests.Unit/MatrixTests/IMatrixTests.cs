﻿using System;
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
            Assert.True(matrices.All(x => x.Rows.SelectMany(y => y).AreEqual(x, opDef)));
        }

        public static void TestColumns<S, T>(IEnumerable<IMatrix<S, T>> matrices)
            where T : IRingOperationDefiner<S>, new()
        {
            var opDef = new T();
            Assert.True(matrices.All(x => x.Columns.All(y => y.Length.Equals(x.RowCount))));
            Assert.True(matrices.All(x => x.Columns.ToArray().ZipMany().AreEqual(x, opDef)));
        }

	    public static void TestItemIndexers<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    foreach (var matrix in matrices)
		    {
			    var rows = matrix.Rows.ToArray();
			    for (int i = 0; i < matrix.RowCount; i++)
			    {
				    var row = rows[i];
				    for (int j = 0; j < matrix.ColumnCount; j++)
					    Assert.True(opDef.Equals(matrix[i, j], row[j]));
			    }
		    }
	    }

	    public static void TestRowIndexers<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    foreach (var matrix in matrices)
		    {
			    var rows = matrix.Rows.ToArray();
			    for (int i = 0; i < matrix.RowCount; i++)
			    {
				    var row = rows[i];
				    var indexedRow = matrix[i];
				    Assert.True(row.AreEqual(indexedRow, opDef));
			    }
		    }
	    }

	    public static void TestLeftScale<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    S first = default;
		    foreach (var matrix in matrices)
		    {
			    if (matrix.Any())
			    {
				    first = matrix.First();
				    Assert.True(matrix.LeftScale(first)
					    .AreEqual(matrix.Select(x => opDef.Multiply(first, x)), opDef));
			    }
			    else
			    {
				    if (first != null)
					    Assert.False(matrix.LeftScale(first).Any());
			    }
		    }
	    }

		public static void TestRightScale<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    S first = default;
		    foreach (var matrix in matrices)
		    {
			    if (matrix.Any())
			    {
				    first = matrix.First();
				    Assert.True(matrix.RightScale(first)
					    .AreEqual(matrix.Select(x => opDef.Multiply(x, first)), opDef));
			    }
			    else
			    {
				    if (first != null)
					    Assert.False(matrix.RightScale(first).Any());
			    }
		    }
	    }

	    public static void TestAdd<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    var zero = opDef.Zero;
		    var groups = matrices.GroupBy(x => (x.RowCount, x.ColumnCount)).ToList();
		    foreach (var group in groups)
		    {
			    var result1 = group.Aggregate((x, y) => x.Add(y));
			    var result2 = group.Aggregate(
				    Enumerable.Repeat(zero, group.Key.RowCount * group.Key.ColumnCount),
				    (x, y) => x.Zip(y, (f, s) => opDef.Add(f, s))
			    );
			    Assert.True(result1.AreEqual(result2, opDef));
		    }
	    }

	    public static void TestMultiply<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    var opDef = new T();
		    var zero = opDef.Zero;
		    var rowGroups = matrices.GroupBy(x => x.RowCount).ToList();
			var columnGroups = matrices.GroupBy(x => x.ColumnCount).ToList();
		    foreach (var rowGroup in rowGroups)
		    {
			    var columnGroup = columnGroups.FirstOrDefault(x => x.Key == rowGroup.Key);
			    if (columnGroup == null)
				    continue;
			    var pairs = columnGroup.Zip(rowGroup, (x, y) => (x, y)).ToList();
			    foreach (var pair in pairs)
			    {
				    pair.x.Multiply(pair.y);
				    pair.x.Multiply(pair.y);
					//var columns = pair.y.Columns;
					//var expResult = pair.x.Rows.SelectMany(x => columns.Select(x.InnerProduct));
					//Assert.True(pair.x.Multiply(pair.y).AreEqual(expResult, opDef));
				}
		    }
	    }

		public static void RunIMatrixTestSuite<S, T>(IEnumerable<IMatrix<S, T>> matrices)
		    where T : IRingOperationDefiner<S>, new()
	    {
		    matrices = matrices.ToList();
            TestItemCount(matrices);
		    TestRows(matrices);
		    TestColumns(matrices);
		    TestRowIndexers(matrices);
			TestItemIndexers(matrices);
		    TestLeftScale(matrices);
		    TestRightScale(matrices);
		    TestAdd(matrices);
		    TestMultiply(matrices);
	    }

	    private static bool AreEqual<S, T>(this IEnumerable<S> matrix1, IEnumerable<S> matrix2, T opDef)
		    where T : IRingOperationDefiner<S>
	    {
		    return matrix1.Zip(matrix2, (x, y) => (x, y)).All(p => opDef.Equals(p.x, p.y));
	    }
	}
}
