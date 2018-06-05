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

        IVector<TDataType, TOperationDefiner> Scale(TDataType scalar);

        IVector<TDataType, TOperationDefiner> Add(IVector<TDataType, TOperationDefiner> addend);

        IVector<TDataType, TOperationDefiner> Negative();

        IVector<TDataType, TOperationDefiner> AdditiveIdentity();

        TDataType InnerProduct();
    }
}
