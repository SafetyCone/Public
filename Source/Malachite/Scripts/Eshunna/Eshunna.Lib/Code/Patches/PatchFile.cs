using System;


namespace Eshunna.Lib.Patches
{
    public class PatchFile
    {
        public Patch[] Patches { get; }


        public PatchFile(Patch[] patches)
        {
            this.Patches = patches;
        }
    }
}
