using System;


namespace ExaminingSynchronicity
{
    public static class Demonstrations
    {
        public static void SubMain()
        {
            //Demonstrations.FalseAsync();
            //Demonstrations.FalseAsyncWithInterveningWork();
            //Demonstrations.SimpleAsync();
            Demonstrations.SimpleAsyncWithInterveningWork();
        }

        /// <summary>
        /// Demonstrates how to call an asynchronous method, get the returned task object, do work during the processing of the task, then wait for the asynchronous task from a synchronous method.
        /// </summary>
        private static void SimpleAsyncWithInterveningWork()
        {
            var writer = Console.Out;

            writer.WriteLine(@"Calling asynchronous wait...");

            var task = Utilities.AwaitThreeSeconds();

            writer.WriteLine(@"Doing work in the meantime...");

            task.Wait();

            writer.WriteLine(@"Called asynchronous wait.");
        }

        /// <summary>
        /// Demonstrates how to wait for an asynchronous method from a synchronous method.
        /// </summary>
        private static void SimpleAsync()
        {
            var writer = Console.Out;

            writer.WriteLine(@"Calling asynchronous wait...");

            Utilities.AwaitThreeSeconds().Wait();

            writer.WriteLine(@"Called asynchronous wait.");
        }

        /// <summary>
        /// Demonstrates how a even though a method returns a task, it may NOT necessarily be asynchronous!
        /// The <see cref="Utilities.BlockThreeSeconds"/> method actually sleeps the calling thread, meaning the intervening work is not done until the current thread comes out of the function returning the task.
        /// </summary>
        private static void FalseAsyncWithInterveningWork()
        {
            var writer = Console.Out;

            writer.WriteLine(@"Calling FALSE asynchronous wait...");

            var task = Utilities.BlockThreeSeconds();

            writer.WriteLine(@"Doing work in the meantime...");

            task.Wait();

            writer.WriteLine(@"Called FALSE asynchronous wait.");
        }

        /// <summary>
        /// Demonstrates how a even though a method returns a task, it may NOT necessarily be asynchronous!
        /// The <see cref="Utilities.BlockThreeSeconds"/> method actually sleeps the calling thread. You don't notice it here, but you do when you try to do intervening work as in <see cref="Demonstrations.FalseAsyncWithInterveningWork"/>.
        /// </summary>
        private static void FalseAsync()
        {
            var writer = Console.Out;

            writer.WriteLine(@"Calling FALSE asynchronous wait...");

            Utilities.BlockThreeSeconds().Wait();

            writer.WriteLine(@"Called FALSE asynchronous wait.");
        }
    }
}
