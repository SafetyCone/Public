using System;


namespace Public.Common.Lib.Code.Physical
{
    // Ok.
    public static class ProjectFileLanguageExtensions
    {
        public const string CSharp = @"csproj";
        public const string Cpp = @"vcxproj";


        public static string ToDefaultString(this Language language)
        {
            string output;
            switch (language)
            {
                case Language.CSharp:
                    output = ProjectFileLanguageExtensions.CSharp;
                    break;

                case Language.Cpp:
                    output = ProjectFileLanguageExtensions.Cpp;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Language>(language);
            }

            return output;
        }

        public static Language FromDefault(string projectFileExtension)
        {
            Language output;
            if (!ProjectFileLanguageExtensions.TryFromDefault(projectFileExtension, out output))
            {
                string message = String.Format(@"Unrecognized project file extension: {0}.", projectFileExtension);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string projectFileExtension, out Language value)
        {
            bool output = true;
            value = Language.CSharp;

            switch (projectFileExtension)
            {
                case ProjectFileLanguageExtensions.CSharp:
                    value = Language.CSharp;
                    break;

                case ProjectFileLanguageExtensions.Cpp:
                    value = Language.Cpp;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
