using System;

using Public.Common.Lib.IO;


namespace Public.Common.Granby.Lib
{
    public class OutputStreamDummyTask : ITask
    {
        #region ITask Members

        public void Run()
        {
            this.OutputStream.WriteLine(this.Message);
        }

        #endregion


        public IOutputStream OutputStream { get; set; }
        public string Message { get; set; }


        public OutputStreamDummyTask(IOutputStream outputStream, string message)
        {
            this.OutputStream = outputStream;
            this.Message = message;
        }
    }
}
