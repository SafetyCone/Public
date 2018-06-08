using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace MathNetNetCore
{
    public static class LinearAlgebraTests
    {
        public static void SubMain()
        {
            LinearAlgebraTests.TestMatrixCreation();
        }

        private static void TestMatrixCreation()
        {
            double[] sizeNine = new double[9];

            Matrix<double> threeByThree = new DenseMatrix(3, 3, sizeNine);
        }
    }
}
