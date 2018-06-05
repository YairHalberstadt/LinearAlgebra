namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface IOneOperationDefiner<T> : IMultiplicationOperationDefiner<T>
    {
        /// <summary>
        /// Operation.Equal(Operation.Multiply(t, Operation.Zero), t) must return true for all t.
        /// </summary>
        T One { get; }
    }
}
