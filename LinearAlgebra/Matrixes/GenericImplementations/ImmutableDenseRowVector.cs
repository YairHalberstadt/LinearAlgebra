using System;
using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;
using Vectors.GenericImplementations;

namespace Matrixes.GenericImplementations
{
	public class ImmutableDenseRowVector<TDataType, TOperationDefiner> : RowVector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		private readonly ImmutableDenseVector<TDataType, TOperationDefiner> _vector;

		public ImmutableDenseRowVector(IEnumerable<TDataType> values) =>
			_vector = new ImmutableDenseVector<TDataType, TOperationDefiner>(values);

		/// <summary>
		/// When we know TDataType[] is immutable, used to reduce the overhead of creating a new vector.
		/// </summary>
		/// <param name="values"></param>
		internal ImmutableDenseRowVector(TDataType[] values) =>
			_vector = new ImmutableDenseVector<TDataType, TOperationDefiner>(values);

		public override int Length => _vector.Length;

		public override TDataType this[int index] => _vector[index];

		public override Vector<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
		{
			return _vector.LeftScale(scalar);
		}

		public override Vector<TDataType, TOperationDefiner> RightScale(TDataType scalar)
		{
			return _vector.RightScale(scalar);
		}

		public override Vector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend)
		{
			return _vector.Add(addend);
		}

		public override Vector<TDataType, TOperationDefiner> Negative()
		{
			return _vector.Negative();
		}

		public override Vector<TDataType, TOperationDefiner> AdditiveIdentity()
		{
			return _vector.AdditiveIdentity();
		}

		public override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func)
		{
			return _vector.Apply(func);
		}

		public override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType, TDataType> func, IVector<TDataType, TOperationDefiner> vector)
		{
			return _vector.Apply(func, vector);
		}

		public override TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand)
		{
			return _vector.InnerProduct(operand);
		}

		public override Vector<TDataType, TOperationDefiner> Slice(int @from = 0, int to = 0)
		{
			return _vector.Slice(@from, to);
		}

		public override IEnumerator<TDataType> GetEnumerator()
		{
			return _vector.GetEnumerator();
		}

		public override Matrix<TDataType, TOperationDefiner> AsMatrix()
		{
			throw new NotImplementedException();
		}
	}
}