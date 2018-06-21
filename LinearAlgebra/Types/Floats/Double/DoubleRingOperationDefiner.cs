using System;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Floats.Double
{
	/// <summary>
	/// Due to the difficulty of floating point comparison, it is impossible to make a double a group
	/// this is because no matter the implementation of == it is either possible that ((a + b) + c) != (a + (b + c))
	/// or it is possible that a == b, b == c, c != a.
	/// Hence although this implementation of IRingOperationDefiner&lt;double&gt; is provided,
	/// we reccommend you use the DecimalRingOperationDefiner instead.
	/// </summary>
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
