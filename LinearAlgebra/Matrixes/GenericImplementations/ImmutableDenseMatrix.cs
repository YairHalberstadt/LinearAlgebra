using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Utils;

namespace Matrixes.GenericImplementations
{
    public class ImmutableDenseMatrix<TDataType, TOperationDefiner> : IMatrix<TDataType, TOperationDefiner>
		where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly TOperationDefiner _opDef;

		#region constructors

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

		#endregion

		#region Equality, Hashcode, Enumerators

		public sealed override bool Equals(object equand)
		{
			if (equand is IMatrix<TDataType, TOperationDefiner> vec)
				return Equals(vec);
			return false;
		}

		public bool Equals(IMatrix<TDataType, TOperationDefiner> equand)
		{
			if (equand == null)
				return false;

			if (!SameSize(equand))
				return false;

			if (equand is ImmutableDenseMatrix<TDataType, TOperationDefiner> mat)
				return Equals(mat);

			for (int i = 0; i < RowCount; i++)
			for (int j = 0; j < ColumnCount; j++)
				if (!_opDef.Equals(this[i, j], equand[i, j]))
					return false;
			return true;
		}

		public bool Equals(ImmutableDenseMatrix<TDataType, TOperationDefiner> equand)
		{
			if (equand == null)
				return false;
			if (!SameSize(equand))
				return false;
			var items = equand.Items;
			for (int i = 0; i < ItemCount; i++)
				if (!_opDef.Equals(Items[i], items[i]))
					return false;
			return true;
		}

		public sealed override int GetHashCode()
		{
			int hash = 31 * (19 * 31 + RowCount) + ColumnCount;
			int step = RowCount / 16 + 1;
			for (int i = 0, j = 0; i < 16; i ++, j = hash % ItemCount)
			{
				hash = hash * 31 + this[j]?.GetHashCode() ?? 0;
			}
			return hash;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<TDataType> GetEnumerator()
		{
			return ((IEnumerable<TDataType>)Items).GetEnumerator();
		}

		#endregion

		#region static operators

		public static bool operator ==(ImmutableDenseMatrix<TDataType, TOperationDefiner> first,
			IMatrix<TDataType, TOperationDefiner> second) => first?.Equals(second) ?? second is null;

		public static bool operator !=(ImmutableDenseMatrix<TDataType, TOperationDefiner> first,
			IMatrix<TDataType, TOperationDefiner> second) => !(first == second);

		public static ImmutableDenseMatrix<TDataType, TOperationDefiner> operator +(ImmutableDenseMatrix<TDataType, TOperationDefiner> first,
			IMatrix<TDataType, TOperationDefiner> second) => first.Add(second);

		public static ImmutableDenseMatrix<TDataType, TOperationDefiner> operator -(ImmutableDenseMatrix<TDataType, TOperationDefiner> first,
			IMatrix<TDataType, TOperationDefiner> second) => first.Subtract(second);

		public static ImmutableDenseMatrix<TDataType, TOperationDefiner> operator -(ImmutableDenseMatrix<TDataType, TOperationDefiner> operand) =>
			operand.Negative();

		public static ImmutableDenseMatrix<TDataType, TOperationDefiner> operator *(ImmutableDenseMatrix<TDataType, TOperationDefiner> first,
			IMatrix<TDataType, TOperationDefiner> second) => first.Multiply(second);

		#endregion

		#region Properties

		public int RowCount { get; }

		public int ColumnCount { get; }

		/* Since an array cant contain more than int.MaxValue items, we can safely
		 use an int here, which most architectures are optimised for */
		public int ItemCount => Items.Length;

		long IMatrix<TDataType, TOperationDefiner>.ItemCount => ItemCount;

		public IEnumerable<ImmutableDenseRowVector<TDataType, TOperationDefiner>> Rows
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


		IEnumerable<IRowVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Rows => Rows;

		public IEnumerable<ImmutableDenseColumnVector<TDataType, TOperationDefiner>> Columns
		{
			get
			{
				var pointer = 0;
				for (int i = 0; i < ColumnCount; pointer = ++i)
				{
					var array = new TDataType[RowCount];

					for (int j = 0; j < RowCount; j++, pointer += ColumnCount)
					{
						array[j] = Items[pointer];
					}

					yield return new ImmutableDenseColumnVector<TDataType, TOperationDefiner>(array);
				}
			}
		}

		IEnumerable<IColumnVector<TDataType, TOperationDefiner>> IMatrix<TDataType, TOperationDefiner>.Columns => Columns;

		public ImmutableArray<TDataType> Items { get; }

		public ImmutableDenseRowVector<TDataType, TOperationDefiner> this[int index]
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

		IRowVector<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.this[int index] => this[index];

		public TDataType this[int rowIndex, int columnIndex]
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

		#endregion

		#region Core Operations

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(scalar, Items[i]);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public bool CanMultiply(IMatrix<TDataType, TOperationDefiner> multiplicand) => ColumnCount == multiplicand.RowCount;

		public bool SameSize(IMatrix<TDataType, TOperationDefiner> operand) => ColumnCount == operand.ColumnCount && RowCount == operand.RowCount;

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			var itemsCount = ItemCount;
			var array = new TDataType[itemsCount];
			for (int i = 0; i < itemsCount; i++)
			{
				array[i] = _opDef.Multiply(Items[i], scalar);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Add(IMatrix<TDataType, TOperationDefiner> addend)
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

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Subtract(IMatrix<TDataType, TOperationDefiner> addend)
		{
			if (addend is ImmutableDenseMatrix<TDataType, TOperationDefiner> mat)
				return Subtract(mat);
			if (!SameSize(addend))
				throw new ArgumentOutOfRangeException(nameof(addend),
					(addend), "Addend must be same size as the matrix");

			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			for (int row = 0, pointer = 0; row < RowCount; row++)
			{
				for (int column = 0; column < ColumnCount; column++, pointer++)
					array[pointer] = _opDef.Subtract(Items[pointer], addend[row, column]);
			}

			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Subtract(
			ImmutableDenseMatrix<TDataType, TOperationDefiner> addend)
		{
			if (!SameSize(addend))
				throw new ArgumentOutOfRangeException(nameof(addend),
					(addend), "Addend must be same size as the matrix");

			var items = addend.Items;
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Add(Items[i], _opDef.Negative(items[i]));
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
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

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Multiply(ImmutableDenseMatrix<TDataType, TOperationDefiner> multiplicand)
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

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Negative()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];

			for (int i = 0; i < itemCount; i++)
				array[i] = _opDef.Negative(Items[i]);
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			var itemCount = ItemCount;
			var array = new TDataType[itemCount];
			var zero = _opDef.Zero;

			for (int i = 0; i < itemCount; i++)
				array[i] = zero;
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(array.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func)
		{
			var results = new TDataType[ItemCount];
			for (int i = 0; i < ItemCount; i++)
			{
				results[i] = func(Items[i]);
			}
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(results.UnsafeMakeImmutable(), RowCount, ColumnCount);
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> ApplyOnColumns(Func<IColumnVector<TDataType, TOperationDefiner>, IColumnVector<TDataType, TOperationDefiner>> func)
		{
			throw new NotImplementedException();
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> ApplyOnRows(Func<IRowVector<TDataType, TOperationDefiner>, IRowVector<TDataType, TOperationDefiner>> func)
		{
			throw new NotImplementedException();
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Slice(int rowsFrom = 0, int rowsTo = 0, int columnsFrom = 0, int columnsTo = 0)
		{
			throw new NotImplementedException();
		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> Transpose()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IMatrix Implementation

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.LeftScale(TDataType scalar)
		{
			return LeftScale(scalar);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.RightScale(TDataType scalar)
		{
			return RightScale(scalar);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Add(IMatrix<TDataType, TOperationDefiner> addend)
		{
			return Add(addend);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Subtract(IMatrix<TDataType, TOperationDefiner> subtrand)
		{
			return Subtract(subtrand);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Multiply(IMatrix<TDataType, TOperationDefiner> multiplicand)
		{
			return Multiply(multiplicand);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Negative()
		{
			return Negative();
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.AdditiveIdentity()
		{
			return AdditiveIdentity();
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Apply(Func<TDataType, TDataType> func)
		{
			return Apply(func);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.ApplyOnColumns(Func<IColumnVector<TDataType, TOperationDefiner>, IColumnVector<TDataType, TOperationDefiner>> func)
		{
			return ApplyOnColumns(func);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.ApplyOnRows(Func<IRowVector<TDataType, TOperationDefiner>, IRowVector<TDataType, TOperationDefiner>> func)
		{
			return ApplyOnRows(func);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Slice(int rowsFrom, int rowsTo, int columnsFrom, int columnsTo)
		{
			return Slice(rowsFrom, rowsTo, columnsFrom, columnsTo);
		}

		IMatrix<TDataType, TOperationDefiner> IMatrix<TDataType, TOperationDefiner>.Transpose()
		{
			return Transpose();
		}

		#endregion
	}
}