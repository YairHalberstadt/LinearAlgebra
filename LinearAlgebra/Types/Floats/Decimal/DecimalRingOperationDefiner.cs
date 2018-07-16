using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Floats.Decimal
{
    public struct DecimalRingOperationDefiner : IRingOperationDefiner<decimal>
    {
        public bool Equals(decimal first, decimal second) => first == second;

        public decimal Add(decimal first, decimal second) => first + second;

        public decimal Multiply(decimal first, decimal second) => first * second;

	    public decimal Zero => 0;

	    public decimal Negative(decimal operand) => -operand;

	    public decimal One => 1;

        public decimal Subtract(decimal first, decimal second) => first - second;
    }
}
