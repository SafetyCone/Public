using System;


namespace Public.Common.Lib.Code.Logical
{
    public class FileReferenceCompilationUnit : CompilationUnit
    {
        public FileReference FileReference { get; set; }


        public FileReferenceCompilationUnit()
        {
        }

        public FileReferenceCompilationUnit(FileReference fileReference)
        {
            this.FileReference = fileReference;
        }
    }
}
