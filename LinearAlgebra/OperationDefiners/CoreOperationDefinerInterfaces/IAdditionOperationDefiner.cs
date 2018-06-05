namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface IAdditionOperationDefiner<T> : IEqualityOperationDefiner<T>
    {
        /// <summary>
        /// Must Be Comutative And Associative
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        T Add(T first, T second);
    }
}
