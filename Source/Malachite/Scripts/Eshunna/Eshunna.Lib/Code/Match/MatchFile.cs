using System;
using System.Collections.Generic;


namespace Eshunna.Lib.Match
{
    public class MatchFile
    {
        public MatchFileHeader Header { get; set; }
        public MatchFileDefinition Definition { get; set; }
        public Dictionary<string, MatchRecordV3> RecordsByFileName { get; set; }
        public Dictionary<string, int[,]> AllMatchesByFileName { get; set; }
        public Dictionary<string, int[,]> InlierMatchesByFileName { get; set; }


        public MatchFile(MatchFileHeader header, MatchFileDefinition definition, Dictionary<string, MatchRecordV3> recordsByFileName, Dictionary<string, int[,]> allMatchesByFileName, Dictionary<string, int[,]> inlierMatchesByFileName)
        {
            this.Header = header;
            this.Definition = definition;
            this.RecordsByFileName = recordsByFileName;
            this.AllMatchesByFileName = allMatchesByFileName;
            this.InlierMatchesByFileName = inlierMatchesByFileName;
        }

        public MatchFile()
        {
            this.RecordsByFileName = new Dictionary<string, MatchRecordV3>();
            this.AllMatchesByFileName = new Dictionary<string, int[,]>();
            this.InlierMatchesByFileName = new Dictionary<string, int[,]>();
        }
    }
}
