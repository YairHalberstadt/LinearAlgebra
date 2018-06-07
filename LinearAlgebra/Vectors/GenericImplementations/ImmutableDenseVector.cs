using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Vectors.GenericImplementations
{
    public class ImmutableDenseVector<TDataType, TOperationDefiner> : Vector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
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

        public override TDataType this[int index] => _items[index];

        public override int Length => _items.Length;

        public override Vector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend)
        {
	        var length = Length;
            if (addend.Length != length)
                throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

            var result = new TDataType[length];
            var opDef = new TOperationDefiner();
            
            for (int i = 0; i < length; i++)
                result[i] = opDef.Add(_items[i], addend[i]);

            return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> AdditiveIdentity()
        {
            var result = new TDataType[Length];
            var zero = new TOperationDefiner().Zero;
	        var length = Length;

            for (int i = 0; i < length; i++)
                result[i] = zero;

            return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override IEnumerator<TDataType> GetEnumerator()
        {
	        var length = Length;

	        for (int i = 0; i < length; i++)
		        yield return _items[i];
        }

        public override TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand)
        {
	        var length = Length;
	        if (operand.Length != length)
		        throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

	        var opDef = new TOperationDefiner();
	        var result = opDef.Zero;

	        for (int i = 0; i < length; i++)
		        result = opDef.Add(result, opDef.Multiply(_items[i], operand[i]));

            return result;
        }

        public override Vector<TDataType, TOperationDefiner> Slice(int from = 0, int to = -0)
        {
            var length = Length;
            if (from < 0)
                from = length + from;
            if (to <= 0)
                to = length + to;
	        if (from >= length  || to > length)
		        throw new ArgumentOutOfRangeException(
			        "the absolute values of the parameters must be less than the Length of the vector");

            TDataType[] result;
	        if (from < to)
	        {
		        result = new TDataType[to - from];
		        for (int i = from; i < to; i++)
		        {
			        result[i] = _items[i];
		        }
	        }
	        else
	        {
		        result = new TDataType[length - from + to];
		        for (int i = from; i < length; i++)
		        {
			        result[i] = _items[i];
		        }
		        for (int i = 0; i < to; i++)
		        {
			        result[i] = _items[i];
		        }
            }

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> Negative()
        {
	        var result = new TDataType[Length];
	        var length = Length;
	        var opDef = new TOperationDefiner();

	        for (int i = 0; i < length; i++)
		        result[i] = opDef.Negative(_items[i]);

	        return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
        {
	        var result = new TDataType[Length];
	        var length = Length;
	        var opDef = new TOperationDefiner();

	        for (int i = 0; i < length; i++)
		        result[i] = opDef.Multiply(scalar,_items[i]);

	        return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

	    public override Vector<TDataType, TOperationDefiner> RightScale(TDataType scalar)
	    {
		    var result = new TDataType[Length];
		    var length = Length;
		    var opDef = new TOperationDefiner();

		    for (int i = 0; i < length; i++)
			    result[i] = opDef.Multiply(_items[i], scalar);

		    return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
	    }
    }
}
