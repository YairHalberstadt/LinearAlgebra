using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Matrixes
{
	public abstract class Matrix<TDataType, TOperationDefiner> : IMatrix<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		public abstract int RowCount { get; }

		public abstract int ColumnCount { get; }

		public abstract IEnumerable<RowVector<TDataType, TOperationDefiner>> Rows { get; }

		public abstract IEnumerable<ColumnVector<TDataType, TOperationDefiner>> Columns { get; }

        public abstract IRowVector<TDataType, TOperationDefiner> this[int index] { get; set; }

        public abstract TDataType this[int rowIndex, int columnIndex] { get; set; }

		public abstract Matrix<TDataType, TOperationDefiner> Scale(TDataType scalar);

		public abstract Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend);

		public abstract Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

		public abstract Matrix<TDataType, TOperationDefiner> Negative();

		public abstract Matrix<TDataType, TOperationDefiner> AdditiveIdentity();

		public abstract bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

		IEnumerable<IRowVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Rows => Rows;

        IEnumerable<IColumnVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Columns => Columns;
		
        IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Scale(TDataType scalar) => Scale(scalar);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Add(
			IMatrix<TDataType, TOperationDefiner> addend) => Add(addend);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Multiply(
			IMatrix<TDataType, TOperationDefiner> multiplicand) => Multiply(multiplicand);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Negative() => Negative();

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.AdditiveIdentity() => AdditiveIdentity();
	}
}