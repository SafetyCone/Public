using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    public class MultipleOutputStream : IOutputStream
    {
        #region Static

        public static IOutputStream GetDebugAndConsoleOutputStream()
        {
            DebugOutputStream debug = new DebugOutputStream();
            ConsoleOutputStream console = new ConsoleOutputStream();

            MultipleOutputStream output = new MultipleOutputStream(new IOutputStream[] { debug, console });
            return output;
        }

        #endregion

        #region IOutputStream Members

        public void Write(string value)
        {
            foreach (IOutputStream outputStream in this.OutputStreams)
            {
                outputStream.Write(value);
            }
        }

        public void WriteLine(string value)
        {
            foreach (IOutputStream outputStream in this.OutputStreams)
            {
                outputStream.WriteLine(value);
            }
        }

        #endregion


        public List<IOutputStream> OutputStreams { get; private set; }


        public MultipleOutputStream()
        {
            this.OutputStreams = new List<IOutputStream>();
        }

        public MultipleOutputStream(IEnumerable<IOutputStream> outputStreams)
            : this()
        {
            this.OutputStreams.AddRange(outputStreams);
        }
    }
}
