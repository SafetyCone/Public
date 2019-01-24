using System;


namespace ExaminingFormatting
{
    public class BasicClass
    {
        #region Static

        public static BasicClass NewExampleInstance()
        {
            var output = new BasicClass()
            {
                IntValue = 5,
                StringValue = @"Five",
            };
            return output;
        }

        #endregion


        public int IntValue { get; set; }
        public string StringValue { get; set; }
    }
}
