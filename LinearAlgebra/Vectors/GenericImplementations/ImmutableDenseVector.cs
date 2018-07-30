using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Utils;

namespace Vectors.GenericImplementations
{
	public class ImmutableDenseVector<TDataType, TOperationDefiner> : IVector<TDataType, TOperationDefiner>
		where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly TOperationDefiner _opDef = new TOperationDefiner();

		#region constructors

		public ImmutableDenseVector(IEnumerable<TDataType> items) : this(items.ToImmutableArray())
		{
		}

		/// <summary>
		/// Requires copying the array to guarantee Immutability.
		/// If you know the array is Immutable, consider calling Utils.UnsafeMakeImmutable(items) first to improve performance;
		/// </summary>
		/// <param name="items"></param>
		public ImmutableDenseVector(TDataType[] items) : this(ImmutableArray.Create(items))
		{
		}

		/// <summary>
		/// Fastest way to initialise a new Vector, as the array does not need to be copied.
		/// </summary>
		/// <param name="items"></param>
		public ImmutableDenseVector(ImmutableArray<TDataType> items)
		{
			Items = items;
		}

		#endregion

		#region Equality, Hashcode, Enumerators

		public bool Equals(ImmutableDenseVector<TDataType, TOperationDefiner> equand)
		{
			if (equand == null)
				return false;

			var opDef = new TOperationDefiner();

			var length = Length;

			if (equand.Length != length)
				return false;

			var equandItems = equand.Items;

			for (int i = 0; i < length; i++)
			{
				if (!opDef.Equals(this[i], equandItems[i]))
					return false;
			}

			return true;
		}

		public bool Equals(IVector<TDataType, TOperationDefiner> equand)
		{
			if (equand == null)
				return false;

			var opDef = new TOperationDefiner();

			var length = Length;

			if (equand.Length != length)
				return false;

			for (int i = 0; i < length; i++)
			{
				if (!opDef.Equals(this[i], equand[i]))
					return false;
			}

			return true;
		}

		public sealed override bool Equals(object equand)
		{
			if (equand is IVector<TDataType, TOperationDefiner> vec)
				return Equals(vec);
			return false;
		}

		public sealed override int GetHashCode()
		{
			int hash = 19 * 31 + Length;
			int step = Length / 16 + 1;
			for (int i = 0; i < Length; i+= step)
			{
				hash = hash * 31 + this[i]?.GetHashCode() ?? 0;
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

		public static bool operator ==(ImmutableDenseVector<TDataType, TOperationDefiner> first,
			IVector<TDataType, TOperationDefiner> second) => first?.Equals(second) ?? second is null;

		public static bool operator !=(ImmutableDenseVector<TDataType, TOperationDefiner> first, IVector<TDataType, TOperationDefiner> second) => !(first == second);

		public static ImmutableDenseVector<TDataType, TOperationDefiner> operator +(ImmutableDenseVector<TDataType, TOperationDefiner> first,
			IVector<TDataType, TOperationDefiner> second) => first.Add(second);

		public static ImmutableDenseVector<TDataType, TOperationDefiner> operator -(ImmutableDenseVector<TDataType, TOperationDefiner> first,
			IVector<TDataType, TOperationDefiner> second) => first.Subtract(second);

		public static ImmutableDenseVector<TDataType, TOperationDefiner> operator -(ImmutableDenseVector<TDataType, TOperationDefiner> operand) => operand.Negative();

		#endregion

		#region Properties

		public TDataType this[int index] => Items[index];

		public ImmutableArray<TDataType> Items { get; }

		public int Length => Items.Length;

		#endregion

		#region Core Operations

		public ImmutableDenseVector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend)
		{
			if (addend is ImmutableDenseVector<TDataType, TOperationDefiner> vec)
				return Add(vec);

			if (addend.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Add(Items[i], addend[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Add(
			ImmutableDenseVector<TDataType, TOperationDefiner> addend)
		{
			if (addend.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var addendItems = addend.Items;

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Add(Items[i], addendItems[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Subtract(IVector<TDataType, TOperationDefiner> subtrand)
		{
			if (subtrand is ImmutableDenseVector<TDataType, TOperationDefiner> vec)
				return Subtract(vec);

			if (subtrand.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Subtract(Items[i], subtrand[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Subtract(
			ImmutableDenseVector<TDataType, TOperationDefiner> subtrand)
		{
			if (subtrand.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var addendItems = subtrand.Items;

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Subtract(Items[i], addendItems[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Negative()
		{
			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Negative(Items[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			var result = new TDataType[Length];
			var zero = _opDef.Zero;

			for (int i = 0; i < Length; i++)
				result[i] = zero;

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func)
		{
			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = func(Items[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType, TDataType> func,
			IVector<TDataType, TOperationDefiner> vector)
		{
			if (vector is ImmutableDenseVector<TDataType, TOperationDefiner> vec)
				return Apply(func, vec);

			if (vector.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = func(Items[i], vector[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType, TDataType> func,
			ImmutableDenseVector<TDataType, TOperationDefiner> vector)
		{
			if (vector.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var thatItems = vector.Items;

			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = func(Items[i], thatItems[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Multiply(scalar, Items[i]);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			var result = new TDataType[Length];

			for (int i = 0; i < Length; i++)
				result[i] = _opDef.Multiply(Items[i], scalar);

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		public TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand)
		{
			if (operand is ImmutableDenseVector<TDataType, TOperationDefiner> vec)
				return InnerProduct(vec);

			if (operand.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var result = _opDef.Zero;

			for (int i = 0; i < Length; i++)
				result = _opDef.Add(result, _opDef.Multiply(Items[i], operand[i]));

			return result;
		}

		public TDataType InnerProduct(ImmutableDenseVector<TDataType, TOperationDefiner> operand)
		{
			if (operand.Length != Length)
				throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

			var operandItems = operand.Items;

			var result = _opDef.Zero;

			for (int i = 0; i < Length; i++)
				result = _opDef.Add(result, _opDef.Multiply(Items[i], operandItems[i]));

			return result;
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Slice(int @from, int to)
		{
			return Slice(@from, to);
		}

		public ImmutableDenseVector<TDataType, TOperationDefiner> Slice(int from = 0, int to = -0)
		{
			if (Length == 0)
				return this;

			from %= Length;
			if (from < 0)
				from += Length;

			to %= Length;
			if (to < 0)
				to += Length;

			TDataType[] result;
			if (from < to)
			{
				result = new TDataType[to - from];
				for (int i = from, j = 0; i < to; i++, j++)
				{
					result[j] = Items[i];
				}
			}
			else
			{
				result = new TDataType[Length - from + to];
				int j = 0;
				for (int i = from; i < Length; i++, j++)
				{
					result[j] = Items[i];
				}

				for (int i = 0; i < to; i++, j++)
				{
					result[j] = Items[i];
				}
			}

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result.UnsafeMakeImmutable());
		}

		#endregion

		#region IVector Implementation

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.LeftScale(TDataType scalar)
		{
			return LeftScale(scalar);
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.RightScale(TDataType scalar)
		{
			return RightScale(scalar);
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Add(IVector<TDataType, TOperationDefiner> addend)
		{
			return Add(addend);
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Negative()
		{
			return Negative();
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Subtract(IVector<TDataType, TOperationDefiner> subtrand)
		{
			return Subtract(subtrand);
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.AdditiveIdentity()
		{
			return AdditiveIdentity();
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Apply(Func<TDataType, TDataType> func)
		{
			return Apply(func);
		}

		IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Apply(Func<TDataType, TDataType, TDataType> func, IVector<TDataType, TOperationDefiner> vector)
		{
			return Apply(func, vector);
		}

		#endregion
	}
}