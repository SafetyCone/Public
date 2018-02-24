using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyElementDescriptorEqualityComparer : IEqualityComparer<PlyElementDescriptor>
    {
        public PlyPropertyDescriptorEqualityComparer PropertyComparer { get; }
        public ILog Log { get; }


        public PlyElementDescriptorEqualityComparer(PlyPropertyDescriptorEqualityComparer propertyComparer, ILog log)
        {
            this.PropertyComparer = propertyComparer;
            this.Log = log;
        }

        public PlyElementDescriptorEqualityComparer(ILog log)
            : this(new PlyPropertyDescriptorEqualityComparer(log), log)
        {
        }

        public bool Equals(PlyElementDescriptor x, PlyElementDescriptor y)
        {
            bool output = true;

            bool elementNameEquals = x.Name == y.Name;
            if(!elementNameEquals)
            {
                output = false;

                string message = $@"Element names not equal - x: {x.Name}, y: {y.Name}";
                this.Log.WriteLine(message);
            }

            bool countEquals = x.Count == y.Count;
            if(!countEquals)
            {
                output = false;

                string message = $@"Element instance counts were not equal - x: {x.Count.ToString()}, y: {y.Count.ToString()}";
                this.Log.WriteLine(message);
            }

            int nPropertiesX = x.PropertyDescriptors.Count;
            int nPropertiesY = y.PropertyDescriptors.Count;
            bool propertyCountEquals = nPropertiesX == nPropertiesY;
            if(!propertyCountEquals)
            {
                output = false;

                string message = $@"Element property count mismatch: x: {nPropertiesX.ToString()}, y: {nPropertiesY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iProperty = 0; iProperty < nPropertiesX; iProperty++)
                {
                    PlyPropertyDescriptor propertyX = x.PropertyDescriptors[iProperty];
                    PlyPropertyDescriptor propertyY = y.PropertyDescriptors[iProperty];

                    bool propertiesEqual = this.PropertyComparer.Equals(propertyX, propertyY);
                    if(!propertiesEqual)
                    {
                        output = false;

                        string message = $@"Property mismatch: index: {iProperty.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(PlyElementDescriptor obj)
        {
            int output = HashHelper.GetHashCode(obj.Name, obj.Count, obj.PropertyDescriptors.Count); // Ignore the content of the property descriptors for now.
            return output;
        }
    }
}
