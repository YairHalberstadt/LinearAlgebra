namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface IMultiplicationOperationDefiner<T> : IEqualityOperationDefiner<T>
    {
        /// <summary>
        /// Must be Associative
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        T Multiply(T first, T second);
    }
}
