using System;
using System.IO;

using Newtonsoft.Json;

using ExaminingClasses.Lib;


namespace ExaminingJSON
{
    public static class Explorations
    {
        public static void SubMain()
        {
            Explorations.SerializeSimpleObject();
        }

        private static void SerializeSimpleObject()
        {
            var simpleObject = BasicClass.NewExampleInstance();

            var jsonSerializer = Program.GetStandardJsonSerializer();

            using (var textFileWriter = File.CreateText(Constants.TempJsonFile1Path))
            {
                jsonSerializer.Serialize(textFileWriter, simpleObject);
            }
        }
    }
}
