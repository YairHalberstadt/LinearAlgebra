using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Integers.Int16
{
    public class Int16RingOperationDefiner : IRingOperationDefiner<short>
    {
        public bool Equals(short first, short second) => first == second;

        public short Add(short first, short second) => (short) (first + second);

        public short Multiply(short first, short second) => (short) (first * second);

	    public short Zero => 0;

	    public short Negative(short operand) => (short) -operand;

	    public short One => 1;

        public short Subtract(short first, short second) => (short) (first - second);
    }
}
