using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Public.Common.Lib.Code.Logical;
using LogicalDelegate = Public.Common.Lib.Code.Logical.Delegate;
using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Physical.CSharp;
using CSharpConstants = Public.Common.Lib.Code.Physical.CSharp.Constants;
using CSharpTypes = Public.Common.Lib.Code.Physical.CSharp.Types;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    /// <summary>
    /// Allows serialization ONLY of C# code files to a path. Designed for use in a serialization list.
    /// </summary>
    public class CSharpCodeFileSerializer : SerializerBase<CSharpCodeFileSerializationUnit>
    {
        private const string UnexpectedScopeTypeErrorMessage = @"Unexpected type of scope.";
        private const string UnexpectedLogicalObjectTypeErrorMessage = @"Unexpected type of logical object.";


        #region Static

        private static Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> ScopeWriters { get; set; }
        private static Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> LogicalObjectWriters { get; set; }


        static CSharpCodeFileSerializer()
        {
            Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> scopeWriters = new Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>>();
            CSharpCodeFileSerializer.ScopeWriters = scopeWriters;

            scopeWriters.Add(typeof(Scope).FullName, CSharpCodeFileSerializer.WriteScope);
            scopeWriters.Add(typeof(NamespaceScope).FullName, CSharpCodeFileSerializer.WriteNamespaceScope);

            Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> logicalObjectWriters = new Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>>();
            CSharpCodeFileSerializer.LogicalObjectWriters = logicalObjectWriters;

            logicalObjectWriters.Add(typeof(Class).FullName, CSharpCodeFileSerializer.WriteClass);
            logicalObjectWriters.Add(typeof(EmptyType).FullName, CSharpCodeFileSerializer.WriteEmptyType);
            logicalObjectWriters.Add(typeof(Struct).FullName, CSharpCodeFileSerializer.WriteStruct);
            logicalObjectWriters.Add(typeof(LogicalDelegate).FullName, CSharpCodeFileSerializer.WriteDelegate);
            logicalObjectWriters.Add(typeof(Enumeration).FullName, CSharpCodeFileSerializer.WriteEnumeration);
            logicalObjectWriters.Add(typeof(Method).FullName, CSharpCodeFileSerializer.WriteMethod);
            logicalObjectWriters.Add(typeof(Member).FullName, CSharpCodeFileSerializer.WriteMember);
            logicalObjectWriters.Add(typeof(InitializedMember).FullName, CSharpCodeFileSerializer.WriteInitializedMember);
            logicalObjectWriters.Add(typeof(PropertyMember).FullName, CSharpCodeFileSerializer.WritePropertyMember);
        }

        public static void SerializeStatic(string path, CodeFile codeFile)
        {
            using (CSharpCodeFileWriter writer = new CSharpCodeFileWriter(path))
            {
                CSharpCodeFileSerializer.WriteUsings(writer, codeFile.Header.Usings);
                CSharpCodeFileSerializer.WriteScopes(writer, codeFile.Scopes);
            }
        }

        private static void WriteUsings(CSharpCodeFileWriter writer, IEnumerable<UsingDeclaration> usings)
        {
            List<UsingDeclaration> usingsList = new List<UsingDeclaration>(usings);

            if (null != usings)
            {
                usingsList.Sort();

                foreach (UsingDeclaration usingDeclaration in usingsList)
                {
                    string line = CSharpCodeFileSerializer.FormatUsing(usingDeclaration);
                    writer.WriteCodeLine(line);
                }
            }

            writer.WriteBlankLine();
            writer.WriteBlankLine();
        }

        private static string FormatUsing(UsingDeclaration usingDeclaration)
        {
            string declaration = usingDeclaration.ToString();

            string output = String.Format(@"using {0}", declaration);
            return output;
        }

        private static void WriteScopes(CSharpCodeFileWriter writer, IEnumerable<ScopeBase> scopes)
        {
            foreach (ScopeBase scope in scopes)
            {
                // Determine what type of scope it is.
                string scopeTypeName = scope.GetType().FullName;
                if (CSharpCodeFileSerializer.ScopeWriters.ContainsKey(scopeTypeName))
                {
                    CSharpCodeFileSerializer.ScopeWriters[scopeTypeName](writer, scope);
                }
                else
                {
                    throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedScopeTypeErrorMessage);
                }
            }
        }

        private static void WriteScope(CSharpCodeFileWriter writer, ScopeBase scopeBase)
        {
            Scope scope = scopeBase as Scope;
            if (null == scope)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedScopeTypeErrorMessage);
            }

            // Determine what type of logical object we have.
            string logicalObjectTypeName = scope.LogicalObject.GetType().FullName;
            if (CSharpCodeFileSerializer.LogicalObjectWriters.ContainsKey(logicalObjectTypeName))
            {
                CSharpCodeFileSerializer.LogicalObjectWriters[logicalObjectTypeName](writer, scope);
            }
            else
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }
        }

        private static void WriteNamespaceScope(CSharpCodeFileWriter writer, ScopeBase scopeBase)
        {
            NamespaceScope namespaceScope = scopeBase as NamespaceScope;
            if (null == namespaceScope)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedScopeTypeErrorMessage);
            }

            string line = String.Format(@"{0} {1}", CSharpConstants.NamespaceKeyword, namespaceScope.Name);
            writer.WriteLine(line);
            writer.OpenScope();

            CSharpCodeFileSerializer.WriteScopes(writer, namespaceScope.Children);

            writer.CloseScope();
        }

        private static void WriteEmptyType(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            EmptyType emptyTypeObj = scope.LogicalObject as EmptyType;
            if (null == emptyTypeObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }

            foreach (string line in emptyTypeObj.Lines)
            {
                writer.WriteLine(line);
            }
        }

        private static void WriteClass(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Class classObj = scope.LogicalObject as Class;
            if (null == classObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }

            string line = CSharpCodeFileSerializer.GetClassSignature(writer, classObj);
            writer.WriteIndentedLine(line);
            writer.OpenScope();

            CSharpCodeFileSerializer.WriteScopes(writer, scope.Children);

            writer.CloseScope();
        }

        private static string GetClassSignature(CSharpCodeFileWriter writer, Class classObj)
        {
            string accessibility;
            if (Accessibility.Private == classObj.Accessibility)
            {
                accessibility = String.Empty;
            }
            else
            {
                accessibility = classObj.Accessibility.ToDefaultString();
            }

            string isStatic;
            if (classObj.IsStatic)
            {
                isStatic = CSharpConstants.StaticKeyword;
            }
            else
            {
                isStatic = String.Empty;
            }

            string className;
            if (writer.Types.PhysicalCSharpNamesByLogicalName.ContainsKey(classObj.Name))
            {
                className = writer.Types.PhysicalCSharpNamesByLogicalName[classObj.Name];
            }
            else
            {
                className = classObj.Name;
            }

            string inheritanceChain = CSharpCodeFileSerializer.GetClassInheritanceChain(classObj);

            string[] qualifiers = new string[] { accessibility, isStatic, CSharpConstants.ClassKeyword };
            StringBuilder builder = new StringBuilder();
            foreach (string qualifier in qualifiers)
            {
                if (!String.IsNullOrEmpty(qualifier))
                {
                    builder.Append(qualifier);
                    builder.Append(CSharpConstants.SpaceChar);
                }
            }

            builder.Append(className);
            builder.Append(inheritanceChain);

            string output = builder.ToString();
            return output;
        }

        private static string GetClassInheritanceChain(Class classObj)
        {
            string interfaceInheritanceChain = CSharpCodeFileSerializer.GetClassInterfaceInheritanceChain(classObj);

            string inheritanceChain;
            if (String.IsNullOrEmpty(classObj.BaseClassTypeName))
            {
                if (String.IsNullOrEmpty(interfaceInheritanceChain))
                {
                    inheritanceChain = String.Empty;
                }
                else
                {
                    inheritanceChain = interfaceInheritanceChain;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(interfaceInheritanceChain))
                {
                    inheritanceChain = classObj.BaseClassTypeName;
                }
                else
                {
                    inheritanceChain = String.Format(@"{0}{1} {2}", classObj.BaseClassTypeName, CSharpConstants.SeriesOperatorChar, interfaceInheritanceChain);
                }
            }

            string output;
            if (String.IsNullOrEmpty(inheritanceChain))
            {
                output = String.Empty;
            }
            else
            {
                output = String.Format(@" {0} {1}", CSharpConstants.ClassInheritanceOperatorChar, inheritanceChain);
            }

            return output;
        }

        private static string GetClassInterfaceInheritanceChain(Class classObj)
        {
            string output;
            if (0 == classObj.InterfacesImplemented.Count)
            {
                output = String.Empty;
            }
            else
            {
                output = StringExtensions.LinearizeTokens(classObj.InterfacesImplemented.ToArray(), CSharpConstants.SeriesOperatorChar.ToString());
            }

            return output;
        }

        private static void WriteStruct(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Struct structObj = scope.LogicalObject as Struct;
            if (null == structObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteDelegate(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            LogicalDelegate delegateObj = scope.LogicalObject as LogicalDelegate;
            if (null == delegateObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteEnumeration(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Enumeration enumerationObj = scope.LogicalObject as Enumeration;
            if (null == enumerationObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteMethod(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Method methodObj = scope.LogicalObject as Method;
            if (null == methodObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }

            string methodSignature = CSharpCodeFileSerializer.GetMethodSignature(writer, methodObj);

            writer.WriteIndentedLine(methodSignature);
            writer.OpenScope();

            foreach (string line in methodObj.Lines)
            {
                writer.WriteIndentedLine(line);
            }

            writer.CloseScope();
        }

        private static string GetMethodSignature(CSharpCodeFileWriter writer, Method method)
        {
            string accessibility = method.Accessibility.ToDefaultString();

            string isStatic;
            if (method.IsStatic)
            {
                isStatic = CSharpConstants.StaticKeyword;
            }
            else
            {
                isStatic = String.Empty;
            }

            string returnType;
            if (writer.Types.PhysicalCSharpNamesByLogicalName.ContainsKey(method.ReturnTypeName))
            {
                returnType = writer.Types.PhysicalCSharpNamesByLogicalName[method.ReturnTypeName];
            }
            else
            {
                returnType = method.ReturnTypeName;
            }

            string methodName;
            if (writer.Methods.PhysicalCSharpNamesByLogicalName.ContainsKey(method.Name))
            {
                methodName = writer.Methods.PhysicalCSharpNamesByLogicalName[method.Name];
            }
            else
            {
                methodName = method.Name;
            }

            string argumentsList = CSharpCodeFileSerializer.GetArgumentsList(writer.Types, method.Arguments);

            string[] qualifiers = new string[] { accessibility, isStatic, returnType };
            StringBuilder builder = new StringBuilder();
            foreach (string qualifier in qualifiers)
            {
                if (!String.IsNullOrEmpty(qualifier))
                {
                    builder.Append(qualifier);
                    builder.Append(CSharpConstants.SpaceChar);
                }
            }

            builder.Append(methodName);
            builder.Append(CSharpConstants.CallBeginOperatorChar);
            builder.Append(argumentsList);
            builder.Append(CSharpConstants.CallEndOperatorChar);

            string output = builder.ToString();
            return output;
        }

        private static string GetArgumentsList(CSharpTypes types, List<MethodArgument> arguments)
        {
            List<string> argumentPairs = new List<string>();
            StringBuilder builder = new StringBuilder();
            foreach (MethodArgument argument in arguments)
            {
                string argumentType;
                if (types.PhysicalCSharpNamesByLogicalName.ContainsKey(argument.TypeName))
                {
                    argumentType = types.PhysicalCSharpNamesByLogicalName[argument.TypeName];
                }
                else
                {
                    argumentType = argument.TypeName;
                }

                string passType = argument.PassType.ToDefaultString();

                builder.Clear();
                if (String.Empty != passType)
                {
                    builder.Append(passType);
                    builder.Append(CSharpConstants.SpaceChar);
                }

                builder.Append(argumentType);
                builder.Append(CSharpConstants.SpaceChar);

                builder.Append(argument.Name);

                string argumentPair = builder.ToString();
                argumentPairs.Add(argumentPair);
            }

            string output = StringExtensions.LinearizeTokens(argumentPairs.ToArray(), CSharpConstants.SeriesOperatorChar.ToString());
            return output;
        }

        private static void WriteMember(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Member memberObj = scope.LogicalObject as Member;
            if (null == memberObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteInitializedMember(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            InitializedMember memberObj = scope.LogicalObject as InitializedMember;
            if (null == memberObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WritePropertyMember(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            PropertyMember memberObj = scope.LogicalObject as PropertyMember;
            if (null == memberObj)
            {
                throw new ArgumentException(CSharpCodeFileSerializer.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        #endregion


        protected override void Serialize(CSharpCodeFileSerializationUnit unit)
        {
            this.Serialize(unit.Path, unit.CodeFile);
        }

        public void Serialize(string path, CodeFile codeFile)
        {
            string directoryPath = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryPath);

            CSharpCodeFileSerializer.SerializeStatic(path, codeFile);
        }
    }
}
