using System;


namespace Public.Common.Lib.Code
{
    public class ActionCommand : CommandBase
    {
        public Action Action { get; protected set; }


        public ActionCommand(Action action)
        {
            this.Action = action;
        }

        public override void Run()
        {
            this.Action();
        }
    }
}
