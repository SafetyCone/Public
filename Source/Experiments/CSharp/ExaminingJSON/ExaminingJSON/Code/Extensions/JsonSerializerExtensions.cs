using System;
using System.IO;

using Newtonsoft.Json;


namespace ExaminingJSON
{
    public static class JsonSerializerExtensions
    {
        public static void Serialize<T>(this JsonSerializer jsonSerializer, string jsonFilePath, T value)
        {
            using (var textFileWriter = File.CreateText(jsonFilePath))
            {
                jsonSerializer.Serialize(textFileWriter, value);
            }
        }

        public static T Deserialize<T>(this JsonSerializer jsonSerializer, string jsonFilePath)
        {
            using (var textReader = File.OpenText(jsonFilePath))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var value = jsonSerializer.Deserialize<T>(jsonReader);
                return value;
            }
        }
    }
}
