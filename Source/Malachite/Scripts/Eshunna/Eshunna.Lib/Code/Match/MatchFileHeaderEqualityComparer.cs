using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class MatchFileHeaderEqualityComparer : IEqualityComparer<MatchFileHeader>
    {
        public ILog Log { get; }


        public MatchFileHeaderEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(MatchFileHeader x, MatchFileHeader y)
        {
            bool output = true;

            bool versionEquals = x.Version == y.Version;
            if(!versionEquals)
            {
                output = false;

                string message = $@"Version mismatch: x: {x.Version}, y: {y.Version}";
                this.Log.WriteLine(message);
            }

            bool fileCountEquals = x.FileCount == y.FileCount;
            if (!fileCountEquals)
            {
                output = false;

                string message = $@"FileCount mismatch: x: {x.FileCount.ToString()}, y: {y.FileCount.ToString()}";
                this.Log.WriteLine(message);
            }

            bool definiitionSizeEquals = x.DefinitionSize == y.DefinitionSize;
            if (!definiitionSizeEquals)
            {
                output = false;

                string message = $@"DefinitionSize mismatch: x: {x.DefinitionSize.ToString()}, y: {y.DefinitionSize.ToString()}";
                this.Log.WriteLine(message);
            }

            bool definiitionBufferSizeEquals = x.DefinitionBufferSize == y.DefinitionBufferSize;
            if (!definiitionBufferSizeEquals)
            {
                output = false;

                string message = $@"DefinitionBufferSize mismatch: x: {x.DefinitionBufferSize.ToString()}, y: {y.DefinitionBufferSize.ToString()}";
                this.Log.WriteLine(message);
            }

            bool featureCountEquals = x.FeatureCount == y.FeatureCount;
            if (!featureCountEquals)
            {
                output = false;

                string message = $@"FeatureCount mismatch: x: {x.FeatureCount.ToString()}, y: {y.FeatureCount.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(MatchFileHeader obj)
        {
            int output = HashHelper.GetHashCode(obj.FileCount, obj.DefinitionSize, obj.DefinitionBufferSize);
            return output;
        }
    }
}
