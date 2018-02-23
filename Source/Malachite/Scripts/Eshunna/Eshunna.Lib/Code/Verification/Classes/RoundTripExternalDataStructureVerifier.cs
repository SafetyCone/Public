using System;
using System.Collections.Generic;

using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.Verification
{
    public class RoundTripExternalDataStructureVerifier
    {
        #region Static

        public static bool Verify<T>(IFileSerializer<T> serializer, string sourceFilePath, string serializedFilePath, IEqualityComparer<T> equalityComparer)
        {
            T externalDataStructure = serializer.Deserialize(sourceFilePath);

            serializer.Serialize(serializedFilePath, externalDataStructure);

            T reserializedExternalDataStructure = serializer.Deserialize(serializedFilePath);

            bool output = equalityComparer.Equals(externalDataStructure, reserializedExternalDataStructure);
            return output;
        }

        #endregion
    }
}
