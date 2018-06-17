using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Integers.Int32
{
    public class Int32RingOperationDefiner : IRingOperationDefiner<int>
    {
        public bool Equals(int first, int second) => first == second;

        public int Add(int first, int second) => first + second;

        public int Multiply(int first, int second) => first * second;

	    public int Zero => 0;

	    public int Negative(int operand) => -operand;

	    public int One => 1;

        public int Subtract(int first, int second) => first - second;
    }
}
