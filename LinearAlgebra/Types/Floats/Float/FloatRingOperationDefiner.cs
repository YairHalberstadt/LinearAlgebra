using System;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace Types.Floats.Float
{
	/// <summary>
	/// Due to the difficulty of floating point comparison, it is impossible to make a float a group
	/// this is because no matter the implementation of == it is either possible that ((a + b) + c) != (a + (b + c))
	/// or it is possible that a == b, b == c, c != a.
	/// Hence although this implementation of IRingOperationDefiner&lt;float&gt; is provided,
	/// we recommend you use the DecimalRingOperationDefiner instead.
	/// </summary>
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
