using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Matrixes
{
	public abstract class RowVector<TDataType, TOperationDefiner> : Vector<TDataType, TOperationDefiner>,
		IRowVector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		public static implicit operator Matrix<TDataType, TOperationDefiner>(RowVector<TDataType, TOperationDefiner> cv) => cv.AsMatrix();

		public abstract Matrix<TDataType, TOperationDefiner> AsMatrix();

		IMatrix<TDataType, TOperationDefiner> IRowVector<TDataType, TOperationDefiner>.AsMatrix() => AsMatrix();
	}
}