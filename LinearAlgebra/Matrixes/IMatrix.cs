using System;
using System.Collections;
using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Matrixes
{
	/// <The Ienumerable iterates through all of the first row
	/// then all of the second row
	/// etc />
	public interface IMatrix<TDataType, TOperationDefiner> : IEnumerable<TDataType> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
        int RowCount { get; }

        int ColumnCount { get; }

		long ItemCount { get; }

	    bool Equals(IMatrix<TDataType, TOperationDefiner> equand);

		IEnumerable<IRowVector<TDataType, TOperationDefiner>> Rows { get; }

        IEnumerable<IColumnVector<TDataType, TOperationDefiner>> Columns { get; }

	    IRowVector<TDataType, TOperationDefiner> this[int index] { get;}

        TDataType this[int rowIndex, int columnIndex] { get;}

        IMatrix<TDataType, TOperationDefiner> LeftScale(TDataType scalar);

	    IMatrix<TDataType, TOperationDefiner> RightScale(TDataType scalar);

		IMatrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend);

	    IMatrix<TDataType, TOperationDefiner> Subtract(IMatrix<TDataType, TOperationDefiner> subtrand);

		IMatrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

        IMatrix<TDataType, TOperationDefiner> Negative();

        IMatrix<TDataType, TOperationDefiner> AdditiveIdentity();

	    IMatrix<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func);

	    IMatrix<TDataType, TOperationDefiner> ApplyOnColumns(Func<IColumnVector<TDataType, TOperationDefiner>, IColumnVector<TDataType, TOperationDefiner>> func);

	    IMatrix<TDataType, TOperationDefiner> ApplyOnRows(Func<IRowVector<TDataType, TOperationDefiner>, IRowVector<TDataType, TOperationDefiner>> func);

	    IMatrix<TDataType, TOperationDefiner> Slice(int rowsFrom = 0, int rowsTo = -0, int columnsFrom = 0, int columnsTo = -0);

	    IMatrix<TDataType, TOperationDefiner> Transpose();

		bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

	    bool SameSize(IMatrix<TDataType, TOperationDefiner> addend);
	}
}

