using System;

using ExaminingClasses.Lib;


namespace ExaminingFormatting
{
    public static class CustomFormattingExperiments
    {
        public static void SubMain()
        {
            //CustomFormattingExperiments.ExploreIFormatProviderGetFormatInput();
            CustomFormattingExperiments.ExploreCustomFormatter();
        }

        /// <summary>
        /// Example custom formatter.
        /// </summary>
        private static void ExploreCustomFormatter()
        {
            var instance = BasicClass.NewExampleInstance();

            var formatProvider1 = new FormatProviderGetFormatFormatTypeExplorer()
            {
                ExplorerFunction = (formatType) =>
                {
                    var customFormatter = new CustomFormatterFormatExplorer()
                    {
                        ExplorerFunction = (format, arg, formatProvider2) =>
                        {
                            Console.WriteLine($@"format: {format}"); // yyy
                            Console.WriteLine($@"arg: {arg}"); // ExaminingFormatting.BasicClass
                            Console.WriteLine($@"formatProvider: {formatProvider2}"); // ExaminingFormatting.FormatProviderGetFormatFormatTypeExplorer

                            if (arg is BasicClass basicClassInstance)
                            {
                                var lengthOfFormat = format?.Length ?? 0;
                                if (lengthOfFormat < 1)
                                {
                                    return @"YOLO!";
                                }

                                if (lengthOfFormat < 2)
                                {
                                    var intValueOnlyOutput = $@"IntValue: {basicClassInstance.IntValue}";
                                    return intValueOnlyOutput;
                                }

                                var output = $@"IntValue: {basicClassInstance.IntValue}, StringValue: {basicClassInstance.StringValue}";
                                return output;
                            }
                            else
                            {
                                var output = $@"WARNING! Wrong type. Expected: {typeof(BasicClass).FullName}, found: {arg.GetType().FullName}";
                                return output;
                            }
                        },
                    };

                    return customFormatter;
                },
            };

            // Call to actually call the format provider.
            var outputString1 = String.Format(formatProvider1, @"{0:} Hello World!", instance);
            Console.WriteLine($@"Output string1: '{outputString1}'");

            var outputString2 = String.Format(formatProvider1, @"{0:y} Hello World!", instance);
            Console.WriteLine($@"Output string2: '{outputString2}'");

            var outputString3 = String.Format(formatProvider1, @"{0:yy} Hello World!", instance);
            Console.WriteLine($@"Output string3: '{outputString3}'");

            var outputString4 = String.Format(formatProvider1, @"{0:anything!} Hello World!", 4);
            Console.WriteLine($@"Output string4: '{outputString4}'");
        }

        /// <summary>
        /// Result: True, the formatType instance of the <see cref="Type"/> class will describe the <see cref="ICustomFormatter"/> interface.
        /// What IS the type described by the <see cref="Type"/> instance provided to the <see cref="IFormatProvider.GetFormat(Type)"/> method by the .NET framework?
        /// Expectation: The formatType instance of the <see cref="Type"/> class will describe the <see cref="ICustomFormatter"/> interface.
        /// 
        /// As stated here, the <see cref="Type"/> instance provided to the <see cref="IFormatProvider.GetFormat(Type)"/> method describes the <see cref="ICustomFormatter"/> interface. https://docs.microsoft.com/en-us/dotnet/standard/base-types/formatting-types#custom-formatting-with-icustomformatter
        /// </summary>
        public static void ExploreIFormatProviderGetFormatInput()
        {
            var instance = BasicClass.NewExampleInstance();

            var formatProvider = new FormatProviderGetFormatFormatTypeExplorer()
            {
                ExplorerFunction = (formatType) =>
                {
                    // What is this formatType?
                    Console.WriteLine(formatType); // System.ICustomFormatter

                    var customFormatterInterfaceType = typeof(ICustomFormatter);

                    var isEquals = customFormatterInterfaceType == formatType;
                    Console.WriteLine($@"Input formatType equals typeof(ICustomFormatter): {isEquals}"); // True.

                    return null;
                }
            };

            // Call to actually call the format provider.
            var str = String.Format(formatProvider, @"Hello World", instance);
        }
    }
}
