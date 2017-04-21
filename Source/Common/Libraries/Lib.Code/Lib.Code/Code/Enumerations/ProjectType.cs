using System;

using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Specifies the exact Visual Studio project type.
    /// </summary>
    /// <remarks>
    /// See: https://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs
    /// </remarks>
    public enum ProjectType
    {
        Console,
        Library,
        WindowsForms,
        //WPF, // TODO
        //WebSite,
        //WebApp,
    }


    /// <summary>
    /// Basic string representations.
    /// </summary>
    public static class ProjectTypeExtensions
    {
        public const string Console = @"Console";
        public const string Library = @"Library";
        public const string WindowsForms = @"WindowsForms";


        public static string ToDefaultString(this ProjectType projectType)
        {
            string output;
            switch (projectType)
            {
                case ProjectType.Console:
                    output = ProjectTypeExtensions.Console;
                    break;

                case ProjectType.Library:
                    output = ProjectTypeExtensions.Library;
                    break;

                case ProjectType.WindowsForms:
                    output = ProjectTypeExtensions.WindowsForms;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ProjectType>(projectType);
            }

            return output;
        }

        public static ProjectType FromDefault(string projectType)
        {
            ProjectType output;
            if (!ProjectTypeExtensions.TryFromDefault(projectType, out output))
            {
                string message = String.Format(@"Unrecognized project type string: {0}.", projectType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string projectType, out ProjectType value)
        {
            bool output = true;
            value = ProjectType.Console;

            switch (projectType)
            {
                case ProjectTypeExtensions.Console:
                    value = ProjectType.Console;
                    break;

                case ProjectTypeExtensions.Library:
                    value = ProjectType.Library;
                    break;

                case ProjectTypeExtensions.WindowsForms:
                    value = ProjectType.WindowsForms;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }

        public static ProjectOutputType ToDefaultOutputType(this ProjectType projectType)
        {
            ProjectOutputType output;
            switch (projectType)
            {
                case ProjectType.Console:
                    output = ProjectOutputType.Exe;
                    break;

                case ProjectType.Library:
                    output = ProjectOutputType.Library;
                    break;

                case ProjectType.WindowsForms:
                    output = ProjectOutputType.WinExe;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<ProjectType>(projectType);
            }

            return output;
        }

        public static ProjectType FromDefault(ProjectOutputType projectType)
        {
            ProjectType output;
            if (!ProjectTypeExtensions.TryFromDefault(projectType, out output))
            {
                string message = String.Format(@"Unrecognized project type string: {0}.", projectType);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(ProjectOutputType projectType, out ProjectType value)
        {
            bool output = true;
            value = ProjectType.Console;

            switch (projectType)
            {
                case ProjectOutputType.Exe:
                    value = ProjectType.Console;
                    break;

                case ProjectOutputType.Library:
                    value = ProjectType.Library;
                    break;

                case ProjectOutputType.WinExe:
                    value = ProjectType.WindowsForms;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
