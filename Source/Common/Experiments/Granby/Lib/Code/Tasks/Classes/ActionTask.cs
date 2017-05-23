using System;


namespace Public.Common.Granby.Lib
{
    public class ActionTask : ITask
    {
        #region ITask Members

        public void Run()
        {
            this.Action();
        }

        #endregion


        public Action Action { get; set; }


        public ActionTask()
        {
        }

        public ActionTask(Action action)
        {
            this.Action = action;
        }
    }
}
