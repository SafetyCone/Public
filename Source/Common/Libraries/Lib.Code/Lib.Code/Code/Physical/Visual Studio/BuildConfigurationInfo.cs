using System;
using System.Collections.Generic;


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


        #region Static

        public static BuildConfigurationInfo GetDebugDefault(VisualStudioVersion visualStudioVersion)
        {
            BuildConfigurationInfo output = new BuildConfigurationInfo();
            output.BuildConfiguration = new BuildConfiguration(Configuration.Debug, Platform.AnyCPU);
            output.DebugSymbols = true;
            output.DebugType = DebugType.Full;
            output.Optimize = false;
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);
            output.OutputPath = String.Format(@"\bin\{0}\Debug", vsVersion);
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
            output.OutputPath = String.Format(@"\bin\{0}\Release", vsVersion);
            output.DefinedConstants.Add(BuildConfigurationInfo.TraceConstant);

            return output;
        }

        #endregion


        public BuildConfiguration BuildConfiguration { get; set; }
        public bool DebugSymbols { get; set; }
        public DebugType DebugType { get; set; }
        public bool Optimize { get; set; }
        public string OutputPath { get; set; }
        public List<string> DefinedConstants { get; set; }


        public BuildConfigurationInfo()
        {
            this.DefinedConstants = new List<string>();
        }
    }
}
