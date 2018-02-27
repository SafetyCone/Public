using System;


namespace Eshunna.Lib.SIFT
{
    public class SiftDescriptor
    {
        public const int DefaultDescriptorComponentNumber = 128;


        public byte[] Components { get; set; }


        public SiftDescriptor(byte[] components)
        {
            this.Components = components;
        }
    }
}
