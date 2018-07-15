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

        public static IEnumerable<T> ZipMany<T>(this IEnumerable<T>[] sequences)
        {
            if (!sequences.Any())
                yield break;
            while (true)
            {
                for (int i = 0; i < sequences.Length; i++)
                {
                    var sequence = sequences[i];
                    if (!sequence.Any())
                        yield break;
                    yield return sequence.First();
                    sequences[i] = sequence.Skip(1);
                }
            }
        }
    }
}