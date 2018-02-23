using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;


namespace Eshunna.Lib.NVM
{
    public class FeaturePointComparer : IEqualityComparer<FeaturePoint>
    {
        public bool Equals(FeaturePoint x, FeaturePoint y)
        {
            bool output =
                x.Location == y.Location &&
                x.Color == y.Color &&
                x.Measurements.SequenceEqual(y.Measurements, this.MeasurementComparer);
            return output;
        }

        public int GetHashCode(FeaturePoint obj)
        {
            int output = HashHelper.GetHashCode(obj.Location, obj.Color);
            return output;
        }


        public IEqualityComparer<Measurement> MeasurementComparer { get; }


        public FeaturePointComparer(IEqualityComparer<Measurement> measurementComparer)
        {
            this.MeasurementComparer = measurementComparer;
        }

        public FeaturePointComparer()
            : this(new MeasurementEqualityComparer())
        {
        }
    }
}
