using System;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Provides placeholder names for important type names. These are replaced in the physical code file by the actual type name.
    /// </summary>
    public class Types
    {
        public const string ProgramTypeName = @"PROGRAM_TYPE_NAME";
        public const string VoidTypeName = @"VOID_TYPE_NAME"; // Not just 'void' to ensure conversion of logical type names to physical type names.

        public const string StringTypeName = @"STRING_TYPE_NAME";
        public const string StringBoxedTypeName = @"STRING_BOXED_TYPE_NAME";
        public const string StringArrayTypeName = @"STRING_ARRAY_TYPE_NAME";
    }
}
