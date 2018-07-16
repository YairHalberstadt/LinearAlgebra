using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Integers.Int64
{
    public struct Int64RingOperationDefiner : IRingOperationDefiner<long>
    {
        public bool Equals(long first, long second) => first == second;

        public long Add(long first, long second) => first + second;

        public long Multiply(long first, long second) => first * second;

	    public long Zero => 0;

	    public long Negative(long operand) => -operand;

	    public long One => 1;

        public long Subtract(long first, long second) => first - second;
    }
}
