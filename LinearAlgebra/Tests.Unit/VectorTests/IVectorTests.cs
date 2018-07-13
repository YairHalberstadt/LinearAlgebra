using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using NUnit.Framework;
using Vectors;
using OperationDefiners.CoreOperationDefinerInterfaces;
using Tests.Utils;
using Vectors.GenericImplementations;

namespace Tests.Unit.VectorTests
{
	static class IVectorTests
	{
		public static void TestLength<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			foreach (var vector in vectors)
				Assert.AreEqual(vector.Length, vector.Count());
		}

		public static void TestArrayAccessors<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
            foreach (var vector in vectors)
				Assert.True(vector.Select((x, i) => (x, vector[i])).All(p => opDef.Equals(p.Item1, p.Item2)));
		}

		public static void TestEquals<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
			var zero = opDef.Zero;
			var one = opDef.One;

			foreach (var vector in vectors)
			{
				Assert.True(vector.Equals(new ImmutableDenseVector<S, T>(vector)));
				var lastElem = vector.Length > 0 && opDef.Equals(vector[vector.Length - 1], zero) ? one : zero;
				Assert.False(
					vector.Equals(new ImmutableDenseVector<S, T>(vector.Take(vector.Length - 1).Concat(new[] {lastElem}))));
			}
		}

		public static void TestLeftScale<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
			var Ss = vectors.SelectMany(x => x);
			var scalars = Ss.Select(x => Ss.First(y => opDef.Equals(x, y))).Take(10);
			foreach (var vector in vectors)
			{
				foreach (var scalar in scalars)
				{
					var scaled = vector.LeftScale(scalar);
					Assert.True(vector.Select(x => opDef.Multiply(scalar, x)).AreEqual(scaled, opDef));

				}
			}
		}

		public static void TestRightScale<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
			var Ss = vectors.SelectMany(x => x);
			var scalars = Ss.Select(x => Ss.First(y => opDef.Equals(x, y))).Take(10);
			foreach (var vector in vectors)
			{
				foreach (var scalar in scalars)
				{
					var scaled = vector.RightScale(scalar);
					Assert.True(vector.Select(x => opDef.Multiply(x, scalar)).AreEqual(scaled, opDef));
				}
			}
		}

		public static void TestAddition<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().Add(y.First())));

			var opDef = new T();
			var zero = opDef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Aggregate((x, y) => x.Add(y))
						.AreEqual(
							group.Aggregate(
								Enumerable.Repeat(zero, group.Key),
								(x, y) => x.Zip(y, (f, s) => opDef.Add(f, s))
							), opDef
						)
				);
			}
		}

		public static void TestNegative<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
			foreach (var vector in vectors)
			{
				var negative = vector.Negative();
				Assert.True(vector.Select((x, i) => (opDef.Negative(x), negative[i])).All(p => opDef.Equals(p.Item1, p.Item2)));
			}
		}

		public static void TestSubtraction<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().Subtract(y.First())));

			var opDef = new T();
			var zero = opDef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Aggregate(group.First().AdditiveIdentity(), (x, y) => x.Subtract(y))
						.AreEqual(
							group.Aggregate(
								Enumerable.Repeat(zero, group.Key),
								(x, y) => x.Zip(y, (f, s) => opDef.Subtract(f, s))
							), opDef
						)
				);
			}
		}

		public static void AdditiveIdentity<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			var opDef = new T();
			var zero = opDef.Zero;
			foreach (var group in groups)
			{
				var additiveIdentity = group.First().AdditiveIdentity();
				Assert.True(
					group.First().Length == additiveIdentity.Length
					&&
					additiveIdentity.All(x => opDef.Equals(zero, zero))
				);
			}
		}

		public static void TestInnerProduct<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().InnerProduct(y.First())));

			var opDef = new T();
			var zero = opDef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Zip(group.Reverse(), (a, b) => (a, b))
						.All(p => opDef
							.Equals(
								p.a.InnerProduct(p.b),
								p.a
									.Zip(p.b, (x, y) => (x, y))
									.Aggregate(
										zero,
										(a, x) => opDef.Add(a, opDef.Multiply(x.x, x.y))
									)
							)
						)
				);
			}
		}

		public static void TestSlice<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			var slices = new[] {(0, 0), (0, 1), (0, -1), (1, 0), (1, 1), (1, -1), (-1, 0), (-1, 1), (-1, -1), (312456, -745678)};

			foreach (var vector in vectors)
			{
				foreach (var slice in slices)
				{
					if (vector.Length == 0)
					{
						Assert.False(vector.Slice(slice.Item1, slice.Item2).Any());
					}
					else
					{
						var first = slice.Item1 % vector.Length >= 0
							? slice.Item1 % vector.Length
							: vector.Length + slice.Item1 % vector.Length;
						var second = slice.Item2 % vector.Length >= 0
							? slice.Item2 % vector.Length
							: vector.Length + slice.Item2 % vector.Length;

						if (second > first)
							Assert.True(vector.Slice(slice.Item1, slice.Item2).SequenceEqual(vector.Skip(first).Take(second - first)));
						else
							Assert.True(vector.Slice(slice.Item1, slice.Item2)
								.SequenceEqual(vector.Skip(first).Concat(vector.Take(second))));
					}
				}
			}
		}

		public static void TestApplyOnTwoVectors<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);
			var opDef = new T();

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().Apply((a, b) => opDef.Add(a, b), y.First())));

			foreach (var group in groups)
			{
				Assert.True(
					group
						.Aggregate((x, y) => x.Add(y))
						.AreEqual(
							group.Aggregate(
								(x, y) => x.Apply((a, b) => opDef.Add(a, b), y))
							,opDef)
				);
			}
		}

		public static void TestApplyOnOneVectors<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var opDef = new T();
			var zero = opDef.Zero;

			foreach (var vector in vectors)
			{
				if (vector.Length == 0)
				{
					Assert.True(
						vector.Apply(x => zero)
							.AreEqual(vector, opDef
							)
					);
                    continue;
				}

				var scalar = vector.First();
				Assert.True(
					vector.Apply(x => opDef.Multiply(scalar, x))
						.AreEqual(
							vector.LeftScale(scalar)
							, opDef)
				);
			}
		}

        public static void RunIVectorTestSuite<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			vectors = vectors.ToList();
			TestLength(vectors);
			TestArrayAccessors(vectors);
			TestEquals(vectors);
			TestLeftScale(vectors);
			TestRightScale(vectors);
			TestAddition(vectors);
			TestNegative(vectors);
			TestSubtraction(vectors);
			AdditiveIdentity(vectors);
			TestInnerProduct(vectors);
			TestSlice(vectors);
			TestApplyOnTwoVectors(vectors);
			TestApplyOnOneVectors(vectors);
		}

		private static bool AreEqual<S, T>(this IEnumerable<S> vector1, IEnumerable<S> vector2, T opDef)
			where T : IRingOperationDefiner<S>
		{
			return vector1.Zip(vector2, (x, y) => (x, y)).All(p => opDef.Equals(p.x, p.y));
		}
	}
}