using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    /// <summary>
    /// Represents the physical structure of a C# code file.
    /// </summary>
    /// <remarks>
    /// Generally, a code file contains a logical type object. Frequently a code file contains multiple types and sometimes a type can be split over multiple files (a partial class).
    /// Thus a logical type can easily be translated to a code file, but the reverse is more difficult and is thus unimplemented.
    /// A code file can be serialized to a path, but the reverse is again more difficult and is thus unimplemented.
    /// </remarks>
    public class CodeFile : CodeFileBase
    {
        #region Static

        public static CodeFile ProcessClass(Class classObj)
        {
            CodeFile codeFile = new CodeFile();

            Header header = new Header();
            codeFile.Header = header;

            header.Usings.Add(UsingDeclaration.System);
            header.Usings.Add(UsingDeclaration.SystemCollectionsGeneric);
            header.Usings.Add(UsingDeclaration.SystemLinq);
            header.Usings.Add(UsingDeclaration.SystemText);
            foreach (LogicalObjectBase member in classObj.Members)
            {
                header.AddNamespacesUsed(member);
            }
            foreach (LogicalObjectBase method in classObj.Methods)
            {
                header.AddNamespacesUsed(method);
            }

            NamespaceScope namespaceScope = new NamespaceScope(classObj.NamespaceName);
            codeFile.Scopes.Add(namespaceScope);

            ScopeBase classScope = new Scope(classObj);
            namespaceScope.Children.Add(classScope);

            CodeFile.AddAllClassMemberAndMethodScopes(classScope, classObj);

            return codeFile;
        }

        private static void AddAllClassMemberAndMethodScopes(ScopeBase classScope, Class classObj)
        {
            foreach (Member member in classObj.Members)
            {
                ScopeBase memberScope = new Scope(member);
                classScope.Children.Add(memberScope);
            }

            foreach (Method method in classObj.Methods)
            {
                ScopeBase methodScope = new Scope(method);
                classScope.Children.Add(methodScope);
            }
        }

        public static CodeFile ProcessEmptyType(EmptyType emptyTypeObj)
        {
            CodeFile codeFile = new CodeFile();

            Header header = new Header();
            codeFile.Header = header;

            header.AddNamespacesUsed(emptyTypeObj);

            codeFile.Scopes.Add(new Scope(emptyTypeObj));

            return codeFile;
        }

        #endregion


        public Header Header { get; set; }


        public CodeFile()
            : base()
        {
        }
    }
}
