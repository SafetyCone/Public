using System;


namespace Eshunna.Lib.SIFT
{
    public class SiftHeader
    {
        public string Name { get; set; }
        public Version Version { get; set; }
        public int NumberOfFeaturePoints { get; set; }
        public int NumberOfFeatureProperties { get; set; }
        public int NumberOfDescriptorComponents { get; set; }


        public SiftHeader(string name, Version version, int numberOfFeaturePoints, int numberOfLocationProperties, int numberOfDescriptorComponents)
        {
            this.Name = name;
            this.Version = version;
            this.NumberOfFeaturePoints = numberOfFeaturePoints;
            this.NumberOfFeatureProperties = numberOfLocationProperties;
            this.NumberOfDescriptorComponents = numberOfDescriptorComponents;
        }
    }
}
