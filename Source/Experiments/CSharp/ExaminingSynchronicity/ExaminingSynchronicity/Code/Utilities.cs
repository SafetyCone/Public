using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace ExaminingSynchronicity
{
    public static class Utilities
    {
        /// <summary>
        /// Will block the calling thread for three seconds!
        /// </summary>
        public static Task BlockThreeSeconds()
        {
            Thread.Sleep(3000);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Will block the calling thread for three seconds!
        /// </summary>
        public static Task<string> BlockThreeSecondsAndReturnString()
        {
            Thread.Sleep(3000);

            return Task.FromResult(@"Hello world!");
        }

        public static Task TaskThreeSeconds()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(3000);

                return Task.CompletedTask;
            });
        }

        public static async Task AwaitThreeSeconds()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);

                return Task.CompletedTask;
            });
        }

        public static List<BasicWorkItem> GetThreeWorkItems(TextWriter writer)
        {
            var numberOfWorkItems = 3;

            var workItems = new List<BasicWorkItem>(numberOfWorkItems);
            for (int i = 0; i < numberOfWorkItems; i++)
            {
                workItems.Add(new BasicWorkItem(writer));
            }

            return workItems;
        }

        public static List<Task> GetThreeWorkItemTasks(TextWriter writer)
        {
            var workItems = Utilities.GetThreeWorkItems(writer);

            var tasks = new List<Task>(workItems.Count);
            foreach (var workItem in workItems)
            {
                tasks.Add(workItem.Run());
            }

            return tasks;
        }
    }
}
