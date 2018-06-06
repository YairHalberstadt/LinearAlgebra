using System;
using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Matrixes
{
    public interface IMatrix<TDataType, TOperationDefiner> : IEnumerable<IRowVector<TDataType, TOperationDefiner>>,
        IEnumerable<IColumnVector<TDataType, TOperationDefiner>>,
        IEnumerable<TDataType> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
        int RowCount { get; }

        int ColumnCount { get; }

        IEnumerable<IRowVector<TDataType, TOperationDefiner>> Rows { get; }

        IEnumerable<IColumnVector<TDataType, TOperationDefiner>> Columns { get; }

        IRowVector<TDataType, TOperationDefiner> this[int index] { get; set; }

        TDataType this[int rowIndex, int columnIndex] { get; set; }

        IMatrix<TDataType, TOperationDefiner> Scale(TDataType scalar);

        IMatrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend);

        IMatrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

        IMatrix<TDataType, TOperationDefiner> Negative();

        IMatrix<TDataType, TOperationDefiner> AdditiveIdentity();

        bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand);
    }
}
