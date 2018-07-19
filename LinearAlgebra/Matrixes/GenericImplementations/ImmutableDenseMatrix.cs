using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Utils;

namespace Matrixes.GenericImplementations
{
    public class ImmutableDenseMatrix<TDataType, TOperationDefiner> : Matrix<TDataType, TOperationDefiner>
		where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly TOperationDefiner _opDef;

		public ImmutableDenseMatrix(IEnumerable<TDataType> items, int rowCount, int columnCount) : this(ImmutableArray.CreateRange(items),
			rowCount, columnCount)
		{
		}

		/// <summary>
		/// Requires copying the array to guarantee Immutability.
		/// If you know the array is Immutable, consider calling Utils.UnsafeMakeImmutable(items) first to improve performance;
		/// </summary>
		/// <param name="items"></param>
		/// <param name="rowCount"></param>
		/// <param name="columnCount"></param>
		public ImmutableDenseMatrix(TDataType[] items, int rowCount, int columnCount) : this(ImmutableArray.Create(items),
			rowCount, columnCount)
		{
		}

		/// <summary>
		/// Fastest way to initialise a new Matrix, as the array does not need to be copied.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="rowCount"></param>
		/// <param name="columnCount"></param>
		public ImmutableDenseMatrix(ImmutableArray<TDataType> items, int rowCount, int columnCount)
		{
			if (rowCount < 0)
				throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, "The rowCount cannot be negative");
			if (columnCount < 0)
				throw new ArgumentOutOfRangeException(nameof(columnCount), columnCount, "The columnCount cannot be negative");

			Items = items;
			RowCount = rowCount;
			ColumnCount = columnCount;

			if (checked(RowCount * ColumnCount) != Items.Length)
				throw new ArgumentOutOfRangeException(nameof(items), items.Length,
					"The number of items must be equal to the rowCount multiplied by the columnCount");

			_opDef = new TOperationDefiner();
		}

		public sealed override int RowCount { get; }

		public sealed override int ColumnCount { get; }

		public sealed override int ItemCount => Items.Length;

		public sealed override IEnumerable<RowVector<TDataType, TOperationDefiner>> Rows
		{
			get
			{
				var pointer = 0;
				for (int i = 0; i < RowCount; i++)
				{
					var array = new TDataType[ColumnCount];

					for (int j = 0; j < ColumnCount; j++, pointer++)
					{
						array[j] = Items[pointer];
					}

					yield return new ImmutableDenseRowVector<TDataType, TOperationDefiner>(array);
				}
			}
		}

		public sealed override IEnumerable<ColumnVector<TDataType, TOperationDefiner>> Columns
		{
			get
			{
				var pointer = 0;
				for (int i = 0; i < ColumnCount; pointer = ++i)
				{
					var array = new TDataType[ColumnCount];

					for (int j = 0; j < RowCount; j++, pointer += ColumnCount)
					{
						array[j] = Items[pointer];
					}

					yield return new ImmutableDenseColumnVector<TDataType, TOperationDefiner>(array);
				}
			}
		}

		public ImmutableArray<TDataType> Items { get; }

		public sealed override RowVector<TDataType, TOperationDefiner> this[int index]
		{
			get
			{
				if (index >= RowCount)
					throw new ArgumentOutOfRangeException(nameof(index), index,
						"The index must be less than the number of rows in the Matrix");
				if (index < 0)
					throw new ArgumentOutOfRangeException(nameof(index), index,
						"The index cannot be negative");

				var array = new TDataType[ColumnCount];
				var pointer = index * ColumnCount;
				for (int i = 0; i < ColumnCount; i++, pointer++)
				{
					array[i] = Items[pointer];
				}

				return new ImmutableDenseRowVector<TDataType, TOperationDefiner>(array);
			}
		}

		public override TDataType this[int rowIndex, int columnIndex]
		{
			get
			{
				if (rowIndex >= RowCount)
					throw new ArgumentOutOfRangeException(nameof(rowIndex), rowIndex,
						"The rowIndex must be less than the number of rows in the Matrix");

				if (rowIndex < 0)
					throw new ArgumentOutOfRangeException(nameof(rowIndex), rowIndex,
						"The rowIndex cannot be negative");

				if (columnIndex >= ColumnCount)
					throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex,
						"The columnIndex must be less than the number of columns in the Matrix");

				if (columnIndex < 0)
					throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex,
						"The columnIndex cannot be negative");

				return Items[rowIndex * ColumnCount + columnIndex];
			}
		}

		public override Matrix<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(scalar, Items[i]);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public sealed override Matrix<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(Items[i], scalar);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public sealed override Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend)
		{
			if (addend is ImmutableDenseMatrix<TDataType, TOperationDefiner> mat)
				return Add(mat);
			if (!SameSize(addend))
				throw new ArgumentOutOfRangeException(nameof(addend),
					(addend), "Addend must be same size as the matrix");

			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			for (int row = 0, pointer = 0; row < RowCount; row++)
			{
				for (int column = 0; column < ColumnCount; column++, pointer++)
					array[pointer] = _opDef.Add(Items[pointer], addend[row, column]);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Add(
			ImmutableDenseMatrix<TDataType, TOperationDefiner> addend)
		{
			if (!SameSize(addend))
				throw new ArgumentOutOfRangeException(nameof(addend),
					(addend), "Addend must be same size as the matrix");

			var items = addend.Items;
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Add(Items[i], items[i]);
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public sealed override Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
		{
			if (!CanMultiply(multiplicand))
				throw new ArgumentOutOfRangeException(nameof(multiplicand),
					(multiplicand), "Multiplicand must have the same number of rows as the matrix has columns");

			var array = new TDataType[RowCount * multiplicand.ColumnCount];
			var zero = _opDef.Zero;

			var columnCount = ColumnCount;
			var rowCount = RowCount;
			var thatColumnCount = multiplicand.ColumnCount;

			var resultPointer = 0;
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < thatColumnCount; j++, resultPointer++)
				{
					var thisPointer = columnCount * i;
					var sum = zero;
					for (int k = 0; k < columnCount; k++, thisPointer++)
						sum = _opDef.Add(sum, _opDef.Multiply(Items[thisPointer], multiplicand[k,j]));
					array[resultPointer] = sum;
				}
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), rowCount, thatColumnCount);
		}

		public Matrix<TDataType, TOperationDefiner> Multiply(ImmutableDenseMatrix<TDataType, TOperationDefiner> multiplicand)
		{
			if (!CanMultiply(multiplicand))
				throw new ArgumentOutOfRangeException(nameof(multiplicand),
					(multiplicand), "Multiplicand must have the same number of rows as the matrix has columns");

			var thatItems = multiplicand.Items;

			var array = new TDataType[RowCount * multiplicand.ColumnCount];
			var zero = _opDef.Zero;

			var columnCount = ColumnCount;
			var rowCount = RowCount;
			var thatColumnCount = multiplicand.ColumnCount;

			var resultPointer = 0;
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < thatColumnCount; j++, resultPointer++)
				{
					var thisPointer = columnCount * i;
					var thatPointer = j;
					var sum = zero;
					for (int k = 0; k < columnCount; k++, thisPointer++, thatPointer += thatColumnCount)
						sum = _opDef.Add(sum, _opDef.Multiply(Items[thisPointer], thatItems[thatPointer]));
					array[resultPointer] = sum;
				}
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), rowCount, thatColumnCount);
		}

		public override Matrix<TDataType, TOperationDefiner> Negative()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];

			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Negative(Items[i]);
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public override Matrix<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			var zero = _opDef.Zero;

			for (int i = 0; i < itemCount; i++)
				array[i] = zero;
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public sealed override IEnumerator<TDataType> GetEnumerator()
		{
			return ((IEnumerable<TDataType>) Items).GetEnumerator();
		}
	}
}