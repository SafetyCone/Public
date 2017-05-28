using System;
using System.Collections.Generic;

using Public.Common.Lib.Logging;


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
