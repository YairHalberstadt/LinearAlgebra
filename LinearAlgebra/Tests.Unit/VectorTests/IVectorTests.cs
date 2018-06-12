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
	class IVectorTests
	{
		public static void TestLength<S, T>(IEnumerable<IVector<S, T>> vectors) where T : IRingOperationDefiner<S>, new()
		{
			foreach (var vector in vectors)
				Assert.AreEqual(vector.Length, vector.Count());
		}

		public static void TestArrayAccessors<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			foreach (var vector in vectors)
				Assert.True(vector.Select((x, i) => (x, vector[i])).All(p => p.Item1.Equals(p.Item2)));
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
					Assert.True(vector.Select((x, i) => (opDef.Multiply(scalar, x), scaled[i]))
						.All(p => opDef.Equals(p.Item1, p.Item2)));
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
					Assert.True(vector.Select((x, i) => (opDef.Multiply(x, scalar), scaled[i]))
						.All(p => opDef.Equals(p.Item1, p.Item2)));
				}
			}
		}

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

			var opdef = new T();
			var zero = opdef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Aggregate(group.First().AdditiveIdentity(), (x, y) => x.Subtract(y))
						.SequenceEqual(
							group.Aggregate(
								Enumerable.Repeat(zero, group.Key),
								(x, y) => x.Zip(y, (f, s) => opdef.Subtract(f, s))
							)
						)
				);
			}
		}

		public static void AdditiveIdentity<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			var opdef = new T();
			var zero = opdef.Zero;
			foreach (var group in groups)
			{
				var additiveIdentity = group.First().AdditiveIdentity();
				Assert.True(
					group.First().Length == additiveIdentity.Length
					&&
					additiveIdentity.All(x => opdef.Equals(zero, zero))
				);
			}
		}

		public static void TestInnerProduct<S, T>(IEnumerable<IVector<S, T>> vectors)
			where T : IRingOperationDefiner<S>, new()
		{
			var groups = vectors.GroupBy(x => x.Length);

			groups.SelectFromAdjacentPairs((x, y) =>
				Assert.Throws<ArgumentOutOfRangeException>(() => x.First().InnerProduct(y.First())));

			var opdef = new T();
			var zero = opdef.Zero;
			foreach (var group in groups)
			{
				Assert.True(
					group
						.Zip(group.Reverse(), (a, b) => (a, b))
						.All(p => opdef
							.Equals(
								p.a.InnerProduct(p.b),
								p.a
									.Zip(p.b, (x, y) => (x, y))
									.Aggregate(
										zero,
										(a, x) => opdef.Add(a, opdef.Multiply(x.x, x.y))
									)
							)
						)
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
		}
	}
}