using System;


namespace Eshunna.Lib.NVM
{
    [Serializable]
    public class FeaturePoint
    {
        public Location3Double Location { get; set; }
        public Color Color { get; set; }
        public Measurement[] Measurements { get; set; }


        public FeaturePoint(Location3Double location, Color color, Measurement[] measurements)
        {
            this.Location = location;
            this.Color = color;
            this.Measurements = measurements;
        }
    }
}
