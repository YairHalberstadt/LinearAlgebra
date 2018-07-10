using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Matrixes.GenericImplementations
{
	class ImmutableDenseMatrix<TDataType, TOperationDefiner> : Matrix<TDataType, TOperationDefiner>
		where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly TDataType[] _items;
		private readonly TOperationDefiner _opDef;

		public ImmutableDenseMatrix(IEnumerable<TDataType> items, int rowCount, int columnCount) : this(items.ToArray(),
			rowCount, columnCount)
		{
		}

		/// <summary>
		/// When we know TDataType[] is immutable, used to reduce the overhead of creating a new matrix.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="rowCount"></param>
		/// <param name="columnCount"></param>
		internal ImmutableDenseMatrix(TDataType[] items, int rowCount, int columnCount)
		{
			if (rowCount < 0)
				throw new ArgumentOutOfRangeException(nameof(rowCount), rowCount, "The rowCount cannot be negative");
			if (columnCount < 0)
				throw new ArgumentOutOfRangeException(nameof(columnCount), columnCount, "The columnCount cannot be negative");

			_items = items;
			RowCount = rowCount;
			ColumnCount = columnCount;

			if (ItemCount != items.Length)
				throw new ArgumentOutOfRangeException(nameof(items), items.Length,
					"The number of items must be equal to the rowCount multiplied by the columnCount");

			_opDef = new TOperationDefiner();
		}

		public sealed override int RowCount { get; }

		public sealed override int ColumnCount { get; }

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
						array[j] = _items[pointer];
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
						array[j] = _items[pointer];
					}

					yield return new ImmutableDenseColumnVector<TDataType, TOperationDefiner>(array);
				}
			}
		}

		public override ImmutableArray<TDataType> Items => ImmutableArray.Create(_items);

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
					array[i] = _items[pointer];
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

				return _items[rowIndex * ColumnCount + columnIndex];
			}
		}

		public override Matrix<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(scalar, _items[i]);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, RowCount, ColumnCount);
		}

		public sealed override Matrix<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(_items[i], scalar);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, RowCount, ColumnCount);
		}

		public sealed override Matrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend)
		{
			if (!SameSize(addend))
				throw new ArgumentOutOfRangeException(nameof(addend),
					(addend), "Addend must be same size as the matrix");

			var items = addend.Items; 
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Add(_items[i], items[i]);
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, RowCount, ColumnCount);
		}

		public override Matrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
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
						sum = _opDef.Add(sum, _opDef.Multiply(_items[thisPointer], thatItems[thatPointer]));
					array[resultPointer] = sum;
				}
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, rowCount, thatColumnCount);
		}

		public override Matrix<TDataType, TOperationDefiner> Negative()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];

			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Negative(_items[i]);
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, RowCount, ColumnCount);
		}

		public override Matrix<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			var zero = _opDef.Zero;

			for (int i = 0; i < itemCount; i++)
				array[i] = zero;
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array, RowCount, ColumnCount);
		}
	}
}