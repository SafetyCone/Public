using System;
using System.Collections.Generic;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Code.Logical;
using LogicalDelegate = Public.Common.Lib.Code.Logical.Delegate;
using Public.Common.Lib.Code.Physical.CSharp;
using CSharpConstants = Public.Common.Lib.Code.Physical.CSharp.Constants;
using CSharpTypes = Public.Common.Lib.Code.Physical.CSharp.Types;


namespace Public.Common.Lib.Code.Physical
{
    public class SerializeCodeFileToPath : CommandBase
    {
        private const string UnexpectedScopeTypeErrorMessage = @"Unexpected type of scope.";
        private const string UnexpectedLogicalObjectTypeErrorMessage = @"Unexpected type of logical object.";


        #region Static

        private static Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> ScopeWriters { get; set; }
        private static Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> LogicalObjectWriters { get; set; }


        static SerializeCodeFileToPath()
        {
            Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> scopeWriters = new Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>>();
            SerializeCodeFileToPath.ScopeWriters = scopeWriters;

            scopeWriters.Add(typeof(Scope).FullName, SerializeCodeFileToPath.WriteScope);
            scopeWriters.Add(typeof(NamespaceScope).FullName, SerializeCodeFileToPath.WriteNamespaceScope);

            Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>> logicalObjectWriters = new Dictionary<string, Action<CSharpCodeFileWriter, ScopeBase>>();
            SerializeCodeFileToPath.LogicalObjectWriters = logicalObjectWriters;

            logicalObjectWriters.Add(typeof(Class).FullName, SerializeCodeFileToPath.WriteClass);
            logicalObjectWriters.Add(typeof(EmptyType).FullName, SerializeCodeFileToPath.WriteEmptyType);
            logicalObjectWriters.Add(typeof(Struct).FullName, SerializeCodeFileToPath.WriteStruct);
            logicalObjectWriters.Add(typeof(LogicalDelegate).FullName, SerializeCodeFileToPath.WriteDelegate);
            logicalObjectWriters.Add(typeof(Enumeration).FullName, SerializeCodeFileToPath.WriteEnumeration);
            logicalObjectWriters.Add(typeof(Method).FullName, SerializeCodeFileToPath.WriteMethod);
            logicalObjectWriters.Add(typeof(Member).FullName, SerializeCodeFileToPath.WriteMember);
            logicalObjectWriters.Add(typeof(InitializedMember).FullName, SerializeCodeFileToPath.WriteInitializedMember);
            logicalObjectWriters.Add(typeof(PropertyMember).FullName, SerializeCodeFileToPath.WritePropertyMember);
        }

        private static void WriteUsings(CSharpCodeFileWriter writer, IEnumerable<UsingDeclaration> usings)
        {
            List<UsingDeclaration> usingsList = new List<UsingDeclaration>(usings);

            if (null != usings)
            {
                usingsList.Sort();

                foreach (UsingDeclaration usingDeclaration in usingsList)
                {
                    string line = SerializeCodeFileToPath.FormatUsing(usingDeclaration);
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
            foreach(ScopeBase scope in scopes)
            {
                // Determine what type of scope it is.
                string scopeTypeName = scope.GetType().FullName;
                if (SerializeCodeFileToPath.ScopeWriters.ContainsKey(scopeTypeName))
                {
                    SerializeCodeFileToPath.ScopeWriters[scopeTypeName](writer, scope);
                }
                else
                {
                    throw new ArgumentException(SerializeCodeFileToPath.UnexpectedScopeTypeErrorMessage);
                }
            }
        }

        private static void WriteScope(CSharpCodeFileWriter writer, ScopeBase scopeBase)
        {
            Scope scope = scopeBase as Scope;
            if(null == scope)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedScopeTypeErrorMessage);
            }

            // Determine what type of logical object we have.
            string logicalObjectTypeName = scope.LogicalObject.GetType().FullName;
            if(SerializeCodeFileToPath.LogicalObjectWriters.ContainsKey(logicalObjectTypeName))
            {
                SerializeCodeFileToPath.LogicalObjectWriters[logicalObjectTypeName](writer, scope);
            }
            else
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }
        }

        private static void WriteNamespaceScope(CSharpCodeFileWriter writer, ScopeBase scopeBase)
        {
            NamespaceScope namespaceScope = scopeBase as NamespaceScope;
            if (null == namespaceScope)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedScopeTypeErrorMessage);
            }

            string line = String.Format(@"{0} {1}", CSharpConstants.NamespaceKeyword, namespaceScope.Name);
            writer.WriteLine(line);
            writer.OpenScope();

            SerializeCodeFileToPath.WriteScopes(writer, namespaceScope.Children);

            writer.CloseScope();
        }

        private static void WriteEmptyType(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            EmptyType emptyTypeObj = scope.LogicalObject as EmptyType;
            if (null == emptyTypeObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }

            foreach(string line in emptyTypeObj.Lines)
            {
                writer.WriteLine(line);
            }
        }

        private static void WriteClass(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Class classObj = scope.LogicalObject as Class;
            if (null == classObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }

            string line = SerializeCodeFileToPath.GetClassSignature(writer, classObj);
            writer.WriteIndentedLine(line);
            writer.OpenScope();

            SerializeCodeFileToPath.WriteScopes(writer, scope.Children);

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

            string inheritanceChain = SerializeCodeFileToPath.GetClassInheritanceChain(classObj);

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
            string interfaceInheritanceChain = SerializeCodeFileToPath.GetClassInterfaceInheritanceChain(classObj);

            string inheritanceChain;
            if (String.IsNullOrEmpty(classObj.BaseClassTypeName))
            {
                if(String.IsNullOrEmpty(interfaceInheritanceChain))
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
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteDelegate(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            LogicalDelegate delegateObj = scope.LogicalObject as LogicalDelegate;
            if (null == delegateObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteEnumeration(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Enumeration enumerationObj = scope.LogicalObject as Enumeration;
            if (null == enumerationObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteMethod(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            Method methodObj = scope.LogicalObject as Method;
            if (null == methodObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }

            string methodSignature = SerializeCodeFileToPath.GetMethodSignature(writer, methodObj);

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
            if(method.IsStatic)
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

            string argumentsList = SerializeCodeFileToPath.GetArgumentsList(writer.Types, method.Arguments);

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
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WriteInitializedMember(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            InitializedMember memberObj = scope.LogicalObject as InitializedMember;
            if (null == memberObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        private static void WritePropertyMember(CSharpCodeFileWriter writer, ScopeBase scope)
        {
            PropertyMember memberObj = scope.LogicalObject as PropertyMember;
            if (null == memberObj)
            {
                throw new ArgumentException(SerializeCodeFileToPath.UnexpectedLogicalObjectTypeErrorMessage);
            }


        }

        #endregion


        public string Path { get; set; }
        public CodeFile CodeFile { get; set; }


        public SerializeCodeFileToPath()
        {
        }

        public SerializeCodeFileToPath(string path, CodeFile codeFile)
        {
            this.Path = path;
            this.CodeFile = codeFile;
        }

        public override void Run()
        {
            using (CSharpCodeFileWriter writer = new CSharpCodeFileWriter(this.Path))
            {
                SerializeCodeFileToPath.WriteUsings(writer, this.CodeFile.Header.Usings);
                SerializeCodeFileToPath.WriteScopes(writer, this.CodeFile.Scopes);
            }
        }
    }
}
