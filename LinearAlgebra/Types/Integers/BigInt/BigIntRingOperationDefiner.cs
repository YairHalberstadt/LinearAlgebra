using System.Numerics;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Integers.BigInt
{
    public struct BigIntRingOperationDefiner : IRingOperationDefiner<BigInteger>
    {
        public bool Equals(BigInteger first, BigInteger second) => first == second;

        public BigInteger Add(BigInteger first, BigInteger second) => first + second;

        public BigInteger Multiply(BigInteger first, BigInteger second) => first * second;

	    public BigInteger Zero => 0;

	    public BigInteger Negative(BigInteger operand) => -operand;

	    public BigInteger One => 1;

        public BigInteger Subtract(BigInteger first, BigInteger second) => first - second;
    }
}
