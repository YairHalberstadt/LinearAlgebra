using System.Collections;
using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Vectors
{
    public abstract class Vector<TDataType, TOperationDefiner> : IVector<TDataType, TOperationDefiner>
        where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
        public abstract int Length { get; }

        public abstract TDataType this[int index] { get; }

        public virtual bool Equals(IVector<TDataType, TOperationDefiner> equand)
        {
            if (equand == null)
                return false;

            var opDef = new TOperationDefiner();

            var length = Length;

            for (int i = 0; i < length; i++)
            {
                if (!opDef.Equals(this[i], equand[i]))
                    return false;
            }

            return true;
        }

        public override bool Equals(object equand)
        {
            if (equand is IVector<TDataType, TOperationDefiner> vec)
                return Equals(vec);
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 19;
            var length = Length;

            for (int i = 0; i < length; i++)
            {
                hash = hash * 31 + this[i]?.GetHashCode() ?? 1;
            }
            return hash;
        }

	    public static Vector<TDataType, TOperationDefiner> operator +(Vector<TDataType, TOperationDefiner> first,
		    IVector<TDataType, TOperationDefiner> second) => first.Add(second);

	    public static Vector<TDataType, TOperationDefiner> operator -(Vector<TDataType, TOperationDefiner> first,
		    IVector<TDataType, TOperationDefiner> second) => first.Add(second.Negative());

	    public static Vector<TDataType, TOperationDefiner> operator -(Vector<TDataType, TOperationDefiner> operand) => operand.Negative();

        public abstract Vector<TDataType, TOperationDefiner> LeftScale(TDataType scalar);

	    public abstract Vector<TDataType, TOperationDefiner> RightScale(TDataType scalar);

        public abstract Vector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend);

        public abstract Vector<TDataType, TOperationDefiner> Negative();

        public abstract Vector<TDataType, TOperationDefiner> AdditiveIdentity();

        public abstract TDataType InnerProduct(IVector<TDataType, TOperationDefiner> operand);

        public abstract IEnumerator<TDataType> GetEnumerator();

        public abstract Vector<TDataType, TOperationDefiner> Slice(int from = 0, int to = -0);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.LeftScale(TDataType scalar) => LeftScale(scalar);

	    IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.RightScale(TDataType scalar) => RightScale(scalar);

        IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Add(IVector<TDataType, TOperationDefiner> addend) => Add(addend);

        IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Negative() => Negative();

        IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.AdditiveIdentity() => AdditiveIdentity();

        IVector<TDataType, TOperationDefiner> IVector<TDataType, TOperationDefiner>.Slice(int from, int to) => Slice(@from, to);
    }
}