using System;
using System.IO;
using System.Threading.Tasks;


namespace ExaminingSynchronicity
{
    public static class Experiments
    {
        public static void SubMain()
        {
            //Experiments.RunThreeWorkItems();
            //Experiments.RunThreeWorkItemsAsync().Wait();
            //Experiments.RunLevelsAsync();
            Experiments.RunLevelsTask();
        }

        private static void RunLevelsTask()
        {
            Experiments.Level3Task(Console.Out).Wait();

            // Output:
            //Beginning Level3Task
            //Beginning Level2Task
            //Beginning Level1Task
            //Intermediate Level2Task
            //Ending Level2Task
            //Intermediate Level3Task
            //Ending Level3Task
            //Intermediate Level1Task
            //Ending Level1Task
        }

        private static Task Level3Task(TextWriter writer)
        {
            string name = nameof(Level3Task);
            writer.WriteLineAsync($@"Beginning {name}");

            var task = Experiments.Level2Task(writer);

            writer.WriteLineAsync($@"Intermediate {name}");

            writer.WriteLineAsync($@"Ending {name}");

            return task;
        }

        private static Task Level2Task(TextWriter writer)
        {
            string name = nameof(Level2Task);
            writer.WriteLineAsync($@"Beginning {name}");

            var task = Experiments.Level1Task(writer);

            writer.WriteLineAsync($@"Intermediate {name}");

            writer.WriteLineAsync($@"Ending {name}");

            return task;
        }

        private static async Task Level1Task(TextWriter writer)
        {
            string name = nameof(Level1Task);
            await writer.WriteLineAsync($@"Beginning {name}");

            //var task = Utilities.AwaitThreeSeconds();
            await Utilities.TaskThreeSeconds();

            await writer.WriteLineAsync($@"Intermediate {name}");

            //task.Wait();

            await writer.WriteLineAsync($@"Ending {name}");

            //return Task.CompletedTask;
            //return task;
        }

        private static void RunLevelsAsync()
        {
            Experiments.Level3Async(Console.Out).Wait();
        }

        private static async Task Level3Async(TextWriter writer)
        {
            string name = nameof(Level3Async);
            await writer.WriteLineAsync($@"Beginning {name}");

            await Experiments.Level2Async(writer);

            await writer.WriteLineAsync($@"Ending {name}");
        }

        private static async Task Level2Async(TextWriter writer)
        {
            string name = nameof(Level2Async);
            await writer.WriteLineAsync($@"Beginning {name}");

            await Experiments.Level1Async(writer);

            await writer.WriteLineAsync($@"Ending {name}");
        }

        private static async Task Level1Async(TextWriter writer)
        {
            string name = nameof(Level1Async);
            await writer.WriteLineAsync($@"Beginning {name}");

            await Utilities.AwaitThreeSeconds();

            await writer.WriteLineAsync($@"Ending {name}");
        }

        private static async Task RunThreeWorkItemsAsync()
        {
            var tasks = Utilities.GetThreeWorkItemTasks(Console.Out);

            await Task.WhenAll(tasks);
        }

        private static void RunThreeWorkItems()
        {
            var tasks = Utilities.GetThreeWorkItemTasks(Console.Out);

            Task.WaitAll(tasks.ToArray());
        }
    }
}
