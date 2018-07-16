using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Integers.Byte
{
    public struct ByteRingOperationDefiner : IRingOperationDefiner<byte>
    {
        public bool Equals(byte first, byte second) => first == second;

        public byte Add(byte first, byte second) => (byte) (first + second);

        public byte Multiply(byte first, byte second) => (byte) (first * second);

	    public byte Zero => 0;

	    public byte Negative(byte operand) => (byte) -operand;

	    public byte One => 1;

        public byte Subtract(byte first, byte second) => (byte) (first - second);
    }
}
