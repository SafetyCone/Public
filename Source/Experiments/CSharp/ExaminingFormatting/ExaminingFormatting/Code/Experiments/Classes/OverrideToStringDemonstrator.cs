using System;


namespace ExaminingFormatting
{
    /// <summary>
    /// Shows how to override the <see cref="object.ToString()"/> method.
    /// </summary>
    public class OverrideToStringDemonstrator
    {
        #region Static

        public static OverrideToStringDemonstrator NewExampleInstance()
        {
            var output = new OverrideToStringDemonstrator()
            {
                IntValue = 5,
                StringValue = @"Five",
            };
            return output;
        }

        #endregion


        public int IntValue { get; set; }
        public string StringValue { get; set; }


        public override string ToString()
        {
            var output = $@"IntValue: {this.IntValue.ToString()}, StringValue: '{this.IntValue.ToString()}'";
            return output;
        }
    }
}
