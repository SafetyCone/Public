using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.SIFT
{
    public class SiftFileEqualityComparer : IEqualityComparer<SiftFile>
    {
        public SiftHeaderEqualityComparer HeaderComparer { get; }
        public SiftFeatureEqualityComparer FeatureComparer { get; }
        public SiftDescriptorEqualityComparer DescriptorComparer { get; }
        public ILog Log { get; }


        public SiftFileEqualityComparer(SiftHeaderEqualityComparer headerComparer, SiftFeatureEqualityComparer featureComparer, SiftDescriptorEqualityComparer descriptorComparer, ILog log)
        {
            this.HeaderComparer = headerComparer;
            this.FeatureComparer = featureComparer;
            this.DescriptorComparer = descriptorComparer;
            this.Log = log;
        }

        public SiftFileEqualityComparer(ILog log)
            : this(new SiftHeaderEqualityComparer(log), new SiftFeatureEqualityComparer(log), new SiftDescriptorEqualityComparer(log), log)
        {
        }

        public bool Equals(SiftFile x, SiftFile y)
        {
            bool output = true;

            bool headerEqual = this.HeaderComparer.Equals(x.Header, y.Header);
            if(!headerEqual)
            {
                output = false;

                string message = @"Headers not equal.";
                this.Log.WriteLine(message);
            }

            int nFeaturesX = x.Features.Length;
            int nFeaturesY = y.Features.Length;
            bool featureCountEqual = nFeaturesX == nFeaturesY;
            if(!featureCountEqual)
            {
                output = false;

                string message = $@"Feature count mismatch: x: {nFeaturesX.ToString()}, y: {nFeaturesY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iFeature = 0; iFeature < nFeaturesX; iFeature++)
                {
                    SiftFeature featureX = x.Features[iFeature];
                    SiftFeature featureY = y.Features[iFeature];

                    bool featuresEqual = this.FeatureComparer.Equals(featureX, featureY);
                    if(!featuresEqual)
                    {
                        output = false;

                        string message = $@"Feature mismatch: index: {iFeature.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            int nDescriptorsX = x.Descriptors.Length;
            int nDescriptorsY = y.Descriptors.Length;
            bool descriptorCountEqual = nDescriptorsX == nDescriptorsY;
            if(!descriptorCountEqual)
            {
                output = false;

                string message = $@"Descriptor count mismatch: x: {nDescriptorsX.ToString()}, y: {nDescriptorsY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iDescriptor = 0; iDescriptor < nDescriptorsX; iDescriptor++)
                {
                    SiftDescriptor descriptorX = x.Descriptors[iDescriptor];
                    SiftDescriptor descriptorY = y.Descriptors[iDescriptor];

                    bool descriptorsEqual = this.DescriptorComparer.Equals(descriptorX, descriptorY);
                    if(!descriptorsEqual)
                    {
                        output = false;

                        string message = $@"Descriptor mismatch: index: {iDescriptor.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            bool endByteEqual = x.EndByte == y.EndByte;
            if (!endByteEqual)
            {
                output = false;

                string message = $@"End byte mismatch: x: {x.EndByte.ToString()}, y: {y.EndByte.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(SiftFile obj)
        {
            int output = obj.GetHashCode(); // Use the reference type default.
            return output;
        }
    }
}
