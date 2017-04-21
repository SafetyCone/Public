using System;


namespace Public.Common.Lib.Code.Logical
{
    public class CreateAssemblyInfo
    {
        #region Static

        public static EmptyType GetAssemblyInfo(string title, Guid guid)
        {
            EmptyType output = new EmptyType(@"AssemblyInfo");
            output.NamespacesUsed.Add(@"System.Reflection");
            output.NamespacesUsed.Add(@"System.Runtime.CompilerServices");
            output.NamespacesUsed.Add(@"System.Runtime.InteropServices");

            output.Lines.Add(
@"// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly."
                );
            output.Lines.Add(String.Format(@"[assembly: AssemblyTitle(""{0}"")]", title));
            output.Lines.Add(
@"[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct("""")]
[assembly: AssemblyCopyright(""Copyright ©  2017"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM"
                );
            output.Lines.Add(String.Format(@"[assembly: Guid(""{0}"")]", guid.ToString()));
            output.Lines.Add(
@"
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.* "")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]"
                );

            return output;
        }

        #endregion
    }
}
