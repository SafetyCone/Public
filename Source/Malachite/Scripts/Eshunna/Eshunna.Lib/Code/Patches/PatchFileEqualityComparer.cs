using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Patches
{
    public class PatchFileEqualityComparer : IEqualityComparer<PatchFile>
    {
        public IEqualityComparer<Patch> PatchComparer { get; }
        public ILog Log { get; }


        public PatchFileEqualityComparer(IEqualityComparer<Patch> patchComparer, ILog log)
        {
            this.PatchComparer = patchComparer;
            this.Log = log;
        }

        public PatchFileEqualityComparer(ILog log)
            : this(new PatchEqualityComparer(log), log)
        {
        }

        public bool Equals(PatchFile x, PatchFile y)
        {
            bool output = true;

            int nPatchesX = x.Patches.Length;
            int nPatchesY = y.Patches.Length;
            bool patchCountEqual = nPatchesX == nPatchesY;
            if(!patchCountEqual)
            {
                output = false;

                string message = $@"Patch counts not equal - x: {nPatchesX.ToString()}, y: {nPatchesY.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iPatch = 0; iPatch < nPatchesX; iPatch++)
                {
                    Patch xPatch = x.Patches[iPatch];
                    Patch yPatch = y.Patches[iPatch];

                    bool patchesEqual = this.PatchComparer.Equals(xPatch, yPatch);
                    if(!patchesEqual)
                    {
                        output = false;

                        string message = $@"Patches not equal - index: {iPatch.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(PatchFile obj)
        {
            throw new NotImplementedException();
        }

    }
}
