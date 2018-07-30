using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Tests.Unit.OperationDefinerTests
{
    class IRingOperationDefinerTests
    {
	    public void RunAll<T>(IRingOperationDefiner<T> opDef, IEnumerable<T> data) => SubtractTest(opDef, data);

	    private void SubtractTest<T>(IRingOperationDefiner<T> opDef, IEnumerable<T> data)
	    {
		    var items = data as T[] ?? data.ToArray();
		    var first = items.Aggregate(opDef.Zero, opDef.Subtract);
		    var second = items.Aggregate(opDef.Zero, (x, y) => opDef.Add(x, opDef.Negative(y)));
		    Assert.True(opDef.Equals(first, second));
	    }
    }
}
