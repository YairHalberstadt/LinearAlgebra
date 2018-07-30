using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;
using Vectors.GenericImplementations;

namespace Matrixes.GenericImplementations
{
	public class ImmutableDenseColumnVector<TDataType, TOperationDefiner> : ImmutableDenseVector<TDataType, TOperationDefiner>, IColumnVector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly ImmutableDenseVector<TDataType, TOperationDefiner> _vector;

		public ImmutableDenseColumnVector(IEnumerable<TDataType> values) : base(values)
		{ }

		/// <summary>
		/// Requires copying the array to guarantee Immutability.
		/// If you know the array is Immutable, consider calling Utils.UnsafeMakeImmutable(values) first to improve performance;
		/// </summary>
		/// <param name="values"></param>
		public ImmutableDenseColumnVector(TDataType[] values) : base(values)
		{ }

		/// <summary>
		/// Fastest way to initialise a new Vector from values, as the array does not need to be copied.
		/// </summary>
		/// <param name="values"></param>
		public ImmutableDenseColumnVector(ImmutableArray<TDataType> values) : base(values)
		{ }

		/// <summary>
		/// Initialise a Column vector from an already existing vector. Fast
		/// </summary>
		/// <param name="vector"></param>
		public ImmutableDenseColumnVector(ImmutableDenseVector<TDataType, TOperationDefiner> vector) : base(vector.Items)
		{

		}

		public ImmutableDenseMatrix<TDataType, TOperationDefiner> AsMatrix() => new ImmutableDenseMatrix<TDataType, TOperationDefiner>(Items, Length, 1);

		IMatrix<TDataType, TOperationDefiner> IColumnVector<TDataType, TOperationDefiner>.AsMatrix() => AsMatrix();

		public static implicit operator ImmutableDenseMatrix<TDataType, TOperationDefiner>(ImmutableDenseColumnVector<TDataType, TOperationDefiner> vec) =>
			vec.AsMatrix();
	}
}