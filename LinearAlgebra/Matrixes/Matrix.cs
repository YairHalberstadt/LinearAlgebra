using System.Collections.Generic;
using System.Collections.Immutable;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Matrixes
{
	public abstract class Matrix<TDataType, TOperationDefiner> : IMatrix<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		public abstract int RowCount { get; }

		public abstract int ColumnCount { get; }

		public virtual int ItemCount => checked(RowCount * ColumnCount);

		public abstract IEnumerable<RowVector<TDataType, TOperationDefiner>> Rows { get; }

		public abstract IEnumerable<ColumnVector<TDataType, TOperationDefiner>> Columns { get; }

		public abstract ImmutableArray<TDataType> Items { get;}

		public abstract RowVector<TDataType, TOperationDefiner> this[int index] { get;}

        public abstract TDataType this[int rowIndex, int columnIndex] { get;}

		public abstract Matrix<TDataType, TOperationDefiner> LeftScale(TDataType scalar);

		public abstract Matrix<TDataType, TOperationDefiner> RightScale(TDataType scalar);

		public abstract Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend);

		public abstract Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

		public abstract Matrix<TDataType, TOperationDefiner> Negative();

		public abstract Matrix<TDataType, TOperationDefiner> AdditiveIdentity();

		public bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand) => ColumnCount == multiplicand.RowCount;

		public bool SameSize(IMatrix<TDataType, TOperationDefiner> multiplicand) => ColumnCount == multiplicand.ColumnCount && RowCount == multiplicand.RowCount;

		IEnumerable<IRowVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Rows => Rows;

        IEnumerable<IColumnVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Columns => Columns;

		IRowVector<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.this[int index] => this[index];

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.LeftScale(TDataType scalar) => LeftScale(scalar);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.RightScale(TDataType scalar) => RightScale(scalar);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Add(
			IMatrix<TDataType, TOperationDefiner> addend) => Add(addend);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Multiply(
			IMatrix<TDataType, TOperationDefiner> multiplicand) => Multiply(multiplicand);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Negative() => Negative();

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.AdditiveIdentity() => AdditiveIdentity();
	}
}