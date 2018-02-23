using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Patches
{
    public class PatchEqualityComparer : IEqualityComparer<Patch>
    {
        public ILog Log { get; }


        public PatchEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(Patch x, Patch y)
        {
            bool output = true;

            bool locationsEqual = x.Location == y.Location;
            if(!locationsEqual)
            {
                output = false;

                string message = $@"Locations not equal - x: {x.Location.ToString()}, y: {y.Location.ToString()}";
                this.Log.WriteLine(message);
            }

            bool normalsEqual = x.Normal == y.Normal;
            if(!normalsEqual)
            {
                output = false;

                string message = $@"Normals not equal - x: {x.Normal.ToString()}, y: {y.Normal.ToString()}";
                this.Log.WriteLine(message);
            }

            bool photogrammetricScoresEqual = x.PhotometricConsistencyScore == y.PhotometricConsistencyScore;
            if(!photogrammetricScoresEqual)
            {
                output = false;

                string message = $@"Photogrammetric consistency scores not equal - x: {x.PhotometricConsistencyScore.FormatPatch6SignificantDigits()}, y: {y.PhotometricConsistencyScore.FormatPatch6SignificantDigits()}";
                this.Log.WriteLine(message);
            }

            bool debbuggingValuesEqual = (x.Debugging1 == y.Debugging1) && (x.Debugging2 == y.Debugging2);
            if(!debbuggingValuesEqual)
            {
                output = false;

                string message = $@"Debugging values not equal - x: ({x.Debugging1.FormatPatch6SignificantDigits()}, {x.Debugging2.FormatPatch6SignificantDigits()}), y: ({y.Debugging1.FormatNvm12SignificantDigits()}, {y.Debugging2.FormatNvm12SignificantDigits()})";
                this.Log.WriteLine(message);
            }

            bool goodMatchesCountEqual = x.ImageIndicesWithGoodAgreement.Length == y.ImageIndicesWithGoodAgreement.Length;
            if(!goodMatchesCountEqual)
            {
                output = false;

                string message = $@"Good matches count not equal- x: {x.ImageIndicesWithGoodAgreement.Length.ToString()}, y: {y.ImageIndicesWithGoodAgreement.Length.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool goodMatchesEqual = x.ImageIndicesWithGoodAgreement.SequenceEqual(y.ImageIndicesWithGoodAgreement);
                if (!goodMatchesEqual)
                {
                    output = false;

                    string message = @"Good matches not equal.";
                    this.Log.WriteLine(message);
                }
            }

            bool someMatchesCountEqual = x.ImageIndicesWithSomeAgreement.Length == y.ImageIndicesWithSomeAgreement.Length;
            if (!someMatchesCountEqual)
            {
                output = false;

                string message = $@"Some matches count not equal- x: {x.ImageIndicesWithSomeAgreement.Length.ToString()}, y: {y.ImageIndicesWithSomeAgreement.Length.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                bool someMatchesEqual = x.ImageIndicesWithSomeAgreement.SequenceEqual(y.ImageIndicesWithSomeAgreement);
                if (!someMatchesEqual)
                {
                    output = false;

                    string message = @"Some matches not equal.";
                    this.Log.WriteLine(message);
                }
            }

            return output;
        }

        public int GetHashCode(Patch obj)
        {
            int output = HashHelper.GetHashCode(obj.Location, obj.Normal);
            return output;
        }
    }
}
