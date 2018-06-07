using OperationDefiners.CoreOperationDefinerInterfaces;

namespace OperationDefiners.CoreOperationDefinerImplementations.RuntimeOperationsDefiners
{
    /// <summary>
    /// Bit of a hack to allow defining the neccessary operations at runtime, but in my opinion neccessary, and worth it.
    ///
    /// The Operations to be used must be defined for T in a RingOperationsDefinition{T}. They must then be set using the TrySetOperations{T} method.
    ///
    /// The Operations can only be set once per type, for the lifetime duration of the program.
    /// However there should only be a limited number of cases where this is neccessary, as most Types should have only one natural definition of a ring.
    ///
    /// This is neccessary if one want to use any vector/matrix implementation, as there is no way to tell them which Operations to use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RuntimeRingOperationDefiner<T> : RuntimeOperationDefiner<T>, IRingOperationDefiner<T>
    {
        public static RingOperationsDefinition<T> Operations { get; private set; }

        public static bool IsOperationsSet => Operations != null;

        public static bool TrySetOperations(RingOperationsDefinition<T> operations)
        {
            if (Operations != null)
                return false;
            Operations = operations;
            return true;
        }

        public bool Equals(T first, T second) => Operations.EqualsFunc(first, second);

        public T Add(T first, T second) => Operations.AddFunc(first, second);

        public T Multiply(T first, T second) => Operations.MultiplyFunc(first, second);

        public T Zero => Operations.Zero;

        public T Negative(T operand) => Operations.NegativeFunc(operand);

        public T One => Operations.One;
    }
}