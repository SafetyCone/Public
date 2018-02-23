using System;

using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.Verification
{
    public class RoundTripExternalFileFormat
    {
        #region Static

        public static bool Verify<T>(IFileSerializer<T> serializer, string sourceFilePath, string serializedFilePath, IFileComparer fileComparer)
        {
            T externalDataStructure = serializer.Deserialize(sourceFilePath);

            serializer.Serialize(serializedFilePath, externalDataStructure);

            bool output = fileComparer.Equals(sourceFilePath, serializedFilePath);
            return output;
        }

        #endregion
    }
}
