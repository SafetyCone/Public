using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Comparers;
using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class MatchFileEqualityComparer : IEqualityComparer<MatchFile>
    {
        public MatchFileHeaderEqualityComparer HeaderComparer { get; }
        public MatchFileDefinitionEqualityComparer DefinitionComparer { get; }
        public MatchRecordV3EqualityComparer RecordComparer { get; }
        public DictionaryEqualityComparer<string, MatchRecordV3> RecordsComparer { get; }
        public DictionaryEqualityComparer<string, int[,]> MatchesComparer { get; }
        public ILog Log { get; }


        public MatchFileEqualityComparer(MatchFileHeaderEqualityComparer headerComparer, MatchFileDefinitionEqualityComparer definitionComparer, MatchRecordV3EqualityComparer recordComparer, ILog log)
        {
            this.HeaderComparer = headerComparer;
            this.DefinitionComparer = definitionComparer;
            this.RecordComparer = recordComparer;
            this.RecordsComparer = new DictionaryEqualityComparer<string, MatchRecordV3>(new StringEqualityComparer(log), this.RecordComparer, log);
            this.MatchesComparer = new DictionaryEqualityComparer<string, int[,]>(new StringEqualityComparer(log), new Array2DEqualityComparer<int>(new IntegerEqualityComparer(log), log), log);
            this.Log = log;
        }

        public MatchFileEqualityComparer(ILog log)
            : this(new MatchFileHeaderEqualityComparer(log), new MatchFileDefinitionEqualityComparer(log), new MatchRecordV3EqualityComparer(log), log)
        {
        }

        public bool Equals(MatchFile x, MatchFile y)
        {
            bool output = true;

            bool headerEquals = this.HeaderComparer.Equals(x.Header, y.Header);
            if(!headerEquals)
            {
                output = false;

                string message = @"Header mismatch";
                this.Log.WriteLine(message);
            }

            bool definitionEquals = this.DefinitionComparer.Equals(x.Definition, y.Definition);
            if(!definitionEquals)
            {
                output = false;

                string message = @"Definition mismatch";
                this.Log.WriteLine(message);
            }

            bool recordsEquals = this.RecordsComparer.Equals(x.RecordsByFileName, y.RecordsByFileName);
            if (!recordsEquals)
            {
                output = false;

                string message = @"Records mismatch";
                this.Log.WriteLine(message);
            }

            bool allMatchesEquals = this.MatchesComparer.Equals(x.AllMatchesByFileName, y.AllMatchesByFileName);
            if (!allMatchesEquals)
            {
                output = false;

                string message = @"All matches mismatch";
                this.Log.WriteLine(message);
            }

            bool inlierMatchesEquals = this.MatchesComparer.Equals(x.InlierMatchesByFileName, y.InlierMatchesByFileName);
            if (!inlierMatchesEquals)
            {
                output = false;

                string message = @"Inlier matches mismatch";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(MatchFile obj)
        {
            int output = obj.GetHashCode(); // Use the default.
            return output;
        }
    }
}
