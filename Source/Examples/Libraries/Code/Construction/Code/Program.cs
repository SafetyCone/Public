

namespace Public.Examples.Code.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.Test();
        }

        private static void Test()
        {
            EquatableClass equatable = new EquatableClass();
            EqualsClassDescendant descendant = new EqualsClassDescendant();

            equatable.Equals(descendant);
        }
    }
}
