using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib
{
    public class TimingNodeTextSerializer : IFileSerializer<ITimingNode>
    {
        #region Static

        public static void Serialize(string filePath, ITimingNode timingNode, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if(!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fileStream = new FileStream(filePath, fileMode))
            using (TextWriter writer = new StreamWriter(fileStream))
            {
                TimingNodeTextSerializer.Serialize(writer, timingNode);
            }
        }

        public static void Serialize(TextWriter writer, ITimingNode timingNode)
        {
            if(timingNode is LeafTimingNode leaf)
            {
                TimingNodeTextSerializer.Serialize(writer, leaf);
            }
            else
            {
                if (timingNode is ListTimingNode list)
                {
                    TimingNodeTextSerializer.Serialize(writer, list);
                }
                else
                {
                    throw new ArgumentException(nameof(timingNode), $@"Unrecognized timing node type: {timingNode.GetType().FullName}.");
                }
            }
        }

        private static void Serialize(TextWriter writer, ListTimingNode list)
        {
            string startLine = $@"START - {list.Description}";
            writer.WriteLine(startLine);

            TimeSpan subNodesTotalElapsedTime = new TimeSpan();
            foreach(ITimingNode subNode in list.SubNodes)
            {
                subNodesTotalElapsedTime += subNode.ElapsedTime;
            }

            TimeSpan nodeOnlyElapsedTime = list.ElapsedTime - subNodesTotalElapsedTime;
            string nodeOnlyElapsedTimeLine = String.Format(@"{0} - Node", nodeOnlyElapsedTime.ToStringHHMMSSFFF());
            writer.WriteLine(nodeOnlyElapsedTimeLine);

            string subNodesTotalElapsedTimeLine = String.Format(@"{0} - Sub-Nodes", subNodesTotalElapsedTime.ToStringHHMMSSFFF());
            writer.WriteLine(subNodesTotalElapsedTimeLine);

            string totalElapsedTimeLine = String.Format(@"{0} - Total", list.ElapsedTime.ToStringHHMMSSFFF());
            writer.WriteLine(totalElapsedTimeLine);

            writer.WriteLine(@"SUBNODES:");
            foreach(ITimingNode subNode in list.SubNodes)
            {
                TimingNodeTextSerializer.Serialize(writer, subNode);
            }

            string endLine = $@"END - {list.Description}";
            writer.WriteLine(endLine);
        }

        private static void Serialize(TextWriter writer, LeafTimingNode leaf)
        {
            TimingNodeTextSerializer.SerializeNode(writer, leaf);
        }

        private static void SerializeNode(TextWriter writer, ITimingNode timingNode)
        {
            string line = String.Format(@"{0} - {1}", timingNode.ElapsedTime.ToStringHHMMSSFFF(), timingNode.Description);
            writer.WriteLine(line);
        }

        #endregion


        public ITimingNode this[string filePath, bool overwrite = true]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                TimingNodeTextSerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
