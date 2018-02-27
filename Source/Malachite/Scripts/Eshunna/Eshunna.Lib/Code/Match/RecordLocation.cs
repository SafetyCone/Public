using System;


namespace Eshunna.Lib.Match
{
    public class RecordLocation
    {
        public int FeatureCount { get; set; }
        public int ReadLocation { get; set; }
        public int BlockSize { get; set; }
        public int TrashSize { get; set; }
        public int ExtraSize { get; set; }
        public string FileName { get; set; }


        public RecordLocation(int featureCount, int readLocation, int blockSize, int trashSize, int extraSize, string fileName)
        {
            this.FeatureCount = featureCount;
            this.ReadLocation = readLocation;
            this.BlockSize = blockSize;
            this.TrashSize = trashSize;
            this.ExtraSize = extraSize;
            this.FileName = fileName;
        }

        public RecordLocation()
        {
        }
    }
}
