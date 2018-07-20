using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;
using Vectors.GenericImplementations;

namespace Matrixes.GenericImplementations
{
	public class ImmutableDenseRowVector<TDataType, TOperationDefiner> : RowVector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly ImmutableDenseVector<TDataType, TOperationDefiner> _vector;

		/// <summary>
		/// Requires copying the array to guarantee Immutability.
		/// If you know the array is Immutable, consider calling Utils.UnsafeMakeImmutable(values) first to improve performance;
		/// </summary>
		/// <param name="values"></param>
		public ImmutableDenseRowVector(IEnumerable<TDataType> values) =>
			_vector = new ImmutableDenseVector<TDataType, TOperationDefiner>(values);

		public ImmutableDenseRowVector(TDataType[] values) =>
			_vector = new ImmutableDenseVector<TDataType, TOperationDefiner>(values);

		/// <summary>
		/// Fastest way to initialise a new Vector, as the array does not need to be copied.
		/// </summary>
		/// <param name="values"></param>
		public ImmutableDenseRowVector(ImmutableArray<TDataType> values) =>
			_vector = new ImmutableDenseVector<TDataType, TOperationDefiner>(values);

		public sealed override int Length => _vector.Length;

		public sealed override TDataType this[int index] => _vector[index];

		public ImmutableArray<TDataType> Items => _vector.Items;

		public sealed override Vector<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			return _vector.LeftScale(scalar);
		}

		public sealed override Vector<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			return _vector.RightScale(scalar);
		}

		public sealed override Vector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend)
		{
			return _vector.Add(addend);
		}

		public sealed override Vector<TDataType, TOperationDefiner> Negative()
		{
			return _vector.Negative();
		}

		public sealed override Vector<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			return _vector.AdditiveIdentity();
		}

		public sealed override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func)
		{
			return _vector.Apply(func);
		}

		public sealed override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType, TDataType> func, IVector<TDataType, TOperationDefiner> vector)
		{
			return _vector.Apply(func, vector);
		}

		public sealed override TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand)
		{
			return _vector.InnerProduct(operand);
		}

		public sealed override Vector<TDataType, TOperationDefiner> Slice(int @from = 0, int to = 0)
		{
			return _vector.Slice(@from, to);
		}

		public sealed override IEnumerator<TDataType> GetEnumerator()
		{
			return _vector.GetEnumerator();
		}

		public sealed override Matrix<TDataType, TOperationDefiner> AsMatrix()
		{
			return new ImmutableDenseMatrix<TDataType, TOperationDefiner>(Items, Length, 1);
		}
	}
}