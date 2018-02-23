using System;


namespace Eshunna.Lib.NVM
{
    /// <summary>
    /// N-View Match.
    /// </summary>
    [Serializable]
    public class NViewMatch
    {
        public Camera[] Cameras { get; set; }
        public FeaturePoint[] FeaturePoints { get; set; }
        public int[] ModelIndicessWithPlyFiles { get; set; }


        public NViewMatch(Camera[] cameras, FeaturePoint[] featurePoints, int[] modelIndicesWithPlyFiles)
        {
            this.Cameras = cameras;
            this.FeaturePoints = featurePoints;
            this.ModelIndicessWithPlyFiles = modelIndicesWithPlyFiles;
        }
    }
}
