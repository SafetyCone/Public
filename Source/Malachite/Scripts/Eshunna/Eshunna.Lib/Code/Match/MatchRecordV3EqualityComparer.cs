using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class MatchRecordV3EqualityComparer : IEqualityComparer<MatchRecordV3>
    {
        public TwoViewGeometryEqualityComparer TwoViewGeometryComparer { get; }
        public ILog Log { get; }


        public MatchRecordV3EqualityComparer(TwoViewGeometryEqualityComparer twoViewGeometryComparer, ILog log)
        {
            this.TwoViewGeometryComparer = twoViewGeometryComparer;
            this.Log = log;
        }

        public MatchRecordV3EqualityComparer(ILog log)
            : this(new TwoViewGeometryEqualityComparer(log), log)
        {
        }

        public bool Equals(MatchRecordV3 x, MatchRecordV3 y)
        {
            bool output = true;

            bool versionEquals = x.Version == y.Version;
            if(!versionEquals)
            {
                output = false;

                string message = $@"Version mismatch: x: {x.Version}, y: {y.Version}";
                this.Log.WriteLine(message);
            }

            bool twoViewGeometryEquals = this.TwoViewGeometryComparer.Equals(x.TwoViewGeometry, y.TwoViewGeometry);
            if(!twoViewGeometryEquals)
            {
                output = false;

                string message = @"TwoViewGeometry mismatch";
                this.Log.WriteLine(message);
            }

            bool extraLengthEquals = x.Extra.Length == y.Extra.Length;
            if(!extraLengthEquals)
            {
                output = false;

                string message = $@"Extra length mismatch: x: {x.Extra.Length.ToString()}, y: {y.Extra.Length.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool extraEquals = x.Extra.SequenceEqual(y.Extra);
                if(!extraEquals)
                {
                    output = false;

                    string message = @"Extra mismatch";
                    this.Log.WriteLine(message);
                }
            }

            bool triviaLengthEquals = x.Trivia.Length == y.Trivia.Length;
            if (!triviaLengthEquals)
            {
                output = false;

                string message = $@"Trivia length mismatch: x: {x.Trivia.Length.ToString()}, y: {y.Trivia.Length.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iByte = 0; iByte < x.Trivia.Length; iByte++)
                {
                    byte valueX = x.Trivia[iByte];
                    byte valueY = y.Trivia[iByte];

                    bool valuesEqual = valueX == valueY;
                    if (!valuesEqual)
                    {
                        output = false;

                        string message = $@"Trivia values mismatch: index: {iByte.ToString()}, x: {valueX.ToString()}, y: {valueY.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(MatchRecordV3 obj)
        {
            int output = obj.TwoViewGeometry.GetHashCode();
            return output;
        }
    }
}
