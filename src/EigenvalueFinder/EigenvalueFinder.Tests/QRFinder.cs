using NUnit.Framework;
using EigenvalueFinder.Core;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using CoreMatrix = EigenvalueFinder.Core.Matrix;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class QRFinderTests : TestUtils
{
        [Test]
        public void GetQR_ThrowsArgumentNullException_WhenInputMatrixIsNull()
        {
                Assert.Throws<ArgumentNullException>(() => QRFinder.getQR(null));
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForIdentityMatrix()
        {
                int size = 3;
                CoreMatrix A = CoreMatrix.Identity(size);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(size), qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(size), qr.R, TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForSimpleRealMatrix()
        {
                Complex[,] dataA = {
                        { new Complex(12, 0), new Complex(-51, 0), new Complex(4, 0) },
                        { new Complex(6, 0), new Complex(167, 0), new Complex(-68, 0) },
                        { new Complex(-4, 0), new Complex(24, 0), new Complex(-41, 0) }
                };
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForSimpleComplexMatrix()
        {
                Complex[,] dataA = {
                        { new Complex(1, 1), new Complex(2, 0) },
                        { new Complex(0, 1), new Complex(1, 2) }
                };
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        [Repeat(20)]
        public void GetQR_RandomizedComparisonWithMathNet_SquareMatrices()
        {
                Random rnd = new Random();
                int size = rnd.Next(2, 6);

                Complex[,] dataA = new Complex[size, size];
                for (int r = 0; r < size; r++)
                {
                        for (int c = 0; c < size; c++)
                        {
                                dataA[r, c] = GetRandomComplex();
                        }
                }
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        [Repeat(20)]
        public void GetQR_RandomizedComparisonWithMathNet_RectangularMatrices_Tall()
        {
                Random rnd = new Random();
                int rows = rnd.Next(3, 7);
                int cols = rnd.Next(2, rows);

                Complex[,] dataA = new Complex[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                dataA[r, c] = GetRandomComplex();
                        }
                }
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        [Repeat(20)]
        public void GetQR_RandomizedComparisonWithMathNet_RectangularMatrices_Wide()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 5);
                int cols = rnd.Next(rows + 1, 7);

                Complex[,] dataA = new Complex[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                dataA[r, c] = GetRandomComplex();
                        }
                }
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForZeroMatrix()
        {
                int rows = 3;
                int cols = 4;
                CoreMatrix A = new CoreMatrix(rows, cols);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(rows), qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(new CoreMatrix(rows, cols), qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForSingleColumnMatrix()
        {
                Complex[,] dataA = {
                        { new Complex(3, 0) },
                        { new Complex(4, 0) },
                        { new Complex(0, 0) }
                };
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForSingleRowMatrix()
        {
                Complex[,] dataA = { { new Complex(3, 0), new Complex(4, 0), new Complex(5, 0) } };
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        public void GetQR_ReturnsCorrectQR_ForOneByOneMatrix()
        {
                Complex[,] dataA = { { new Complex(5, 5) } };
                CoreMatrix A = new CoreMatrix(dataA);

                MathNet.Numerics.LinearAlgebra.Complex.Matrix mathNetA = DenseMatrix.OfArray(dataA);
                MathNet.Numerics.LinearAlgebra.Factorization.QR<Complex> mathNetQR = mathNetA.QR();

                CoreMatrix expectedQ = new CoreMatrix((DenseMatrix)mathNetQR.Q);
                CoreMatrix expectedR = new CoreMatrix((DenseMatrix)mathNetQR.R);

                QRUtils.QR qr = QRFinder.getQR(A);

                AssertMatricesApproximatelyEqual(expectedQ, qr.Q, TOLERANCE);
                AssertMatricesApproximatelyEqual(expectedR, qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(A, qr.Q * qr.R, TOLERANCE);
                AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), qr.Q * qr.Q.Transpose(), TOLERANCE);
        }

        [Test]
        [Repeat(20)]
        public void GetQR_QIsOrthogonal()
        {
                Random rnd = new Random();
                int size = rnd.Next(2, 6);

                Complex[,] dataA = new Complex[size, size];
                for (int r = 0; r < size; r++)
                {
                        for (int c = 0; c < size; c++)
                        {
                                dataA[r, c] = GetRandomComplex();
                        }
                }
                CoreMatrix A = new CoreMatrix(dataA);

                QRUtils.QR qr = QRFinder.getQR(A);

                CoreMatrix Q = qr.Q;
                CoreMatrix Q_transpose = Q.Transpose();
                CoreMatrix identity = CoreMatrix.Identity(Q.RowCount);

                AssertMatricesApproximatelyEqual(identity, Q * Q_transpose, TOLERANCE);
                AssertMatricesApproximatelyEqual(identity, Q_transpose * Q, TOLERANCE);
        }

        [Test]
        [Repeat(20)]
        public void GetQR_RIsUpperTriangular()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 6);
                int cols = rnd.Next(2, 6);

                Complex[,] dataA = new Complex[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                dataA[r, c] = GetRandomComplex();
                        }
                }
                CoreMatrix A = new CoreMatrix(dataA);

                QRUtils.QR qr = QRFinder.getQR(A);

                CoreMatrix R = qr.R;

                for (int r = 0; r < R.RowCount; r++)
                {
                        for (int c = 0; c < r; c++)
                        {
                                Assert.That(AreApproximatelyEqual(Complex.Zero, R[r, c], TOLERANCE),
                                        $"Element R[{r},{c}] is not zero. Value: {R[r, c]}");
                        }
                }
        }
}
