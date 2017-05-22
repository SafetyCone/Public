using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Physical;


namespace Public.Common.Avon.Lib
{
    public class VisualStudioVersionedFilePathSet
    {
        public string DirectoryPath { get; set; }
        public Dictionary<VisualStudioVersion, string> FilePathsByVisualStudioVersion { get; protected set; }


        public VisualStudioVersionedFilePathSet()
        {
            this.FilePathsByVisualStudioVersion = new Dictionary<VisualStudioVersion, string>();
        }

        public VisualStudioVersionedFilePathSet(string directoryPath)
            : this()
        {
            this.DirectoryPath = directoryPath;
        }
    }
}
