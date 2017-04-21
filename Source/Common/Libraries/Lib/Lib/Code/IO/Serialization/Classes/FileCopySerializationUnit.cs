using System;


namespace Public.Common.Lib.IO.Serialization
{
    public class FileCopySerializationUnit : SerializationUnitBase
    {
        public string SourcePath { get; set; }


        public FileCopySerializationUnit(string destinationPath, string sourcePath)
            : base(destinationPath)
        {
            this.SourcePath = sourcePath;
        }
    }
}
