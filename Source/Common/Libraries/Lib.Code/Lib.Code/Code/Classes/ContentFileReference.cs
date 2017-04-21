using System;


namespace Public.Common.Lib.Code
{
    public class ContentFileReference : FileReference
    {
        public CopyToOutputDirectory CopyToOutputDirectory { get; set; }


        public ContentFileReference()
        {
        }

        public ContentFileReference(string path)
            : base(path)
        {
        }
    }
}
