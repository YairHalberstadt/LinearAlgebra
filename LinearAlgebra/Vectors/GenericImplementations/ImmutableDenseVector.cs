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

	    private readonly TOperationDefiner _opDef = new TOperationDefiner();

        public ImmutableDenseVector(IEnumerable<TDataType> items) : this(items.ToArray())
        {
        }

        /// <summary>
        /// When we know TDataType[] is immutable, used to reduce the overhead of creating a new vector.
        /// </summary>
        /// <param name="items"></param>
        private ImmutableDenseVector(TDataType[] items)
        {
            _items = items;
	        Length = _items.Length;
        }

        public override TDataType this[int index] => _items[index];

	    public override int Length { get; }

        public override Vector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend)
        {
            if (addend.Length != Length)
                throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

            var result = new TDataType[Length];
            
            for (int i = 0; i < Length; i++)
                result[i] = _opDef.Add(_items[i], addend[i]);

            return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> AdditiveIdentity()
        {
            var result = new TDataType[Length];
            var zero = _opDef.Zero;

            for (int i = 0; i < Length; i++)
                result[i] = zero;

            return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

	    public override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType> func)
	    {
		    var result = new TDataType[Length];

		    for (int i = 0; i < Length; i++)
			    result[i] = func(_items[i]);

		    return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

	    public override Vector<TDataType, TOperationDefiner> Apply(Func<TDataType, TDataType, TDataType> func, IVector<TDataType, TOperationDefiner> vector)
	    {
		    if (vector.Length != Length)
			    throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

            var result = new TDataType[Length];

		    for (int i = 0; i < Length; i++)
			    result[i] = func(_items[i], vector[i]);

		    return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

	    public override IEnumerator<TDataType> GetEnumerator()
        {
	        for (int i = 0; i < Length; i++)
		        yield return _items[i];
        }

        public override TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand)
        {
	        if (operand.Length != Length)
		        throw new ArgumentOutOfRangeException("The Length of the two vectors must match");

	        var result = _opDef.Zero;

	        for (int i = 0; i < Length; i++)
		        result = _opDef.Add(result, _opDef.Multiply(_items[i], operand[i]));

            return result;
        }

        public override Vector<TDataType, TOperationDefiner> Slice(int from = 0, int to = -0)
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
			        result[j] = _items[i];
		        }
	        }
	        else
	        {
		        result = new TDataType[Length - from + to];
		        int j = 0;
		        for (int i = from; i < Length; i++, j++)
		        {
			        result[j] = _items[i];
		        }
		        for (int i = 0; i < to; i++, j++)
		        {
			        result[j] = _items[i];
		        }
            }

			return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> Negative()
        {
	        var result = new TDataType[Length];

	        for (int i = 0; i < Length; i++)
		        result[i] = _opDef.Negative(_items[i]);

	        return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

        public override Vector<TDataType, TOperationDefiner> LeftScale(TDataType scalar)
        {
	        var result = new TDataType[Length];

	        for (int i = 0; i < Length; i++)
		        result[i] = _opDef.Multiply(scalar,_items[i]);

	        return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
        }

	    public override Vector<TDataType, TOperationDefiner> RightScale(TDataType scalar)
	    {
		    var result = new TDataType[Length];

		    for (int i = 0; i < Length; i++)
			    result[i] = _opDef.Multiply(_items[i], scalar);

		    return new ImmutableDenseVector<TDataType, TOperationDefiner>(result);
	    }
    }
}
