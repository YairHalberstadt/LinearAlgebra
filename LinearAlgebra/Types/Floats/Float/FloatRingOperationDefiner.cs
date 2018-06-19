using System;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Floats.Float
{
    public class FloatRingOperationDefiner : IRingOperationDefiner<float>
    {
        public const double TOLERANCE = 0.0001;

        public bool Equals(float first, float second) => Math.Abs(first - second) < TOLERANCE;

        public float Add(float first, float second) => first + second;

        public float Multiply(float first, float second) => first * second;

	    public float Zero => 0;

	    public float Negative(float operand) => -operand;

	    public float One => 1;

        public float Subtract(float first, float second) => first - second;
    }
}
