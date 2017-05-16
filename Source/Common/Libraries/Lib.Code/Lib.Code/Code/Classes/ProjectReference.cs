using System;
using SysPath = System.IO.Path;
using System.Xml;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code
{
    public class ProjectReference
    {
        #region Static

        public static ProjectReference GetFromProjectFilePath(string projectFilePath)
        {
            ProjectReference output = new ProjectReference();
            ProjectReference.ModifyForPath(output, projectFilePath);
            ProjectReference.ModifyForName(output, projectFilePath);
            ProjectReference.ModifyForGuidAndOutputType(output, projectFilePath);

            return output;
        }

        private static void ModifyForPath(ProjectReference reference, string projectFilePath)
        {
            reference.Path = projectFilePath;
        }

        private static void ModifyForName(ProjectReference reference, string projectFilePath)
        {
            string projectFileName = SysPath.GetFileName(projectFilePath);
            string projectFileExtension = PathExtensions.GetExtensionOnly(projectFilePath);

            string[] fileNameTokens = projectFileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

            Language language = ProjectFileLanguageExtensions.FromDefault(projectFileExtension);
            switch(language)
            {
                case Language.CSharp:
                    {
                        string[] nameTokens = new string[fileNameTokens.Length - 1];
                        Array.Copy(fileNameTokens, nameTokens, nameTokens.Length);

                        reference.Name = nameTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);
                    }
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Language>(language);
            }
        }

        private static void ModifyForGuidAndOutputType(ProjectReference reference, string projectFilePath)
        {
            XmlDocument document = new XmlDocument();
            document.LoadNoNamespaces(projectFilePath);

            ProjectReference.ModifyForGuid(reference, document);
            ProjectReference.ModifyForOutputType(reference, document);
        }

        private static void ModifyForGuid(ProjectReference reference, XmlDocument document)
        {
            string guidStr = document.SelectSingleNode(@"Project/PropertyGroup/ProjectGuid").InnerText;
            reference.GUID = Guid.Parse(guidStr);
        }

        public static void ModifyForOutputType(ProjectReference reference, XmlDocument document)
        {
            string outputTypeStr = document.SelectSingleNode(@"Project/PropertyGroup/OutputType").InnerText;
            reference.OutputType = ProjectOutputTypeExtensions.FromDefault(outputTypeStr);
        }

        #endregion


        public string Name { get; set; }
        public string Path { get; set; }
        public Guid GUID { get; set; }
        public ProjectOutputType OutputType { get; set; }


        public ProjectReference()
        {
        }

        public ProjectReference(string name, string path, Guid guid)
        {
            this.Name = name;
            this.Path = path;
            this.GUID = guid;
        }

        public ProjectReference(ProjectReference other)
            : this(other.Name, other.Path, other.GUID)
        {
        }
    }
}
