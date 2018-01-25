using System.Diagnostics;


namespace Public.Common.Lib
{
    [DebuggerDisplay("{TimingNode.GetType().Name}")]
    public class StopwatchTimingNode
    {
        #region Static

        public static StopwatchTimingNode GetList(string description, StopwatchTimingNode parent)
        {
            ListTimingNode list = new ListTimingNode(description);
            if(parent?.TimingNode is ListTimingNode parentList)
            {
                parentList.AddSubNode(list);
            }

            StopwatchTimingNode output = new StopwatchTimingNode(list);
            return output;
        }

        public static StopwatchTimingNode GetList(string description, ITimingNode parent = null)
        {
            StopwatchTimingNode parentStopwatchNode = null;
            if(null != parent)
            {
                parentStopwatchNode = new StopwatchTimingNode(parent);
            }

            StopwatchTimingNode output = StopwatchTimingNode.GetList(description, parentStopwatchNode);
            return output;
        }

        public static StopwatchTimingNode GetLeaf(string description, StopwatchTimingNode parent)
        {
            LeafTimingNode leaf = new LeafTimingNode(description);
            if (parent?.TimingNode is ListTimingNode parentList)
            {
                parentList.AddSubNode(leaf);
            }

            StopwatchTimingNode output = new StopwatchTimingNode(leaf);
            return output;
        }

        public static StopwatchTimingNode GetLeaf(string description, ITimingNode parent = null)
        {
            StopwatchTimingNode parentStopwatchNode = null;
            if (null != parent)
            {
                parentStopwatchNode = new StopwatchTimingNode(parent);
            }

            StopwatchTimingNode output = StopwatchTimingNode.GetLeaf(description, parentStopwatchNode);
            return output;
        }

        #endregion


        private Stopwatch Stopwatch { get; set; }
        public ITimingNode TimingNode { get; }


        private StopwatchTimingNode(ITimingNode timingNode)
        {
            this.TimingNode = timingNode;

            this.Stopwatch = new Stopwatch();
            this.Stopwatch.Start();
        }

        public void Stop()
        {
            this.Stopwatch.Stop();

            this.TimingNode.ElapsedTime = this.Stopwatch.Elapsed;
        }
    }
}
