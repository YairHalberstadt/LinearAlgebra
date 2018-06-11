using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vectors;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Tests.Utils;

namespace Tests.Unit.VectorTests
{
	class IVectorTests
	{
		public static void TestAddition<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().Add(y.First())));

			var opdef = new T();
			var zero = opdef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Aggregate((x, y) => x.Add(y))
						.SequenceEqual(
							group.Aggregate(
								Enumerable.Repeat(zero, group.Key),
								(x, y) => x.Zip(y, (f, s) => opdef.Add(f, s))
							)
						)
				);
			}
		}
	}
}