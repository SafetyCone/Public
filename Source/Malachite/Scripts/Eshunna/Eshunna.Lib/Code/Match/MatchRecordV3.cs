using System;


namespace Eshunna.Lib.Match
{
    public class MatchRecordV3
    {
        public const int DefaultNumberOfExtraInts = 7;


        public string Version { get; set; }
        public TwoViewGeometry TwoViewGeometry { get; set; }
        public int[] Extra { get; set; }
        public byte[] Trivia { get; set; }


        public MatchRecordV3(string version, TwoViewGeometry twoViewGeometry, int[] extra, byte[] trivia)
        {
            this.Version = version;
            this.TwoViewGeometry = twoViewGeometry;
            this.Extra = extra;
            this.Trivia = trivia;
        }
    }
}
