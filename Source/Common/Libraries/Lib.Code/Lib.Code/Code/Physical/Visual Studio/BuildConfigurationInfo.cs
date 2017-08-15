using System;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.Code.Physical
{
    // Ok.
    // Error report will always be the default.
    // Warning level will always be the default.
    public class BuildConfigurationInfo
    {
        public const string DefaultErrorReport = @"prompt";
        public const int DefaultWarningLevel = 4;
        public const string DebugConstant = @"DEBUG";
        public const string TraceConstant = @"TRACE";
        public const string BinDirectoryName = @"bin";
        public const string ObjDirectoryName = @"obj";


        #region Static

        /// <remarks>
        /// The configuration token (Debug or Release) will be added 
        /// </remarks>
        public static string GetObjectIntermediateDirectoryName(VisualStudioVersion visualStudioVersion)
        {
            string vsVersionStr = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);

            string output = Path.Combine(BuildConfigurationInfo.ObjDirectoryName, vsVersionStr) + Path.DirectorySeparatorChar;
            return output;
        }

        public static string GetBinaryOutputDirectoryName(VisualStudioVersion visualStudioVersion, Configuration configuration)
        {
            string vsVersionStr = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);
            string configStr = ConfigurationExtensions.ToDefaultString(configuration);

            string output = Path.Combine(BuildConfigurationInfo.BinDirectoryName, vsVersionStr, configStr);
            return output;
        }

        public static BuildConfigurationInfo GetDebugDefault(VisualStudioVersion visualStudioVersion)
        {
            BuildConfigurationInfo output = new BuildConfigurationInfo();
            output.BuildConfiguration = new BuildConfiguration(Configuration.Debug, Platform.AnyCPU);
            output.DebugSymbols = true;
            output.DebugType = DebugType.Full;
            output.Optimize = false;
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);
            output.BinOutputPath = BuildConfigurationInfo.GetBinaryOutputDirectoryName(visualStudioVersion, Configuration.Debug);
            output.ObjIntermediatePath = BuildConfigurationInfo.GetObjectIntermediateDirectoryName(visualStudioVersion);
            output.DefinedConstants.Add(BuildConfigurationInfo.DebugConstant);
            output.DefinedConstants.Add(BuildConfigurationInfo.TraceConstant);

            return output;
        }

        public static BuildConfigurationInfo GetReleaseDefault(VisualStudioVersion visualStudioVersion)
        {
            BuildConfigurationInfo output = new BuildConfigurationInfo();
            output.BuildConfiguration = new BuildConfiguration(Configuration.Release, Platform.AnyCPU);
            output.DebugType = DebugType.PdbOnly;
            output.Optimize = true;
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);
            output.BinOutputPath = BuildConfigurationInfo.GetBinaryOutputDirectoryName(visualStudioVersion, Configuration.Release);
            output.ObjIntermediatePath = BuildConfigurationInfo.GetObjectIntermediateDirectoryName(visualStudioVersion);
            output.DefinedConstants.Add(BuildConfigurationInfo.TraceConstant);

            return output;
        }

        #endregion


        public BuildConfiguration BuildConfiguration { get; set; }
        public bool DebugSymbols { get; set; }
        public DebugType DebugType { get; set; }
        public bool Optimize { get; set; }
        public string BinOutputPath { get; set; }
        public string ObjIntermediatePath { get; set; }
        public List<string> DefinedConstants { get; set; }
        public bool AllowUnsafeBlocks { get; set; }


        public BuildConfigurationInfo()
        {
            this.DefinedConstants = new List<string>();
        }
    }
}
