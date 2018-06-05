using System;
using System.Collections.Generic;
using System.Text;
using OperationDefiners.CoreOperationDefinerInterfaces;

namespace OperationDefiners.CoreOperationDefinerImplementations
{
    abstract class RuntimeOperationDefiner<T> : IOperationDefiner<T>
    {
    }

    class RuntimeRingOperationDefiner<T> : RuntimeOperationDefiner<T>, IRingOperationDefiner<T>
    {
        public static RingOperationFuncs<T> Operations { get; }



        public bool Equals(T first, T second)
        {
            throw new NotImplementedException();
        }

        public T Add(T first, T second)
        {
            throw new NotImplementedException();
        }

        public T Multiply(T first, T second)
        {
            throw new NotImplementedException();
        }

        public T Zero { get; }

        public T Negative(T operand)
        {
            throw new NotImplementedException();
        }

        public T One { get; }
    }

    internal class RingOperationFuncs<T>
    {
    }
}
