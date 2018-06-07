using OperationDefiners.CoreOperationDefinerInterfaces;
using System;
using System.Collections.Generic;
using Vectors;

namespace Tests.Unit
{
    public static class IVectorSetUpMethods<TDataType, TOperationDefiner>
	    where TOperationDefiner : IRingOperationDefiner<TDataType>, new()
    {
	    public static IEnumerable<IVector<TDataType, TOperationDefiner>> CreateRandomVectors(
		    Func<IEnumerable<TDataType>, IVector<TDataType, TOperationDefiner>> createVectorFromIenumerable, Func<int,TDataType> createRandomTDataTypeFromInt, int numberOfVectors, int maxSize)
	    {
		    var rand = new Random(42);
		    for (int i = 0; i < numberOfVectors; i++)
		    {
			    var size = rand.Next(0,maxSize);
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
