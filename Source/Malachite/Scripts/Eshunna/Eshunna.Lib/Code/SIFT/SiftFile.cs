using System;


namespace Eshunna.Lib.SIFT
{
    public class SiftFile
    {
        public const int DefaultDescriptorComponentNumber = SiftDescriptor.DefaultDescriptorComponentNumber;
        public const int HeaderPropertyNumber = 5;
        public const string DefaultSiftName = SiftFile.SiftFileMarker;
        public const string EndOfFileMarker = @"EOF";
        public const string SiftFileMarker = @"SIFT";
        public const string VersionMarker = @"V";


        public SiftHeader Header { get; set; }
        public SiftFeature[] Features { get; set; }
        public SiftDescriptor[] Descriptors { get; set; }
        public byte EndByte { get; set; }


        public SiftFile(SiftHeader header, SiftFeature[] features, SiftDescriptor[] descriptors)
        {
            this.Header = header;
            this.Features = features;
            this.Descriptors = descriptors;
        }
    }
}
