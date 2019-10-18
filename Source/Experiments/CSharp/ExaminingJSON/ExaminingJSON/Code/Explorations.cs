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
            //Explorations.SerializeSimpleObject();
            Explorations.DeserializeSimpleObject();
        }

        private static void DeserializeSimpleObject()
        {
            var jsonSerializer = Program.GetStandardJsonSerializer();

            var simpleObject = jsonSerializer.Deserialize<BasicClass>(Constants.TempJsonFile1Path);
        }

        private static void SerializeSimpleObject()
        {
            var simpleObject = BasicClass.NewExampleInstance();

            var jsonSerializer = Program.GetStandardJsonSerializer();

            jsonSerializer.Serialize(Constants.TempJsonFile1Path, simpleObject);
        }
    }
}
