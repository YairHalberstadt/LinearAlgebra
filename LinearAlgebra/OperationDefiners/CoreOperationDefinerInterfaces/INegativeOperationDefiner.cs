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
    }

	public static class INegativeOperationDefinerExtensions
	{
		public static T Subtract<T>(this INegativeOperationDefiner<T> opDef, T first, T subtrand) => opDef.Add(first, opDef.Negative(subtrand));
	}
}
