using System;


namespace Eshunna.Lib.NVM
{
    [Serializable]
    public class Measurement
    {
        public int ImageIndex { get; set; }
        public int FeatureIndex { get; set; }
        /// <summary>
        /// Location relative to the image center.
        /// </summary>
        public Location2D Location { get; set; }


        public Measurement(int imageIndex, int featureIndex, Location2D location)
        {
            this.ImageIndex = imageIndex;
            this.FeatureIndex = featureIndex;
            this.Location = location;
        }
    }
}
