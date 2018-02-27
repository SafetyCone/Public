using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.SIFT
{
    public class SiftHeaderEqualityComparer : IEqualityComparer<SiftHeader>
    {
        public ILog Log { get; }


        public SiftHeaderEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(SiftHeader x, SiftHeader y)
        {
            bool output = true;

            bool nameEquals = x.Name == y.Name;
            if(!nameEquals)
            {
                output = false;

                string message = $@"Name mismatch: x: {x.Name}, y: {y.Name}";
                this.Log.WriteLine(message);
            }

            bool versionEquals = x.Version == y.Version;
            if(!versionEquals)
            {
                output = false;

                string message = $@"Version mismatch: x: {x.Version}, y: {y.Version}";
                this.Log.WriteLine(message);
            }

            bool featureCountEquals = x.NumberOfFeaturePoints == y.NumberOfFeaturePoints;
            if(!featureCountEquals)
            {
                output = false;

                string message = $@"Feature count mismatch: x: {x.NumberOfFeaturePoints.ToString()}, y: {y.NumberOfFeaturePoints.ToString()}";
                this.Log.WriteLine(message);
            }

            bool featurePropertyCountEquals = x.NumberOfFeatureProperties == y.NumberOfFeatureProperties;
            if(!featurePropertyCountEquals)
            {
                output = false;

                string message = $@"Feature property count mismatch: x: {x.NumberOfFeatureProperties.ToString()}, y: {y.NumberOfFeatureProperties.ToString()}";
                this.Log.WriteLine(message);
            }

            bool descriptorComponentCountEquals = x.NumberOfDescriptorComponents == y.NumberOfDescriptorComponents;
            if(!descriptorComponentCountEquals)
            {
                output = false;

                string message = $@"Descriptor component count mismatch: x: {x.NumberOfDescriptorComponents.ToString()}, y: {y.NumberOfDescriptorComponents.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(SiftHeader obj)
        {
            int output = obj.NumberOfFeaturePoints.GetHashCode(); // Just use the number of features on the assumption that all other values will be equal across instances.
            return output;
        }
    }
}
