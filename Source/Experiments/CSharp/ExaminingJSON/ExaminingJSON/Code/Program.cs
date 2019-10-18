using System;

using Newtonsoft.Json;


namespace ExaminingJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Explorations.SubMain();
        }

        public static JsonSerializer GetStandardJsonSerializer()
        {
            var jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            return jsonSerializer;
        }
    }
}
