using System;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Complex
{
    public struct ComplexRingOperationDefiner : IRingOperationDefiner<System.Numerics.Complex>
    {
        public const double TOLERANCE = 0.0001;

        public bool Equals(System.Numerics.Complex first, System.Numerics.Complex second) => Math.Abs(first.Real - second.Real) < TOLERANCE && Math.Abs(first.Imaginary - second.Imaginary) < TOLERANCE;

        public System.Numerics.Complex Add(System.Numerics.Complex first, System.Numerics.Complex second) => first + second;

        public System.Numerics.Complex Multiply(System.Numerics.Complex first, System.Numerics.Complex second) => first * second;

	    public System.Numerics.Complex Zero => 0;

	    public System.Numerics.Complex Negative(System.Numerics.Complex operand) => -operand;

	    public System.Numerics.Complex One => 1;

        public System.Numerics.Complex Subtract(System.Numerics.Complex first, System.Numerics.Complex second) => first - second;
    }
}
