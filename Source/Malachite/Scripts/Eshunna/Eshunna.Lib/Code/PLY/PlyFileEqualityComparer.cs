using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyFileEqualityComparer : IEqualityComparer<PlyFile>
    {
        public PlyFileHeaderEqualityComparer HeaderComparer { get; }
        public ILog Log { get; }


        public PlyFileEqualityComparer(PlyFileHeaderEqualityComparer headerComparer, ILog log)
        {
            this.HeaderComparer = headerComparer;
            this.Log = log;
        }

        public bool Equals(PlyFile x, PlyFile y)
        {
            x.
        }

        public int GetHashCode(PlyFile obj)
        {
            throw new NotImplementedException();
        }
    }
}
