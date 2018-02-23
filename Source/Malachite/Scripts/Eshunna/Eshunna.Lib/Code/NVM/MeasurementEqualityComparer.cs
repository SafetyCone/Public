using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Eshunna.Lib.NVM
{
    public class MeasurementEqualityComparer : IEqualityComparer<Measurement>
    {
        public bool Equals(Measurement x, Measurement y)
        {
            bool output =
                x.ImageIndex == y.ImageIndex &&
                x.FeatureIndex == y.FeatureIndex &&
                x.Location == y.Location;
            return output;
        }

        public int GetHashCode(Measurement obj)
        {
            int output = HashHelper.GetHashCode(obj.ImageIndex, obj.Location);
            return output;
        }
    }
}
