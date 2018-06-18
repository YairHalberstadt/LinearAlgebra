using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Matrixes
{
    public interface IColumnVector<TDataType, TOperationDefiner> : IVector<TDataType, TOperationDefiner>
        where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
	    IMatrix<TDataType, TOperationDefiner> AsMatrix();
    }
}