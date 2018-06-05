# LinearAlgebra
A Linear Algebra Library for C#

The Aim of this Library is to be extremely Generic, Based on Mathematical Principles, and fast, whilst keeping to pure C# Code.

## Contributing Guidelines

I am among Linqs biggest fans, but it has a time and place. The performance cost of using Linq is small, so I believe in 95% of cases, one should not even worry about using it. However in the core of a class Library, where speed is of utmost importance, it should be avoided, unless demonstratably as fast as non-Linq methods, or significantly simpler to work with.

Mathematical correctness is at the core of this Library, as is extensibility. We want the most Generic possible definition of a Vector, Matrix etc.

Would you believe it: we use Generics to do that! If C# ever get TypeClasses I will definitely add TypeClass Support.
