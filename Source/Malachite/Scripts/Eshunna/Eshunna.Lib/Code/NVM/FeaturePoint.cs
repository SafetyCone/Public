using System;


namespace Eshunna.Lib.NVM
{
    [Serializable]
    public class FeaturePoint
    {
        public Location3D Location { get; set; }
        public Color Color { get; set; }
        public Measurement[] Measurements { get; set; }


        public FeaturePoint(Location3D location, Color color, Measurement[] measurements)
        {
            this.Location = location;
            this.Color = color;
            this.Measurements = measurements;
        }
    }
}
