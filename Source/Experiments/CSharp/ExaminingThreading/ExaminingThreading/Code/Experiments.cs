using System;
using System.Threading;


namespace ExaminingThreading
{
    public static class Experiments
    {
        public static void SubMain()
        {
            Experiments.HaveAThreadReactivateAnotherThread();
        }

        private static void HaveAThreadReactivateAnotherThread()
        {
            var writer = Console.Out;

            void TestMethod()
            {
                writer.WriteLine($@"{nameof(TestMethod)}, thread: {Thread.CurrentThread.ManagedThreadId}");
            }

            var threadStart = new ThreadStart(TestMethod);
            var thread = new Thread(threadStart);
            thread.Start();

            thread.Join();

            Thread.Sleep(2000);

            writer.WriteLine($@"{nameof(HaveAThreadReactivateAnotherThread)}, thread: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
