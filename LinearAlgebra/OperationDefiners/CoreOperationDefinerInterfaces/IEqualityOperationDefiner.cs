namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface IEqualityOperationDefiner<T> : IOperationDefiner<T>
    {
        /// <summary>
        /// Must Be Comutative
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        bool Equals(T first, T second);
    }
}
