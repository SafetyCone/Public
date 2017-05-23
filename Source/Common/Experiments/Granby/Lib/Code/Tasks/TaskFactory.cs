using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    public class TaskFactory : AegeanFactoryBase<ITask>
    {
        protected const char SpecificationTokenSeparator = '_'; // Can't use spaces since the message box dummy task will want to use spaces in its message.
        public const string DebugOutputStreamDummyTaskKey = @"DebugOutputStreamTask";
        public const string ExceptionDummyTaskKey = @"ExceptionTask";
        public const string MessageBoxDummyTaskKey = @"MessageBoxTask";
        public const string RunExecutableTaskKey = @"RunExecutable";


        #region Static

        private static void AddDefaultConstructors(Dictionary<string, Func<string[], ITask>> constructors)
        {
            constructors.Add(TaskFactory.DebugOutputStreamDummyTaskKey, TaskFactory.GetDebugOutptStreamDummyTask);
            constructors.Add(TaskFactory.ExceptionDummyTaskKey, TaskFactory.GetExceptionDummyTask);
            constructors.Add(TaskFactory.MessageBoxDummyTaskKey, TaskFactory.GetMessageBoxDummyTask);
            constructors.Add(TaskFactory.RunExecutableTaskKey, TaskFactory.GetRunExecutableTask);
        }

        private static ITask GetDebugOutptStreamDummyTask(string[] tokens)
        {
            string messageToken = tokens[1];

            DebugOutputStreamDummyTask output = new DebugOutputStreamDummyTask(messageToken);
            return output;
        }

        private static ITask GetExceptionDummyTask(string[] tokens)
        {
            string messageToken = tokens[1];

            ExceptionDummyTask output = new ExceptionDummyTask(messageToken);
            return output;
        }

        private static ITask GetMessageBoxDummyTask(string[] tokens)
        {
            string messageToken = tokens[1];

            MessageBoxDummyTask output = new MessageBoxDummyTask(messageToken);
            return output;
        }

        private static ITask GetRunExecutableTask(string[] tokens)
        {
            string executablePathToken = tokens[1];
            string argumentsToken;
            if (2 < tokens.Length)
            {
                argumentsToken = tokens[2];
            }
            else
            {
                argumentsToken = String.Empty;
            }

            RunExecutableTask output = new RunExecutableTask(executablePathToken, argumentsToken);
            return output;
        }

        #endregion


        public TaskFactory()
            : base(TaskFactory.SpecificationTokenSeparator)
        {
            TaskFactory.AddDefaultConstructors(this.zConstructors);
        }
    }
}
