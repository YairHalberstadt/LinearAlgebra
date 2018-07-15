using System.Collections.Generic;
using System.Linq;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Tests.Utils
{
    public static class VectorUtils
    {
        public static bool AreEqual<S, T>(this IEnumerable<S> vector1, IEnumerable<S> vector2, T opDef)
            where T : IRingOperationDefiner<S>
        {
            return vector1.Zip(vector2, (x, y) => (x, y)).All(p => opDef.Equals(p.x, p.y));
        }
    }
}
