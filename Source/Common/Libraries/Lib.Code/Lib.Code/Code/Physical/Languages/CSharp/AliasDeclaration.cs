using System;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    public class AliasDeclaration : UsingDeclaration
    {
        public string Alias { get; set; }


        public AliasDeclaration(string alias, string namespaceName)
            : base(namespaceName)
        {
            this.Alias = alias;
        }

        public override string ToString()
        {
            string output = String.Format(@"{0} = {1}", this.Alias, this.NamespaceName);
            return output;
        }
    }
}
