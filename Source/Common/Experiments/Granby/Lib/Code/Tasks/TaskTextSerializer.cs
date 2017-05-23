using System;
using System.Collections.Generic;


namespace Public.Common.Granby.Lib
{
    public class TaskTextSerializer : AegeanSerializerBase<ITask>
    {
        public TaskTextSerializer(TaskFactory taskFactory)
            : base(taskFactory)
        {
        }

        public TaskTextSerializer(TaskFactory taskFactory, ILog log)
            : base(taskFactory, log)
        {
        }
    }
}
