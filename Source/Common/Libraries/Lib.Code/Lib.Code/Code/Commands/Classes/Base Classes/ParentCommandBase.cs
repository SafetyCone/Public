using System.Collections.Generic;


namespace Public.Common.Lib.Code
{
    public abstract class ParentCommandBase : CommandBase
    {
        protected List<ICommand> ChildCommands { get; private set; }


        public ParentCommandBase()
        {
            this.ChildCommands = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            this.ChildCommands.Add(command);
        }

        public override void Run()
        {
            this.BeforeRun();

            foreach(ICommand childCommand in ChildCommands)
            {
                childCommand.Run();
            }

            this.AfterRun();
        }

        protected virtual void BeforeRun()
        {
        }

        protected virtual void AfterRun()
        {
        }
    }
}
