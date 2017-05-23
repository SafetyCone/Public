using System;
using System.Diagnostics;


namespace Public.Common.Granby.Lib
{
    public class RunExecutableTask : ITask
    {
        #region ITask Members

        public void Run()
        {
            Process.Start(this.ExecutableFileRootedPath, this.Arguments);
        }

        #endregion


        public string ExecutableFileRootedPath { get; set; }
        public string Arguments { get; set; }


        public RunExecutableTask() { }

        public RunExecutableTask(string executableFileRootedPath, string arguments)
        {
            this.ExecutableFileRootedPath = executableFileRootedPath;
            this.Arguments = arguments;
        }
    }
}
