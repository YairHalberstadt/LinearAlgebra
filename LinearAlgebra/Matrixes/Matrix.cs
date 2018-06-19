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

        public abstract RowVector<TDataType, TOperationDefiner> this[int index] { get;}

        public abstract TDataType this[int rowIndex, int columnIndex] { get;}

		public abstract Matrix<TDataType, TOperationDefiner> Scale(TDataType scalar);

		public abstract Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend);

		public abstract Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

		public abstract Matrix<TDataType, TOperationDefiner> Negative();

		public abstract Matrix<TDataType, TOperationDefiner> AdditiveIdentity();

		public abstract bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand);

		IEnumerable<IRowVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Rows => Rows;

        IEnumerable<IColumnVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Columns => Columns;

		IRowVector<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.this[int index] => this[index];

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Scale(TDataType scalar) => Scale(scalar);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Add(
			IMatrix<TDataType, TOperationDefiner> addend) => Add(addend);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Multiply(
			IMatrix<TDataType, TOperationDefiner> multiplicand) => Multiply(multiplicand);

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Negative() => Negative();

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.AdditiveIdentity() => AdditiveIdentity();
	}

	public class ImmutableDenseMatric<TDataType, TOperationDefiner> : Matrix<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly TDataType[] _values;
		public override int RowCount { get; }
		public override int ColumnCount { get; }
		public override bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
		{
			throw new System.NotImplementedException();
		}

		public override IEnumerable<RowVector<TDataType, TOperationDefiner>> Rows { get; }
		public override IEnumerable<ColumnVector<TDataType, TOperationDefiner>> Columns { get; }

		public override RowVector<TDataType, TOperationDefiner> this[int index] => throw new System.NotImplementedException();

		public override TDataType this[int rowIndex, int columnIndex] => throw new System.NotImplementedException();

		public override Matrix<TDataType, TOperationDefiner> Scale(TDataType scalar)
		{
			throw new System.NotImplementedException();
		}

		public override Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend)
		{
			throw new System.NotImplementedException();
		}

		public override Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
		{
			throw new System.NotImplementedException();
		}

		public override Matrix<TDataType, TOperationDefiner> Negative()
		{
			throw new System.NotImplementedException();
		}

		public override Matrix<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			throw new System.NotImplementedException();
		}
	}
}