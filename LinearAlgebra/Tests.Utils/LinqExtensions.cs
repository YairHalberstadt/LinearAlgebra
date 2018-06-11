using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Utils
{
    public static class LinqExtensions
    {
	    public static IEnumerable<TResult> SelectFromAdjacentPairs<TSource, TResult>
	    (this IEnumerable<TSource> source,
		    Func<TSource, TSource, TResult> projection)
	    {
		    using (var iterator = source.GetEnumerator())
		    {
			    if (!iterator.MoveNext())
			    {
				    yield break;
			    }
			    TSource previous = iterator.Current;
			    while (iterator.MoveNext())
			    {
				    yield return projection(previous, iterator.Current);
				    previous = iterator.Current;
			    }
		    }
	    }
    }
}
