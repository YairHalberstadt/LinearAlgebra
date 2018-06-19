using System;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Floats.Double
{
    public class DoubleRingOperationDefiner : IRingOperationDefiner<double>
    {
        public const double TOLERANCE = 0.0001;

        public bool Equals(double first, double second) => Math.Abs(first - second) < TOLERANCE;

        public double Add(double first, double second) => first + second;

        public double Multiply(double first, double second) => first * second;

	    public double Zero => 0;

	    public double Negative(double operand) => -operand;

	    public double One => 1;

        public double Subtract(double first, double second) => first - second;
    }
}
