using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    public class SolutionSerializer : SerializerBase<SolutionSerializationUnit>
    {
        public const string SolutionFileExtension = @"sln";


        #region Static

        public static string GetSolutionFilePath(string solutionDirectoryPath, Solution solution)
        {
            string fileName = String.Format(@"{0}.{1}", solution.Info.NamesInfo.Name, SolutionSerializer.SolutionFileExtension);

            string output = Path.Combine(solutionDirectoryPath, fileName);
            return output;
        }

        public static void Serialize(string filePath, Solution solution)
        {
            using(CodeFileWriter writer = new CodeFileWriter(filePath, "\t"))
            {
                SolutionSerializer.WriteVisualStudioInformation(writer, solution);
                SolutionSerializer.WriteProjectReferences(writer, solution);
                SolutionSerializer.WriteGlobal(writer, solution);
            }
        }

        private static void WriteGlobal(CodeFileWriter writer, Solution solution)
        {
            writer.WriteIndentedLine(@"Global");
            writer.IncreaseIndent();

            SolutionSerializer.WriteSolutionConfigurationPlatforms(writer, solution);
            SolutionSerializer.WriteProjectConfigurationPlatforms(writer, solution);
            SolutionSerializer.WriteSolutionProperties(writer, solution);

            writer.DecreaseIndent();
            writer.WriteIndentedLine(@"EndGlobal");
        }

        private static void WriteSolutionProperties(CodeFileWriter writer, Solution solution)
        {
            writer.WriteIndentedLine(@"GlobalSection(SolutionProperties) = preSolution");
            writer.IncreaseIndent();
            writer.WriteIndentedLine(@"HideSolutionNode = FALSE");
            writer.DecreaseIndent();
            writer.WriteIndentedLine(@"EndGlobalSection");
        }

        private static void WriteProjectConfigurationPlatforms(CodeFileWriter writer, Solution solution)
        {
            writer.WriteIndentedLine(@"GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            writer.IncreaseIndent();

            Dictionary<Guid, List<string>> linesByProjectID = new Dictionary<Guid, List<string>>();
            string curLine;
            foreach (BuildConfiguration config in solution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Keys)
            {
                ProjectBuildConfigurationSet configSet = solution.ProjectBuildConfigurationsBySolutionBuildConfiguration[config];

                foreach (Guid ID in configSet.ProjectBuildConfigurationsByProjectGuid.Keys)
                {
                    List<string> lines;
                    if(linesByProjectID.ContainsKey(ID))
                    {
                        lines = linesByProjectID[ID];
                    }
                    else
                    {
                        lines = new List<string>();
                        linesByProjectID.Add(ID, lines);
                    }

                    ProjectBuildConfigurationInfo info = configSet.ProjectBuildConfigurationsByProjectGuid[ID];

                    string guidToken = String.Format(@"{{{0}}}", ID.ToString().ToUpperInvariant());
                    string configToken = SolutionSerializer.GetBuildConfigurationToken(config);
                    string activeConfig = SolutionSerializer.GetBuildConfigurationToken(info.ProjectActiveConfiguration);
                    string activeConfigToken = String.Format(@"ActiveCfg = {0}", activeConfig);

                    curLine = String.Format(@"{0}.{1}.{2}", guidToken, configToken, activeConfigToken);
                    lines.Add(curLine);

                    if (info.Build)
                    {
                        string buildToken = String.Format(@"Build.0 = {0}", activeConfig);
                        curLine = String.Format(@"{0}.{1}.{2}", guidToken, configToken, buildToken);
                        lines.Add(curLine);
                    }
                }
            }

            foreach (Guid ID in linesByProjectID.Keys)
            {
                List<string> lines = linesByProjectID[ID];
                foreach (string line in lines)
                {
                    writer.WriteIndentedLine(line);
                }
            }

            writer.DecreaseIndent();
            writer.WriteIndentedLine(@"EndGlobalSection");
        }

        private static void WriteSolutionConfigurationPlatforms(CodeFileWriter writer, Solution solution)
        {
            writer.WriteIndentedLine(@"GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            writer.IncreaseIndent();

            foreach (BuildConfiguration config in solution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Keys)
            {
                string configToken = SolutionSerializer.GetBuildConfigurationToken(config);
                string line = String.Format(@"{0} = {0}", configToken);
                writer.WriteIndentedLine(line);
            }

            writer.DecreaseIndent();
            writer.WriteIndentedLine(@"EndGlobalSection");
        }

        private static string GetBuildConfigurationToken(BuildConfiguration config)
        {
            string output = String.Format(@"{0}|{1}", config.Configuration.ToDefaultString(), SolutionPlatformExtensions.ToDefaultString(config.Platform));
            return output;
        }

        private static void WriteProjectReferences(CodeFileWriter writer, Solution solution)
        {
            foreach(Guid ID in solution.ProjectsByGuid.Keys)
            {
                ProjectReference project = solution.ProjectsByGuid[ID];

                string projectLine = String.Format(@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{{2}}}""", project.Name, project.RelativePath, project.GUID.ToString().ToUpperInvariant());
                writer.WriteLine(projectLine);
                writer.WriteLine(@"EndProject");
            }
        }

        private static void WriteVisualStudioInformation(CodeFileWriter writer, Solution solution)
        {
            writer.WriteBlankLine();

            string vsFileFormatLine = SolutionSerializer.GetFileFormatVersionLine(solution);
            writer.WriteLine(vsFileFormatLine);

            string vsSolutionFileVersionLine = VisualStudioVersionSolutionFileExtensions.ToDefaultString(solution.VisualStudioVersion);
            writer.WriteLine(vsSolutionFileVersionLine);

            SolutionSerializer.WriteVsVersionAndMinVersion(writer, solution);
        }

        private static void WriteVsVersionAndMinVersion(CodeFileWriter writer, Solution solution)
        {
            switch(solution.VisualStudioVersion)
            {
                case VisualStudioVersion.VS2013:
                    writer.WriteLine(@"VisualStudioVersion = 12.0.21005.1");
                    writer.WriteLine(@"MinimumVisualStudioVersion = 10.0.40219.1");
                    break;

                case VisualStudioVersion.VS2015:
                    writer.WriteLine(@"VisualStudioVersion = 14.0.25420.1");
                    writer.WriteLine(@"MinimumVisualStudioVersion = 10.0.40219.1");
                    break;

                case VisualStudioVersion.VS2017:
                    writer.WriteLine(@"VisualStudioVersion = 15.0.26403.0");
                    writer.WriteLine(@"MinimumVisualStudioVersion = 10.0.40219.1");
                    break;

                default:
                    break; // Do nothing.
            }
        }

        private static string GetFileFormatVersionLine(Solution solution)
        {
            string output;
            switch (solution.VisualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    output = @"Microsoft Visual Studio Solution File, Format Version 11.00";
                    break;

                default:
                    output = @"Microsoft Visual Studio Solution File, Format Version 12.00";
                    break;
            }

            return output;
        }

        public static Solution Deserialize(string filePath)
        {
            string[] untrimmedLines = File.ReadAllLines(filePath);
            List<string> lines = new List<string>();
            foreach (string line in untrimmedLines)
            {
                string trimmedLine = line.Trim(' ', '\t');
                if (String.Empty != trimmedLine)
                {
                    lines.Add(trimmedLine);
                }
            }

            Solution output = new Solution();
            output.Info.NamesInfo.Name = Path.GetFileNameWithoutExtension(filePath);
            output.Info.NamesInfo.FileName = Path.GetFileName(filePath);

            string fileDirectoryPath = Path.GetDirectoryName(filePath);
            output.Info.NamesInfo.DirectoryName = Path.GetFileName(fileDirectoryPath); // Treats directory as file.

            // Try to get the solution type.
            string fileDirectoryParentDirectoryPath = Path.GetDirectoryName(fileDirectoryPath);
            string fileDirectoryParentDirectoryName = Path.GetFileName(fileDirectoryParentDirectoryPath);
            Logical.SolutionType solutionType;
            if(Logical.SolutionTypeExtensions.TryFromDefault(fileDirectoryParentDirectoryName, out solutionType))
            {
                output.Info.Type = solutionType;
            }

            string currentLine;
            int lineIndex = 1; // Skip the first line.

            currentLine = lines[lineIndex];
            output.VisualStudioVersion = VisualStudioVersionSolutionFileExtensions.FromDefault(currentLine);

            SolutionSerializer.ProcessLines(output, lines, ref lineIndex);

            return output;
        }

        private static void ProcessLines(Solution solution, List<string> lines, ref int lineIndex)
        {
            int VsVersion = VisualStudioVersionToolsVersionExtensions.ToDefaultInt(solution.VisualStudioVersion);
            if (VisualStudioVersionToolsVersionExtensions.VS2013 > VsVersion)
            {
                SolutionSerializer.Format11MoveToProjects(ref lineIndex);
            }
            else
            {
                SolutionSerializer.Format12MoveToProjects(ref lineIndex);
            }

            SolutionSerializer.ProcessProjects(solution, lines, ref lineIndex);

            SolutionSerializer.ProcessGlobal(solution, lines, ref lineIndex);
        }

        private static void ProcessGlobal(Solution solution, List<string> lines, ref int lineIndex)
        {
            lineIndex++; // Line comes in as "Global", move to first global section line.

            SolutionSerializer.ProcessSolutionConfigurationPlatforms(solution, lines, ref lineIndex);
            SolutionSerializer.ProcessProjectConfigurationPlatforms(solution, lines, ref lineIndex);
            SolutionSerializer.ProcessSolutionProperties(solution, lines, ref lineIndex);

            lineIndex++;
            string line = lines[lineIndex];

            if(@"EndGlobal" != line)
            {
                throw new Exception(@"Improper parse of global node of solution file.");
            }
        }

        private static void ProcessSolutionProperties(Solution solution, List<string> lines, ref int lineIndex)
        {
            lineIndex++; // Line comes in as "EndGlobalSection", move to the first project build configuration.
            string line = lines[lineIndex];

            while (@"EndGlobalSection" != line)
            {
                // Do nothing.

                lineIndex++;
                line = lines[lineIndex];
            }
        }

        private static void ProcessProjectConfigurationPlatforms(Solution solution, List<string> lines, ref int lineIndex)
        {
            lineIndex += 2; // Line comes in as "EndGlobalSection", move to the first project build configuration.
            string line = lines[lineIndex];

            while (@"EndGlobalSection" != line)
            {
                string[] projectConfigTokens = line.Split('.');

                string guidToken = projectConfigTokens[0];
                string buildConfigToken = projectConfigTokens[1];
                string activeConfigToken = projectConfigTokens[2];

                Guid projectGuid = Guid.Parse(guidToken);
                BuildConfiguration buildConfig = SolutionSerializer.ParseBuildConfiguration(buildConfigToken);

                ProjectBuildConfigurationSet projectConfigSet = solution.ProjectBuildConfigurationsBySolutionBuildConfiguration[buildConfig];

                if(!projectConfigSet.ProjectBuildConfigurationsByProjectGuid.ContainsKey(projectGuid))
                {
                    projectConfigSet.ProjectBuildConfigurationsByProjectGuid.Add(projectGuid, new ProjectBuildConfigurationInfo());
                }

                if(@"Build" == activeConfigToken)
                {
                    projectConfigSet.ProjectBuildConfigurationsByProjectGuid[projectGuid].Build = true;
                }
                else
                {
                    string[] activeConfigTokens = activeConfigToken.Split('=');

                    string activeConfigBuildToken = activeConfigTokens[1].Trim(' ');
                    BuildConfiguration activeBuildConfig = SolutionSerializer.ParseBuildConfiguration(activeConfigBuildToken);

                    projectConfigSet.ProjectBuildConfigurationsByProjectGuid[projectGuid].ProjectActiveConfiguration = activeBuildConfig;
                }

                lineIndex++;
                line = lines[lineIndex];
            }
        }

        private static void ProcessSolutionConfigurationPlatforms(Solution solution, List<string> lines, ref int lineIndex)
        {
            lineIndex++; // Line comes in as "GlobalSection(SolutionConfigurationPlatforms)", move to the first build configuration.
            string line = lines[lineIndex];

            while (@"EndGlobalSection" != line)
            {
                string[] configTokens = line.Split('=');

                string buildConfigToken = configTokens[0].Trim(' ');
                BuildConfiguration buildConfig = SolutionSerializer.ParseBuildConfiguration(buildConfigToken);

                solution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Add(buildConfig, new ProjectBuildConfigurationSet());

                lineIndex++;
                line = lines[lineIndex];
            }
        }

        private static BuildConfiguration ParseBuildConfiguration(string buildConfigToken)
        {
            string[] buildConfigTokens = buildConfigToken.Split('|');

            string configToken = buildConfigTokens[0];
            string platformToken = buildConfigTokens[1];

            Configuration configuration = ConfigurationExtensions.FromDefault(configToken);
            Platform platform = SolutionPlatformExtensions.FromDefault(platformToken);

            BuildConfiguration buildConfig = new BuildConfiguration(configuration, platform);
            return buildConfig;
        }

        private static void ProcessProjects(Solution solution, List<string> lines, ref int lineIndex)
        {
            string line = lines[lineIndex];

            while (@"Global" != line)
            {
                string[] projectTokens = line.Split('=', ',');

                string projectToken = projectTokens[0];
                string name = projectTokens[1].Trim('"', ' ');
                string relativePath = projectTokens[2].Trim('"', ' ');
                string guidStr = projectTokens[3].Trim('"', ' ');

                Guid guid = Guid.Parse(guidStr);

                ProjectReference reference = new ProjectReference(name, relativePath, guid);
                solution.ProjectsByGuid.Add(reference.GUID, reference);

                lineIndex++;
                line = lines[lineIndex];

                if(@"EndProject" != line)
                {
                    throw new Exception(@"Improper parse of project in solution file.");
                }

                lineIndex++;
                line = lines[lineIndex];
            }
        }

        private static void Format12MoveToProjects(ref int lineIndex)
        {
            lineIndex += 3; // Skip the VisualStudioVersion and MinimumVisualStudioVersion lines.
        }

        private static void Format11MoveToProjects(ref int lineIndex)
        {
            lineIndex++;
        }

        #endregion


        protected override void Serialize(SolutionSerializationUnit unit)
        {
            SolutionSerializer.Serialize(unit.Path, unit.Solution);
        }
    }
}
