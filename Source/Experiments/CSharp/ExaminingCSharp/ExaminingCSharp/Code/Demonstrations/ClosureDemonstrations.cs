using System;
using System.Collections.Generic;
using System.Linq;


namespace ExaminingCSharp
{
    /// <summary>
    /// Demonstrations of properties of closures.
    /// </summary>
    public static class ClosureDemonstrations
    {
        public static void SubMain()
        {
            //ClosureDemonstrations.CapturedVariableValue();
            ClosureDemonstrations.ForeachDoesNotCaptureReference();
        }

        /// <summary>
        /// Demonstrates that the loop-variable of a for-loop is logically "outside" of the loop. Thus a closure within the loop will capture a reference to the variable, resulting in only one closure.
        /// With C# 5, the loop-variable of a foreach-loop is logically "inside" of the loop. This a closure within the loop will get a new reference each time, resulting in multiple closures.
        /// 
        /// Source: https://blogs.msdn.microsoft.com/ericlippert/2009/11/12/closing-over-the-loop-variable-considered-harmful/
        /// </summary>
        private static void ForeachDoesNotCaptureReference()
        {
            var actions = new List<Func<int>>();

            Console.WriteLine(@"For-loop");
            for (int iValue = 0; iValue < 5; iValue++)
            {
                int action() => iValue * 2;
                actions.Add(action);
            }

            ClosureDemonstrations.Execute(actions); // Gives 10 five times.

            actions.Clear();

            Console.WriteLine(@"Foreach-loop");
            foreach (var value in Enumerable.Range(0, 5))
            {
                int action() => value * 2;
                actions.Add(action);
            }

            ClosureDemonstrations.Execute(actions); // Gives the expected 0, 2, 4, 6, 8.
        }

        /// <summary>
        /// Provides a demonstration of a closure capturing a reference to a variable.
        /// Because there is only one reference to a variable, and it does not change, only one closure is created.
        /// The expected behavior is instead creating multiple closures for multiple values of a variable.
        /// 
        /// See: https://stackoverflow.com/questions/271440/captured-variable-in-a-loop-in-c-sharp
        /// And: https://blogs.msdn.microsoft.com/ericlippert/tag/closures/
        /// And (best): https://blogs.msdn.microsoft.com/ericlippert/2009/11/12/closing-over-the-loop-variable-considered-harmful/
        /// </summary>
        private static void CapturedVariableValue()
        {
            var actions = new List<Func<int>>();
            int variable = 0;

            Console.WriteLine(@"Lambda Function");
            while(variable < 5)
            {
                actions.Add(() => variable * 2);

                variable += 1;
            }

            ClosureDemonstrations.Execute(actions); // Just writes 10 five times.

            actions.Clear();
            variable = 0;

            // Do local functions behave any differently? NO!
            Console.WriteLine(@"Local Function");
            while (variable < 5)
            {
                int action() => variable * 2;
                actions.Add(action);

                variable += 1;
            }

            ClosureDemonstrations.Execute(actions); // Again, just writes 10 five times.

            actions.Clear();
            variable = 0;

            // Get the expected behavior by creating a per-loop reference to the variable and close on that.
            Console.WriteLine(@"Loop Reference");
            while (variable < 5)
            {
                var loopVariable = variable;
                //actions.Add(() => loopVariable * 2); // No difference between lambda function and local function.
                int action() => loopVariable * 2;
                actions.Add(action);

                variable += 1;
            }

            ClosureDemonstrations.Execute(actions); // Provides the expected 0, 2, 4, 6, 8.
        }

        private static void Execute<T>(IEnumerable<Func<T>> actions)
        {
            foreach (var action in actions)
            {
                Console.WriteLine(action());
            }
        }
    }
}
