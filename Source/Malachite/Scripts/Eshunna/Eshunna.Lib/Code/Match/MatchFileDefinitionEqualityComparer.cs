using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class MatchFileDefinitionEqualityComparer : IEqualityComparer<MatchFileDefinition>
    {
        public RecordLocationEqualityComparer RecordLocationComparer { get; }
        public ILog Log { get; }

        
        public MatchFileDefinitionEqualityComparer(RecordLocationEqualityComparer recordLocationComparer, ILog log)
        {
            this.RecordLocationComparer = recordLocationComparer;
            this.Log = log;
        }

        public MatchFileDefinitionEqualityComparer(ILog log)
            : this(new RecordLocationEqualityComparer(log), log)
        {
        }

        public bool Equals(MatchFileDefinition x, MatchFileDefinition y)
        {
            bool output = true;

            bool fileNamesCountEquals = x.FileNamesInOrder.Count == y.FileNamesInOrder.Count;
            if(!fileNamesCountEquals)
            {
                output = false;

                string message = $@"File names count mismatch: x: {x.FileNamesInOrder.Count.ToString()}, y: {y.FileNamesInOrder.Count.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool fileNamesEqual = x.FileNamesInOrder.SequenceEqual(y.FileNamesInOrder);
                if(!fileNamesEqual)
                {
                    output = false;

                    string message = @"File names mismatch.";
                    this.Log.WriteLine(message);
                }
            }

            List<string> keysX = new List<string>(x.RecordLocationsByFileName.Keys);
            List<string> keysY = new List<string>(y.RecordLocationsByFileName.Keys);

            bool keyCountEqual = keysX.Count == keysY.Count;
            if(!keyCountEqual)
            {
                output = false;

                string message = $@"Key count mismatch: x: {keysX.Count.ToString()}, y: {keysY.Count.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool keysEqual = keysX.SequenceEqual(keysY);
                if (!keysEqual)
                {
                    output = false;

                    string message = @"Keys mismatch";
                    this.Log.WriteLine(message);
                }
                else
                {
                    foreach(string fileName in keysX)
                    {
                        RecordLocation recordLocationX = x.RecordLocationsByFileName[fileName];
                        RecordLocation recordLocationY = y.RecordLocationsByFileName[fileName];

                        bool recordLocationsEqual = this.RecordLocationComparer.Equals(recordLocationX, recordLocationY);
                        if(!recordLocationsEqual)
                        {
                            output = false;

                            string message = $@"Record location mismatch for file name: {fileName}";
                            this.Log.WriteLine(message);
                        }
                    }
                }
            }

            bool triviaLengthEquals = x.Trivia.Length == y.Trivia.Length;
            if(!triviaLengthEquals)
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
                    if(!valuesEqual)
                    {
                        output = false;

                        string message = $@"Trivia values mismatch: index: {iByte.ToString()}, x: {valueX.ToString()}, y: {valueY.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(MatchFileDefinition obj)
        {
            int output = obj.GetHashCode(); // Use default.
            return output;
        }
    }
}
