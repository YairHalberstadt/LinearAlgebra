using System;
using System.Collections.Generic;
using Matrixes;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Vectors;

namespace Tests.Utils
{
	public static class GetRandom
	{
		public static IEnumerable<int> GetRandomInts(int numberOfInts = 10000, int seed = 42,
			int maxAbsoluteSize = int.MaxValue)
		{
			var rand = new Random(42);
			for (int i = 0; i < numberOfInts; i++)
				yield return rand.Next(-maxAbsoluteSize, maxAbsoluteSize);
		}

		public static IEnumerable<IVector<TDataType, TOperationDefiner>> GetRandomVectors<TDataType, TOperationDefiner>(
			Func<IEnumerable<TDataType>, IVector<TDataType, TOperationDefiner>> createVectorFromIenumerable,
			Func<int, TDataType> createRandomTDataTypeFromInt,
			int numberOfVectors,
			int[] vectorLengths)
			where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
		{
			var rand = new Random(42);
			for (int i = 0; i < numberOfVectors; i++)
			{
				var size = vectorLengths[rand.Next(0, vectorLengths.Length)];
				TDataType[] randomArray = new TDataType[size];
				for (int j = 0; j < size; j++)
				{
					randomArray[j] = createRandomTDataTypeFromInt(rand.Next());
				}

				yield return createVectorFromIenumerable(randomArray);
			}
		}

		public static IEnumerable<IMatrix<TDataType, TOperationDefiner>> GetRandomMatrices<TDataType, TOperationDefiner>(
			Func<IEnumerable<TDataType>, (int rows, int columns), IMatrix<TDataType, TOperationDefiner>> createMatrixFromIenumerable,
			Func<int, TDataType> createRandomTDataTypeFromInt,
			int numberOfMatrices,
			(int rows, int columns)[] matrixSizes)
			where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
		{
			var rand = new Random(42);
			for (int i = 0; i < numberOfMatrices; i++)
			{
				var size = matrixSizes[rand.Next(0, matrixSizes.Length)];
				TDataType[] randomArray = new TDataType[size.rows * size.columns];
				for (int j = 0; j < size.rows * size.columns; j++)
				{
					randomArray[j] = createRandomTDataTypeFromInt(rand.Next());
				}

				yield return createMatrixFromIenumerable(randomArray, size);
			}
		}
	}
}