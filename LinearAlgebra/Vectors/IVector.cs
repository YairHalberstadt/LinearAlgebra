using System;
using System.Collections.Generic;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Vectors
{
    public interface IVector<TDataType, TOperationDefiner> : IEnumerable<TDataType>
        where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
        int Length { get; }

        TDataType this[int index] { get; }

        bool Equals(IVector<TDataType, TOperationDefiner> equand);

        IVector<TDataType, TOperationDefiner> Scale(TDataType scalar);

        IVector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend);

        IVector<TDataType, TOperationDefiner> Negative();

        IVector<TDataType, TOperationDefiner> AdditiveIdentity();

        TDataType InnerProduct();

        /// <summary>
        /// Gets the vector starting at from, up to and not including to, looping the vector if neccessary.
        /// Zero based index used.
        /// Negative indices refer to elements before the end of the vector.
        /// Eg:
        /// Slice(0, 0) will return the original vector
        /// Slice(4, 8) will return the 5th, 6th, 7th 8th items.
        /// Slice(4, -7) will return all the elements after the 4th, up to, but not including the element 7 before the end.
        /// Slice(-7, 4) will return the last 7 elements of the vector, followed by the first 4.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IVector<TDataType, TOperationDefiner> Slice(int from = 0, int to = 0);
    }
}
