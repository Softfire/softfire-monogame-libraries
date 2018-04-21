namespace Softfire.MonoGame.UTIL
{
    public static class CustomTuples
    {
        /// <summary>
        /// Double Tuple.
        /// </summary>
        /// <typeparam name="T1">First item.</typeparam>
        /// <typeparam name="T2">Second item.</typeparam>
        public class DoubleTuple<T1, T2>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }

            public DoubleTuple()
            {

            }

            public DoubleTuple(T1 first, T2 second)
            {
                First = first;
                Second = second;
            }
        }

        /// <summary>
        /// Ttriple Tuple.
        /// </summary>
        /// <typeparam name="T1">First item.</typeparam>
        /// <typeparam name="T2">Second item.</typeparam>
        /// <typeparam name="T3">Third item.</typeparam>
        public class TripleTuple<T1, T2, T3>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }
            public T3 Third { get; set; }

            public TripleTuple()
            {

            }

            public TripleTuple(T1 first, T2 second, T3 third)
            {
                First = first;
                Second = second;
                Third = third;
            }
        }

        /// <summary>
        /// Quadruple Tuple.
        /// </summary>
        /// <typeparam name="T1">First item.</typeparam>
        /// <typeparam name="T2">Second item.</typeparam>
        /// <typeparam name="T3">Third item.</typeparam>
        /// <typeparam name="T4">Fourth item.</typeparam>
        public class QuadrupleTuple<T1, T2, T3, T4>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }
            public T3 Third { get; set; }
            public T4 Fourth { get; set; }

            public QuadrupleTuple()
            {

            }

            public QuadrupleTuple(T1 first, T2 second, T3 third, T4 fourth)
            {
                First = first;
                Second = second;
                Third = third;
                Fourth = fourth;
            }
        }
    }
}