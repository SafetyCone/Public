using System;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Create the logical structure for the default console program class which includes the entrypoint main method.
    /// </summary>
    public class CreateConsoleProgramClass : ParentCommandBase
    {
        #region Static

        public static Class CreateProgram(string namespaceName)
        {
            Class program = new Class(Types.ProgramTypeName, namespaceName, Accessibility.Private);

            Method main = CreateConsoleProgramClass.CreateMain();
            program.Methods.Add(main);

            return program;
        }

        public static Method CreateMain()
        {
            MethodArgument args = new MethodArgument(Types.StringArrayTypeName, @"args");

            Method main = Method.NewStaticMethod(Methods.MainMethodName, Types.VoidTypeName, Accessibility.Private, new MethodArgument[] { args });
            return main;
        }

        #endregion


        public string NamespaceName { get; set; }
        public Class Class { get; set; }
        

        public CreateConsoleProgramClass(string namespaceName)
        {
            this.NamespaceName = namespaceName;
        }

        public override void Run()
        {
            this.Class = CreateConsoleProgramClass.CreateProgram(this.NamespaceName);  
        }
    }
}
