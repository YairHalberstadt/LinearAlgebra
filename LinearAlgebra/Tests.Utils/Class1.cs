using System;
using System.Collections.Generic;

namespace Tests.Utils
{
    public static class GetRandom
    {
	    public static IEnumerable<int> GetRandomInts(int numberOfInts =10000, int seed = 42, int maxAbsoluteSize= int.MaxValue)
	    {
		    var rand = new Random(42);
		    for (int i = 0; i < numberOfInts; i++)
			    yield return rand.Next(-maxAbsoluteSize, maxAbsoluteSize);
	    }
    }
}
