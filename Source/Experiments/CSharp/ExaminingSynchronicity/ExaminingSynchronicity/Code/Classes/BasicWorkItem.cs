using System;
using System.IO;
using System.Threading.Tasks;


namespace ExaminingSynchronicity
{
    public class BasicWorkItem
    {
        private TextWriter Writer { get; }


        public BasicWorkItem(TextWriter writer)
        {
            this.Writer = writer;
        }

        public async Task Run()
        {
            await this.Writer.WriteLineAsync($@"Running basic work item...");

            await Utilities.AwaitThreeSeconds();

            await this.Writer.WriteLineAsync($@"Ran basic work item.");
        }
    }
}
