using System;
using System.Collections.Generic;


namespace Eshunna.Lib.Match
{
    public class MatchFileDefinition
    {
        public List<string> FileNamesInOrder { get; set; }
        public Dictionary<string, RecordLocation> RecordLocationsByFileName { get; set; }
        public byte[] Trivia { get; set; }


        public MatchFileDefinition(List<string> fileNamesInOrder, Dictionary<string, RecordLocation> recordLocationsByFileName, byte[] trivia)
        {
            this.FileNamesInOrder = fileNamesInOrder;
            this.RecordLocationsByFileName = recordLocationsByFileName;
            this.Trivia = trivia;
        }

        public MatchFileDefinition()
        {
            this.FileNamesInOrder = new List<string>();
            this.RecordLocationsByFileName = new Dictionary<string, RecordLocation>();
        }
    }
}
