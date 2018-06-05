namespace OperationDefiners.CoreOperationDefinerInterfaces
{
    public interface IZeroOperationDefiner<T> : IAdditionOperationDefiner<T>
    {
        /// <summary>
        /// Operation.Equal(Operation.Add(t, Operation.Zero), t) must return true for all t.
        /// </summary>
        T Zero { get; }
    }
}
