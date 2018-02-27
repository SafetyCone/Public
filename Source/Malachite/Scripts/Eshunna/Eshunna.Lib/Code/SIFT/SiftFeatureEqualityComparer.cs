using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.SIFT
{
    public class SiftFeatureEqualityComparer : IEqualityComparer<SiftFeature>
    {
        public ILog Log { get; }


        public SiftFeatureEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(SiftFeature x, SiftFeature y)
        {
            bool output = true;

            bool xEquals = x.X == y.X;
            if(!xEquals)
            {
                output = false;

                string message = $@"X value mismatch: x: {x.X.ToString()}, y: {y.X.ToString()}";
                this.Log.WriteLine(message);
            }

            bool yEquals = x.Y == y.Y;
            if(!yEquals)
            {
                output = false;

                string message = $@"Y value mismatch: x: {x.Y.ToString()}, y: {y.Y.ToString()}";
                this.Log.WriteLine(message);
            }

            bool scaleEquals = x.Scale == y.Scale;
            if(!scaleEquals)
            {
                output = false;

                string message = $@"Scale mismatch: x: {x.Scale.ToString()}, y: {y.Scale.ToString()}";
                this.Log.WriteLine(message);
            }

            bool orientationEquals = x.Orientation == y.Orientation;
            if(!orientationEquals)
            {
                output = false;

                string message = $@"Orientation mismatch: x: {x.Orientation.ToString()}, y: {y.Orientation.ToString()}";
                this.Log.WriteLine(message);
            }

            bool colorEquals = x.Color == y.Color;
            if(!colorEquals)
            {
                output = false;

                string message = $@"Color mismatch: x: {x.Color.ToString()}, y: {y.Color.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(SiftFeature obj)
        {
            int output = HashHelper.GetHashCode(obj.X, obj.Y, obj.Scale, obj.Orientation);
            return output;
        }
    }
}
