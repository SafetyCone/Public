using System;
using Augustus.Extensions;


namespace Augustus
{
    public class BuildItemSpecification
    {
        public const char DefaultTokenSeparator = '|';


        #region Static

        public static BuildItemSpecification Parse(string buildItemSpecification)
        {
            var output = BuildItemSpecification.Parse(buildItemSpecification, BuildItemSpecification.DefaultTokenSeparator);
            return output;
        }

        public static BuildItemSpecification Parse(string buildItemSpecification, char tokenSeparator)
        {
            string buildFilePath;
            Platform platform;
            Architecture architecture;
            BuildItemSpecification.Parse(buildItemSpecification, tokenSeparator, out buildFilePath, out platform, out architecture);

            var output = new BuildItemSpecification(buildFilePath, platform, architecture);
            return output;
        }

        private static void Parse(string buildItemSpecification, char tokenSeparator, out string buildFilePath, out Platform platform, out Architecture architecture)
        {
            string[] tokens = buildItemSpecification.Split(tokenSeparator);
            if (2 > tokens.Length)
            {
                var message = String.Format(@"Missing build file path or platform for build item specification: '{0}'.", buildItemSpecification);
                throw new ArgumentException(message, nameof(buildItemSpecification));
            }

            buildFilePath = BuildItemSpecification.GetBuildFilePathToken(tokens);

            var platformToken = BuildItemSpecification.GetPlatformToken(tokens);
            platform = PlatformExtensions.FromDefault(platformToken);

            architecture = Architecture.Default;
            if (2 < tokens.Length)
            {
                string architectureToken = BuildItemSpecification.GetArchitectureToken(tokens);
                architecture = ArchitectureExtensions.FromDefault(architectureToken);

                if (Platform.Cygwin == platform && Architecture.Default != architecture)
                {
                    var message = String.Format(@"Specification of architecture other than {0} not allowed for platform {1}.", Architecture.Default.ToDefaultString(), platform.ToDefaultString());
                    throw new ArgumentException(message, nameof(buildItemSpecification));
                }
            }
        }

        private static string GetBuildFilePathToken(string[] tokens)
        {
            var output = tokens[0];
            return output;
        }

        private static string GetPlatformToken(string[] tokens)
        {
            var output = tokens[1];
            return output;
        }

        private static string GetArchitectureToken(string[] tokens)
        {
            var output = tokens[2];
            return output;
        }

        #endregion


        public char TokenSeparator { get; set; }
        public string OriginalString { get; private set; }
        public string BuildFilePath { get; private set; }
        public Platform Platform { get; private set; }
        public Architecture Architecture { get; private set; }


        public BuildItemSpecification()
        {
            this.TokenSeparator = BuildItemSpecification.DefaultTokenSeparator;
        }

        public BuildItemSpecification(string buildFilePath, Platform platform, Architecture architecture)
            : this()
        {
            this.BuildFilePath = buildFilePath;
            this.Platform = platform;
            this.Architecture = architecture;
        }

        public BuildItemSpecification(string buildItemSpecification)
            : this()
        {
            string buildFilePath;
            Platform platform;
            Architecture architecture;
            BuildItemSpecification.Parse(buildItemSpecification, this.TokenSeparator, out buildFilePath, out platform, out architecture);

            this.BuildFilePath = buildFilePath;
            this.Platform = platform;
            this.Architecture = architecture;
        }

        public override string ToString()
        {
            var output = String.Format(@"{0}{1}{2}{1}{3}", this.BuildFilePath, this.TokenSeparator, this.Platform.ToDefaultString(), this.Architecture.ToDefaultString());
            return output;
        }
    }
}
