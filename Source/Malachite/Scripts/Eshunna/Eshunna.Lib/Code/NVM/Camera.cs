using System;


namespace Eshunna.Lib.NVM
{
    [Serializable]
    public class Camera
    {
        public string FileName { get; set; }
        public double FocalLength { get; set; }
        public QuaternionDouble Rotation { get; set; }
        /// <summary>
        /// Location of camera in 3D space.
        /// </summary>
        public Location3Double Location { get; set; }
        public double RadialDistortionCoefficient { get; set; }


        public Camera(string fileName, double focalLength, QuaternionDouble rotation, Location3Double location, double radialDistortionCoefficient)
        {
            this.FileName = fileName;
            this.FocalLength = focalLength;
            this.Rotation = rotation;
            this.Location = location;
            this.RadialDistortionCoefficient = radialDistortionCoefficient;
        }
    }
}
