using System;
using System.Collections.Generic;
using System.Linq;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Match
{
    public class TwoViewGeometryEqualityComparer : IEqualityComparer<TwoViewGeometry>
    {
        public ILog Log { get; }


        public TwoViewGeometryEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(TwoViewGeometry x, TwoViewGeometry y)
        {
            bool output = true;

            bool nfEquals = x.NF == y.NF;
            if(!nfEquals)
            {
                output = false;

                string message = $@"NF mismatch: x: {x.NF.ToString()}, y: {y.NF.ToString()}";
                this.Log.WriteLine(message);
            }

            bool neEquals = x.NE == y.NE;
            if (!neEquals)
            {
                output = false;

                string message = $@"NE mismatch: x: {x.NE.ToString()}, y: {y.NE.ToString()}";
                this.Log.WriteLine(message);
            }

            bool nhEquals = x.NH == y.NH;
            if (!nhEquals)
            {
                output = false;

                string message = $@"NH mismatch: x: {x.NH.ToString()}, y: {y.NH.ToString()}";
                this.Log.WriteLine(message);
            }

            bool nh2Equals = x.NH2 == y.NH2;
            if (!nh2Equals)
            {
                output = false;

                string message = $@"NH2 mismatch: x: {x.NH2.ToString()}, y: {y.NH2.ToString()}";
                this.Log.WriteLine(message);
            }

            bool fEquals = x.F == y.F;
            if (!fEquals)
            {
                output = false;

                string message = $@"F mismatch: x: {x.F.ToString()}, y: {y.F.ToString()}";
                this.Log.WriteLine(message);
            }

            bool rEquals = x.R == y.R;
            if (!rEquals)
            {
                output = false;

                string message = $@"R mismatch: x: {x.R.ToString()}, y: {y.R.ToString()}";
                this.Log.WriteLine(message);
            }

            bool tSizeEquals = x.T.Length == y.T.Length;
            if(!tSizeEquals)
            {
                output = false;

                string message = @"T size mismatch.";
                this.Log.WriteLine(message);
            }
            else
            {
                bool tEquals = x.T.SequenceEqual(y.T);
                if(!tEquals)
                {
                    output = false;

                    string message = @"T mismatch.";
                    this.Log.WriteLine(message);
                }
            }

            bool f1Equals = x.F1 == y.F1;
            if (!f1Equals)
            {
                output = false;

                string message = $@"F1 mismatch: x: {x.F1.ToString()}, y: {y.F1.ToString()}";
                this.Log.WriteLine(message);
            }

            bool f2Equals = x.F2 == y.F2;
            if (!f2Equals)
            {
                output = false;

                string message = $@"F2 mismatch: x: {x.F2.ToString()}, y: {y.F2.ToString()}";
                this.Log.WriteLine(message);
            }

            bool hEquals = x.H == y.H;
            if (!hEquals)
            {
                output = false;

                string message = $@"H mismatch: x: {x.H.ToString()}, y: {y.H.ToString()}";
                this.Log.WriteLine(message);
            }

            bool geEquals = x.GE == y.GE;
            if (!geEquals)
            {
                output = false;

                string message = $@"GE mismatch: x: {x.GE.ToString()}, y: {y.GE.ToString()}";
                this.Log.WriteLine(message);
            }

            bool aaEquals = x.AA == y.AA;
            if (!aaEquals)
            {
                output = false;

                string message = $@"AA mismatch: x: {x.AA.ToString()}, y: {y.AA.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(TwoViewGeometry obj)
        {
            float t1 = obj.T.Length > 0 ? obj.T[0] : 0;

            int output = HashHelper.GetHashCode(obj.R, t1);
            return output;
        }
    }
}
