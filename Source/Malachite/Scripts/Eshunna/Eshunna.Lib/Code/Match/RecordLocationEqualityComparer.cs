using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class RecordLocationEqualityComparer : IEqualityComparer<RecordLocation>
    {
        public ILog Log { get; }

        
        public RecordLocationEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(RecordLocation x, RecordLocation y)
        {
            bool output = true;

            bool featureCountEquals = x.FeatureCount == y.FeatureCount;
            if (!featureCountEquals)
            {
                output = false;

                string message = $@"FeatureCount mismatch: x: {x.FeatureCount.ToString()}, y: {y.FeatureCount.ToString()}";
                this.Log.WriteLine(message);
            }

            bool readLocationEquals = x.ReadLocation == y.ReadLocation;
            if (!readLocationEquals)
            {
                output = false;

                string message = $@"ReadLocation mismatch: x: {x.ReadLocation.ToString()}, y: {y.ReadLocation.ToString()}";
                this.Log.WriteLine(message);
            }

            bool blockSizeEquals = x.BlockSize == y.BlockSize;
            if (!blockSizeEquals)
            {
                output = false;

                string message = $@"BlockSize mismatch: x: {x.BlockSize.ToString()}, y: {y.BlockSize.ToString()}";
                this.Log.WriteLine(message);
            }

            bool trashSizeEquals = x.TrashSize == y.TrashSize;
            if (!trashSizeEquals)
            {
                output = false;

                string message = $@"TrashSize mismatch: x: {x.TrashSize.ToString()}, y: {y.TrashSize.ToString()}";
                this.Log.WriteLine(message);
            }

            bool extraSizeEquals = x.ExtraSize == y.ExtraSize;
            if (!extraSizeEquals)
            {
                output = false;

                string message = $@"ExtraSize mismatch: x: {x.ExtraSize.ToString()}, y: {y.ExtraSize.ToString()}";
                this.Log.WriteLine(message);
            }

            bool fileNameEquals = x.FileName == y.FileName;
            if (!fileNameEquals)
            {
                output = false;

                string message = $@"FileName mismatch: x: {x.FileName}, y: {y.FileName}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(RecordLocation obj)
        {
            int output = HashHelper.GetHashCode(obj.ReadLocation, obj.BlockSize);
            return output;
        }
    }
}
