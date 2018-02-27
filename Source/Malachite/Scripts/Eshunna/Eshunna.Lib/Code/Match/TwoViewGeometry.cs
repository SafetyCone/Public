using System;


namespace Eshunna.Lib.Match
{
    public class TwoViewGeometry
    {
        public int NF { get; set; }
        public int NE { get; set; }
        public int NH { get; set; }
        public int NH2 { get; set; }
        public MatrixFloat F { get; set; }
        public MatrixFloat R { get; set; }
        public float[] T { get; set; }
        public float F1 { get; set; }
        public float F2 { get; set; }
        public MatrixFloat H { get; set; }
        /// <summary>
        /// Geometric Error.
        /// </summary>
        public float GE { get; set; }
        /// <summary>
        /// Triangulation error.
        /// </summary>
        public float AA { get; set; }


        public TwoViewGeometry(int nf, int ne, int nh, int nh2, MatrixFloat f, MatrixFloat r, float[] t, float f1, float f2, MatrixFloat h, float ge, float aa)
        {
            this.NF = nf;
            this.NE = ne;
            this.NH = nh;
            this.NH2 = nh2;
            this.F = f;
            this.R = r;
            this.T = t;
            this.F1 = f1;
            this.F2 = f2;
            this.H = h;
            this.GE = ge;
            this.AA = aa;
        }

        public TwoViewGeometry()
        {
        }
    }
}
