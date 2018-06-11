using System;
using System.Collections.Generic;
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
	}
}