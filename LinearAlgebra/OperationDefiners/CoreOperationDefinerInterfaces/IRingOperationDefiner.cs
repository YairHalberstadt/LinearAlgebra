namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    /// <summary>
    ///Must Fulfil Definition Of Ring over T:
    ///
    ///Definition of a ring:
    ///A ring is a set R equipped with two binary operations[1] + and · satisfying the following three sets of axioms, called the ring axioms[2][3] [4]
    ///
    ///1. R is an abelian group under addition, meaning that:
    ///
    ///		(a + b) + c = a + (b + c) for all a, b, c in R(that is, + is associative).
    ///		a + b = b + a for all a, b in R(that is, + is commutative).
    ///		There is an element 0 in R such that a + 0 = a for all a in R(that is, 0 is the additive identity).
    ///		For each a in R there exists −a in R such that a + (−a) = 0   (that is, −a is the additive inverse of a).
    ///
    ///2. R is a monoid under multiplication, meaning that:
    ///
    ///		(a * b) * c = a * (b * c) for all a, b, c in R(that is, * is associative).
    ///		There is an element 1 in R such that a * 1 = a and 1 * a = a for all a in R(that is, 1 is the multiplicative identity).
    ///
    ///3. Multiplication is distributive with respect to addition:
    ///
    ///		a * (b + c) = (a * b) + (a * c) for all a, b, c in R(left distributivity).
    ///		(b + c) * a = (b * a) + (c * a) for all a, b, c in R(right distributivity).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRingOperationDefiner<T> : IAdditionOperationDefiner<T>, IEqualityOperationDefiner<T>,
        IMultiplicationOperationDefiner<T>, INegativeOperationDefiner<T>, IZeroOperationDefiner<T>, IOneOperationDefiner<T>
    {
    }
}
