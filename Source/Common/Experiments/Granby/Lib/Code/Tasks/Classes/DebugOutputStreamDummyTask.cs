using System;

using Public.Common.Lib.IO;


namespace Public.Common.Granby.Lib
{
    public class DebugOutputStreamDummyTask : OutputStreamDummyTask
    {
        public DebugOutputStreamDummyTask(string message)
            : base(new DebugOutputStream(), message)
        {
        }
    }
}
