using System;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;
using BaseExtensions = Public.Common.Lib.IO.Serialization.Extensions.SerializationListExtensions;


namespace Public.Common.Lib.Code.Serialization.Extensions
{
    // 
    public static class SerializationListCodeExtensions
    {
        public const string DefaultCSharpCodeFileMoniker = @"DefaultCSharpCodeFile";
        public const string DefaultCSharpProjectMoniker = @"DefaultCSharpProject";
        public const string DefaultSolutionMoniker = @"DefaultSolution";


        #region Static

        public static void AddDefaultSerializersByMoniker(SerializationList list)
        {
            BaseExtensions.AddDefaultSerializersByMoniker(list);

            list.SerializersByMoniker.Add(SerializationListCodeExtensions.DefaultCSharpCodeFileMoniker, new CSharpCodeFileSerializer());
            list.SerializersByMoniker.Add(SerializationListCodeExtensions.DefaultCSharpProjectMoniker, new CSharpProjectSerializer());
            list.SerializersByMoniker.Add(SerializationListCodeExtensions.DefaultSolutionMoniker, new SolutionSerializer());
        }

        #endregion


        public static void AddSolution(this SerializationList list, SolutionSerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListCodeExtensions.DefaultSolutionMoniker);
        }

        public static void AddSolution(this SerializationList list, string path, Solution solution)
        {
            SolutionSerializationUnit unit = new SolutionSerializationUnit(path, solution);
            list.AddSolution(unit);
        }

        public static void AddProject(this SerializationList list, CSharpProjectSerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListCodeExtensions.DefaultCSharpProjectMoniker);
        }

        public static void AddProject(this SerializationList list, string path, Project project)
        {
            CSharpProjectSerializationUnit unit = new CSharpProjectSerializationUnit(path, project);
            list.AddProject(unit);
        }

        public static void AddCodeFile(this SerializationList list, CSharpCodeFileSerializationUnit unit)
        {
            list.AddUnitByMoniker(unit, SerializationListCodeExtensions.DefaultCSharpCodeFileMoniker);
        }

        public static void AddCodeFile(this SerializationList list, string path, CodeFile codeFile)
        {
            CSharpCodeFileSerializationUnit unit = new CSharpCodeFileSerializationUnit(path, codeFile);
            list.AddCodeFile(unit);
        }
    }
}
