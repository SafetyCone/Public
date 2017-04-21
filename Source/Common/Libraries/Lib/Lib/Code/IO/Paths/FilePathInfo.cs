using System;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.IO
{
    public class FilePathInfo
    {
        public string ParentDirectoryPath { get; set; }
        public string FileNameWithoutExtension { get; set; }
        public string FileName
        {
            get
            {
                string output = this.FileNameWithoutExtension + Constants.WindowsFileExtensionSeparatorChar + this.FileExtension;
                return output;
            }
            set
            {
                this.FileNameWithoutExtension = SysPath.GetFileNameWithoutExtension(value);
                this.FileExtension = SysPath.GetExtension(value);
            }
        }
        public string FileExtension { get; set; }
        public string Path
        {
            get
            {
                string output = SysPath.Combine(this.ParentDirectoryPath, this.FileName);
                return output;
            }
            set
            {
                this.FileName = SysPath.GetFileName(value);
                this.ParentDirectoryPath = SysPath.GetDirectoryName(value);
            }
        }


        public FilePathInfo()
        {
        }

        public FilePathInfo(string path)
        {
            this.Path = path;
        }

        public FilePathInfo(string parentDirectoryPath, string fileName)
        {
            this.ParentDirectoryPath = parentDirectoryPath;
            this.FileName = fileName;
        }

        public FilePathInfo(string parentDirectoryPath, string fileNameWithoutExtension, string fileExtension)
        {
            this.ParentDirectoryPath = parentDirectoryPath;
            this.FileNameWithoutExtension = fileNameWithoutExtension;
            this.FileExtension = fileExtension;
        }
    }
}
