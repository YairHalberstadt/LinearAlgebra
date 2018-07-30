namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface INegativeOperationDefiner<T> : IZeroOperationDefiner<T>
    {
        /// <summary>
        /// Operation.Equal(Operation.Add(t, Negative(t)), Operation.Zero) must return true for all t. 
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        T Negative(T operand);

	    T Subtract(T first, T second);
    }
}
