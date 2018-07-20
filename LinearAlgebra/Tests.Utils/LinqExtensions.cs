using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<T> ZipMany<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            if (!sequences.Any())
                yield break;
	        var iteratorArrays = sequences.Select(x => x.GetEnumerator()).ToArray();
            while (true)
            {
                for (int i = 0; i < iteratorArrays.Length; i++)
                {
                    var iterator = iteratorArrays[i];
                    if (!iterator.MoveNext())
                        yield break;
                    yield return iterator.Current;
	                iteratorArrays[i] = iterator;
                }
            }
        }
    }
}