using System;

namespace OperationDefiners.CoreOperationDefinerImplementations.RuntimeOperationsDefiners
{
    public class RingOperationsDefinition<T>
    {
        public RingOperationsDefinition(Func<T, T, bool> equalsFunc, Func<T, T, T> addFunc, Func<T, T, T> multiplyFunc, T zero, Func<T, T> negativeFunc, T one)
        {
            EqualsFunc = equalsFunc;
            AddFunc = addFunc;
            MultiplyFunc = multiplyFunc;
            Zero = zero;
            NegativeFunc = negativeFunc;
            One = one;
        }

        public Func<T,T, bool> EqualsFunc { get; }

        public Func<T,T,T> AddFunc { get; }

        public  Func<T,T,T> MultiplyFunc { get; }

        public T Zero { get; }

        public Func<T,T> NegativeFunc { get; }

        public T One { get; }
    }
}