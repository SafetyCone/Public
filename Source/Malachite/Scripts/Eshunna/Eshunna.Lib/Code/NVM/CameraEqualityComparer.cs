using System;
using System.Collections.Generic;


namespace Eshunna.Lib.NVM
{
    public class CameraEqualityComparer : IEqualityComparer<Camera>
    {
        public bool Equals(Camera x, Camera y)
        {
            bool output =
                x.FileName == y.FileName &&
                x.FocalLength == y.FocalLength &&
                x.Rotation == y.Rotation &&
                x.Location == y.Location &&
                x.RadialDistortionCoefficient == y.RadialDistortionCoefficient;
            return output;
        }

        public int GetHashCode(Camera obj)
        {
            int output = obj.FileName.GetHashCode();
            return output;
        }
    }
}
