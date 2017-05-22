using System;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// The solution type within a domain. These are specified by the Minex organization paths standard.
    /// </summary>
    public enum SolutionType
    {
        Application,
        Experiment,
        Library,
        Script,
    }


    /// <summary>
    /// Basic singular string representations.
    /// </summary>
    public static class SolutionTypeExtensions
    {
        public const string Application = @"Application";
        public const string Experiment = @"Experiment";
        public const string Library = @"Library";
        public const string Script = @"Script";


        public static string[] GetAllStrings()
        {
            string[] output = new string[]
            {
                SolutionTypeExtensions.Application,
                SolutionTypeExtensions.Experiment,
                SolutionTypeExtensions.Library,
                SolutionTypeExtensions.Script,
            };

            return output;
        }

        public static string ToDefaultString(this SolutionType solutionType)
        {
            string output;
            switch (solutionType)
            {
                case SolutionType.Application:
                    output = SolutionTypeExtensions.Application;
                    break;

                case SolutionType.Experiment:
                    output = SolutionTypeExtensions.Experiment;
                    break;

                case SolutionType.Library:
                    output = SolutionTypeExtensions.Library;
                    break;

                case SolutionType.Script:
                    output = SolutionTypeExtensions.Script;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<SolutionType>(solutionType);
            }

            return output;
        }

        public static SolutionType FromDefault(string solutionType)
        {
            SolutionType output;
            if (!SolutionTypeExtensions.TryFromDefault(solutionType, out output))
            {
                string message = String.Format(@"Unrecognized solution type string: {0}.", solutionType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string solutionType, out SolutionType value)
        {
            bool output = true;
            value = SolutionType.Experiment;

            switch (solutionType)
            {
                case SolutionTypeExtensions.Application:
                    value = SolutionType.Application;
                    break;

                case SolutionTypeExtensions.Experiment:
                    value = SolutionType.Experiment;
                    break;

                case SolutionTypeExtensions.Library:
                    value = SolutionType.Library;
                    break;

                case SolutionTypeExtensions.Script:
                    value = SolutionType.Script;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }


    /// <summary>
    /// Basic plural string representations.
    /// </summary>
    public static class SolutionTypePluralExtensions
    {
        public const string Applications = @"Applications";
        public const string Experiments = @"Experiments";
        public const string Libraries = @"Libraries";
        public const string Scripts = @"Scripts";


        public static string ToDefaultPluralString(this SolutionType solutionType)
        {
            string output;
            switch (solutionType)
            {
                case SolutionType.Application:
                    output = SolutionTypePluralExtensions.Applications;
                    break;

                case SolutionType.Experiment:
                    output = SolutionTypePluralExtensions.Experiments;
                    break;

                case SolutionType.Library:
                    output = SolutionTypePluralExtensions.Libraries;
                    break;

                case SolutionType.Script:
                    output = SolutionTypePluralExtensions.Scripts;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<SolutionType>(solutionType);
            }

            return output;
        }

        public static SolutionType FromDefault(string solutionType)
        {
            SolutionType output;
            if (!SolutionTypePluralExtensions.TryFromDefault(solutionType, out output))
            {
                string message = String.Format(@"Unrecognized plural solution type string: {0}.", solutionType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string solutionType, out SolutionType solutionTypeValue)
        {
            bool output = true;
            solutionTypeValue = SolutionType.Experiment;
            
            switch (solutionType)
            {
                case SolutionTypePluralExtensions.Applications:
                    solutionTypeValue = SolutionType.Application;
                    break;

                case SolutionTypePluralExtensions.Experiments:
                    solutionTypeValue = SolutionType.Experiment;
                    break;

                case SolutionTypePluralExtensions.Libraries:
                    solutionTypeValue = SolutionType.Library;
                    break;

                case SolutionTypePluralExtensions.Scripts:
                    solutionTypeValue = SolutionType.Script;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
