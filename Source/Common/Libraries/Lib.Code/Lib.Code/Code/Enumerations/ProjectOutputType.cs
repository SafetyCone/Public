using System;


namespace Public.Common.Lib.Code.Physical
{
    public enum ProjectOutputType
    {
        Exe, // Console executable.
        Library, // Library.
        WinExe, // Windows executable.
    }


    public static class ProjectOutputTypeExtensions
    {
        public const string Exe = @"Exe";
        public const string Library = @"Library";
        public const string WinExe = @"WinExe";


        public static string ToDefaultString(this ProjectOutputType projectOutputType)
        {
            string output;
            switch (projectOutputType)
            {
                case ProjectOutputType.Exe:
                    output = ProjectOutputTypeExtensions.Exe;
                    break;

                case ProjectOutputType.Library:
                    output = ProjectOutputTypeExtensions.Library;
                    break;

                case ProjectOutputType.WinExe:
                    output = ProjectOutputTypeExtensions.WinExe;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ProjectOutputType>(projectOutputType);
            }

            return output;
        }

        public static ProjectOutputType FromDefault(string projectOutputType)
        {
            ProjectOutputType output;
            if (!ProjectOutputTypeExtensions.TryFromDefault(projectOutputType, out output))
            {
                string message = String.Format(@"Unrecognized project output type string: {0}.", projectOutputType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string projectOutputType, out ProjectOutputType value)
        {
            bool output = true;
            value = ProjectOutputType.Exe;

            switch (projectOutputType)
            {
                case ProjectOutputTypeExtensions.Exe:
                    value = ProjectOutputType.Exe;
                    break;

                case ProjectOutputTypeExtensions.Library:
                    value = ProjectOutputType.Library;
                    break;

                case ProjectOutputTypeExtensions.WinExe:
                    value = ProjectOutputType.WinExe;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
