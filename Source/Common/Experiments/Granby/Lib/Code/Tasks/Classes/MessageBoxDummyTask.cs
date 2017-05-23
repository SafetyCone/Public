using System;
using System.Windows.Forms;


namespace Public.Common.Granby.Lib
{
    public class MessageBoxDummyTask : ITask
    {
        #region ITask Members

        public void Run()
        {
            MessageBox.Show(this.Message);
        }

        #endregion


        public string Message { get; set; }


        public MessageBoxDummyTask()
        {
        }

        public MessageBoxDummyTask(string message)
        {
            this.Message = message;
        }
    }
}
