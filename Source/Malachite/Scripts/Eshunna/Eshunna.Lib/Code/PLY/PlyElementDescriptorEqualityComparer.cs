using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyElementDescriptorEqualityComparer : IEqualityComparer<PlyElementDescriptor>
    {
        public ILog Log { get; }


        public PlyElementDescriptorEqualityComparer(ILog log)
        {
            this.Log = log;
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

            }

            //return output;
        }

        public int GetHashCode(PlyElementDescriptor obj)
        {
            int output = HashHelper.GetHashCode(obj.Name, obj.Count, obj.PropertyDescriptors.Count); // Ignore the content of the property descriptors for now.
            return output;
        }
    }
}
