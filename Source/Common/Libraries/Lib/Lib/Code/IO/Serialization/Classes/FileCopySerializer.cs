using System;
using System.IO;


namespace Public.Common.Lib.IO.Serialization
{
    public class FileCopySerializer : SerializerBase<FileCopySerializationUnit>
    {
        public bool CreateDirectoriesIfNeeded { get; set; }
        public bool Overwrite { get; set; }


        public FileCopySerializer()
        {
            // Set defaults.
            this.CreateDirectoriesIfNeeded = true;
            this.Overwrite = true;
        }

        protected override void Serialize(FileCopySerializationUnit unit)
        {
            string destinationFilePath = unit.Path;
            string destinationDirectoryPath = Path.GetDirectoryName(destinationFilePath);
            if(!Directory.Exists(destinationDirectoryPath))
            {
                if(this.CreateDirectoriesIfNeeded)
                {
                    Directory.CreateDirectory(destinationDirectoryPath);
                }
            }

            File.Copy(unit.SourcePath, destinationFilePath, this.Overwrite);
        }
    }
}
