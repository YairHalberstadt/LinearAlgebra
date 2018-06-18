using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Matrixes
{
    public interface IRowVector<TDataType, TOperationDefiner> : IVector<TDataType, TOperationDefiner>
        where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
	    IMatrix<TDataType, TOperationDefiner> AsMatrix();
    }
}