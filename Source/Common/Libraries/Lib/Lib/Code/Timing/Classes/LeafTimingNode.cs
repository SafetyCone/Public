using System;
using System.Diagnostics;


namespace Public.Common.Lib
{
    [DebuggerDisplay("{Description} - Leaf")]
    [Serializable]
    public class LeafTimingNode : ITimingNode
    {
        public string Description { get; }
        public TimeSpan ElapsedTime { get; set; }


        public LeafTimingNode(string description, TimeSpan elapsedTime)
            : this(description)
        {
            this.ElapsedTime = elapsedTime;
        }

        public LeafTimingNode(string description)
        {
            this.Description = description;
        }
    }
}
