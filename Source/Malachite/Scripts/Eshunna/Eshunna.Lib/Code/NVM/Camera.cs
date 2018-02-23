using System;


namespace Eshunna.Lib.NVM
{
    [Serializable]
    public class Camera
    {
        public string FileName { get; set; }
        public double FocalLength { get; set; }
        public Quaternion Rotation { get; set; }
        /// <summary>
        /// ? Location of camera in 3D space, or principal point of the camera?
        /// </summary>
        public Location3D Location { get; set; }
        public double RadialDistortionCoefficient { get; set; }


        public Camera(string fileName, double focalLength, Quaternion rotation, Location3D location, double radialDistortionCoefficient)
        {
            this.FileName = fileName;
            this.FocalLength = focalLength;
            this.Rotation = rotation;
            this.Location = location;
            this.RadialDistortionCoefficient = radialDistortionCoefficient;
        }
    }
}
