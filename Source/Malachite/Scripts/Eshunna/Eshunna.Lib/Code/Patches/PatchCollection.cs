using System;


namespace Eshunna.Lib.Patches
{
    public class PatchCollection
    {
        public Patch[] Patches { get; }


        public PatchCollection(Patch[] patches)
        {
            this.Patches = patches;
        }
    }
}
