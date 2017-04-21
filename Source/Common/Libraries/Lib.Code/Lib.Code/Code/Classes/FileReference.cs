using System;


namespace Public.Common.Lib.Code
{
    public class FileReference
    {
        /// <summary>
        /// The pay may be any string, relative path, rooted path, interpretted path, etc.
        /// </summary>
        public string Path { get; set; }


        public FileReference()
        {
        }

        public FileReference(string path)
        {
            this.Path = path;
        }
    }
}
