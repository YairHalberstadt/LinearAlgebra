using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Vectors.GenericImplementations
{
    public class ImmutableDenseVector<TDataType, TOperatorDefiner> : IVector<TDataType, TOperatorDefiner> where TOperatorDefiner : IRingOperationDefiner<TDataType>, new()
    {
        private readonly TDataType[] _items;

        public ImmutableDenseVector(IEnumerable<TDataType> items)
        {
            _items = items.ToArray();
        }

        /// <summary>
        /// When we know TDataType[] is immutable, used to reduce the overhead of creating a new vector.
        /// </summary>
        /// <param name="items"></param>
        private ImmutableDenseVector(TDataType[] items)
        {
            _items = items;
        }

        public TDataType this[int index] => _items[index];

        public int Length => _items.Length;

        public IVector<TDataType, TOperatorDefiner> Add(IVector<TDataType, TOperatorDefiner> addend)
        {
            if (addend.Length != Length)
                throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

            var result = new TDataType[Length];
            var opDef = new TOperatorDefiner();

            for (int i = 0; i < Length; i++)
                result[i] = opDef.Add(this[i], addend[i]);

            return new ImmutableDenseVector<TDataType, TOperatorDefiner>(result);
        }

        public IVector<TDataType, TOperatorDefiner> AdditiveIdentity()
        {
            var result = new TDataType[Length];
            var zero = new TOperatorDefiner().Zero;

            for (int i = 0; i < Length; i++)
                result[i] = zero;

            return new ImmutableDenseVector<TDataType, TOperatorDefiner>(result);
        }

        public IEnumerator<TDataType> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public TDataType InnerProduct()
        {
            throw new NotImplementedException();
        }

        public IVector<TDataType, TOperatorDefiner> Slice(int @from = 0, int to = 0)
        {
            throw new NotImplementedException();
        }

        public IVector<TDataType, TOperatorDefiner> Negative()
        {
            throw new NotImplementedException();
        }

        public IVector<TDataType, TOperatorDefiner> Scale(TDataType scalar)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
