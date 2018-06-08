using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace MathNetNetCore
{
    /// <summary>
    /// Examples from the website: https://numerics.mathdotnet.com/
    /// </summary>
    public static class WebsiteExamples
    {
        public static void SubMain()
        {
            WebsiteExamples.KernelExample();
        }

        /// <summary>
        /// Kernel (or Nullspace) of a matrix.
        /// </summary>
        public static void KernelExample()
        {
            Matrix<double> A = DenseMatrix.OfArray(new double[,] {
                {1,1,1,1},
                {1,2,3,4},
                {4,3,2,1}});
            Vector<double>[] nullspace = A.Kernel();

            // verify: the following should be approximately (0,0,0)
            var zero = (A * (2 * nullspace[0] - 3 * nullspace[1]));
        }
    }
}
