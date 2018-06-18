using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Matrixes
{
	public abstract class ColumnVector<TDataType, TOperationDefiner> : Vector<TDataType, TOperationDefiner>,
		IColumnVector<TDataType, TOperationDefiner> where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
	{
		public static implicit operator Matrix<TDataType, TOperationDefiner>(ColumnVector<TDataType, TOperationDefiner> cv) => cv.AsMatrix();

		public abstract Matrix<TDataType, TOperationDefiner> AsMatrix();

		IMatrix<TDataType, TOperationDefiner> IColumnVector<TDataType, TOperationDefiner>.AsMatrix() => AsMatrix();
	}
}