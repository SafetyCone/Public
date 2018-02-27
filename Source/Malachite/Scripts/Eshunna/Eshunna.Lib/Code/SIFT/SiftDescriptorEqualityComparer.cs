using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.SIFT
{
    public class SiftDescriptorEqualityComparer : IEqualityComparer<SiftDescriptor>
    {
        public ILog Log { get; }

        
        public SiftDescriptorEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(SiftDescriptor x, SiftDescriptor y)
        {
            bool output = true;

            int nComponentsX = x.Components.Length;
            int nComponentsY = y.Components.Length;
            bool componentCountEquals = nComponentsX == nComponentsY;
            if(!componentCountEquals)
            {
                output = false;

                string message = $@"Component count mismatch: x: {nComponentsX}, y: {nComponentsY}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iComponent = 0; iComponent < nComponentsX; iComponent++)
                {
                    byte valueX = x.Components[iComponent];
                    byte valueY = y.Components[iComponent];
                    bool valuesEqual = valueX == valueY;
                    if(!valuesEqual)
                    {
                        output = false;

                        string message = $@"Values mismatch: index: {iComponent}.";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(SiftDescriptor obj)
        {
            byte c1 = obj.Components[0];
            byte c2 = obj.Components[1];
            byte c3 = obj.Components[2];

            int output = HashHelper.GetHashCode(c1, c2, c3);
            return output;
        }
    }
}
