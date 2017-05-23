using System;


namespace Public.Common.Granby.Lib
{
    public class ExceptionDummyTask : ITask
    {
        #region ITask Members

        public void Run()
        {
            throw new Exception(this.Message);
        }

        #endregion


        public string Message { get; set; }


        public ExceptionDummyTask()
        {
        }

        public ExceptionDummyTask(string message)
        {
            this.Message = message;
        }
    }
}
