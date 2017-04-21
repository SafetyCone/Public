using System;


namespace Public.Common.Lib.IO.Serialization.Extensions
{
    // Ok.
    public static class SerializationListExtensions
    {
        public const string DefaultCreateDirectoryMoniker = @"DefaultCreateDirectory";
        public const string DefaultFileCopyMoniker = @"DefaultFileCopy";
        public const string DefaultTextFileMoniker = @"DefaultTextFile";


        #region Static

        public static void AddDefaultSerializersByMoniker(SerializationList list)
        {
            list.SerializersByMoniker.Add(SerializationListExtensions.DefaultCreateDirectoryMoniker, new CreateDirectorySerializer());
            list.SerializersByMoniker.Add(SerializationListExtensions.DefaultFileCopyMoniker, new FileCopySerializer());
            list.SerializersByMoniker.Add(SerializationListExtensions.DefaultTextFileMoniker, new TextFileSerializer());
        }

        #endregion


        public static void AddFileCopy(this SerializationList list, FileCopySerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListExtensions.DefaultFileCopyMoniker);
        }

        public static void AddFileCopy(this SerializationList list, string sourcePath, string destinationPath)
        {
            FileCopySerializationUnit unit = new FileCopySerializationUnit(sourcePath, destinationPath);
            list.AddFileCopy(unit);
        }

        public static void AddCreateDirectory(this SerializationList list, CreateDirectorySerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListExtensions.DefaultCreateDirectoryMoniker);
        }

        public static void AddCreateDirectory(this SerializationList list, string directoryPath)
        {
            CreateDirectorySerializationUnit unit = new CreateDirectorySerializationUnit(directoryPath);
            list.AddCreateDirectory(unit);
        }

        public static void AddTextFile(this SerializationList list, TextFileSerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListExtensions.DefaultTextFileMoniker);
        }

        public static void AddTextFile(this SerializationList list, string path, TextFile textFile)
        {
            TextFileSerializationUnit unit = new TextFileSerializationUnit(path, textFile);
            list.AddTextFile(unit);
        }
    }
}
