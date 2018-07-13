using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class ImmutableArrayUtils
    {
	    public static ImmutableArray<T> UnsafeMakeImmutable<T>(this T[] array)
	    {
		    return Unsafe.As<T[], ImmutableArray<T>>(ref array);
	    }
	}
}
