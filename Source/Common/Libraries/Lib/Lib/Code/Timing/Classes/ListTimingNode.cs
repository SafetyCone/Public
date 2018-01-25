using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Public.Common.Lib
{
    [DebuggerDisplay("{Description} - List - Sub-node count: {SubNodes.Count}")]
    [Serializable]
    public class ListTimingNode : ITimingNode
    {
        public string Description { get; }
        public TimeSpan ElapsedTime { get; set; }
        public List<ITimingNode> SubNodes { get; }


        public ListTimingNode(string description, TimeSpan elapsedTime)
            : this(description)
        {
            this.ElapsedTime = elapsedTime;
        }

        public ListTimingNode(string description)
        {
            this.SubNodes = new List<ITimingNode>();

            this.Description = description;
        }

        public void AddSubNode(ITimingNode subNode)
        {
            this.SubNodes.Add(subNode);
        }
    }
}
