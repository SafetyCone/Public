using System;
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

        public static async Task AwaitThreeSeconds()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);

                return Task.CompletedTask;
            });
        }
    }
}
