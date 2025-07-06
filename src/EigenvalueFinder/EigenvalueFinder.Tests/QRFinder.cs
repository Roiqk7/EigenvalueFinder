using CoreMatrix = EigenvalueFinder.Core.Matrix;
using EigenvalueFinder.Core;
using System.Numerics;

namespace EigenvalueFinder.Tests;

[TestFixture]
public static class QRFinderTests
{
        const int MAX_MATRIX_SIZE = 64;

        [Test]
        public static void GetQR_NullMatrix_ThrowsArgumentNullException()
        {
                Assert.Throws<ArgumentNullException>(() => QRFinder.getQR( null));
        }

        [Test]
        public static void GetQR_NonSquareMatrix_ThrowsArgumentException()
        {
                CoreMatrix nonSquareMatrix = new CoreMatrix(2, 3);
                Assert.Throws<ArgumentException>(() => QRFinder.getQR(nonSquareMatrix));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(MAX_MATRIX_SIZE / 4)]
        [TestCase(MAX_MATRIX_SIZE / 2)]
        [TestCase(MAX_MATRIX_SIZE)]
        public static void GetQR_IdentityMatrix_ReturnsIdentityQAndIdentityR(int size)
        {
                CoreMatrix identityMatrix = CoreMatrix.Identity(size);
                QRUtils.QR qr = QRFinder.getQR(identityMatrix);

                TestUtils.AssertMatricesApproximatelyEqualAbs(CoreMatrix.Identity(size), qr.Q, TestUtils.TOLERANCE);
                TestUtils.AssertMatricesApproximatelyEqualAbs(identityMatrix, qr.R, TestUtils.TOLERANCE);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(MAX_MATRIX_SIZE / 4)]
        [TestCase(MAX_MATRIX_SIZE / 2)]
        [TestCase(MAX_MATRIX_SIZE)]
        public static void GetQR_DiagonalMatrix_ReturnsIdentityQAndOriginalR(int size)
        {
                Complex[,] data = new Complex[size, size];
                for (int i = 0; i < size; i++)
                {
                        data[i, i] = TestUtils.GetRandomComplex();
                }
                CoreMatrix diagonalMatrix = new CoreMatrix(data);

                QRUtils.QR qr = QRFinder.getQR(diagonalMatrix);

                TestUtils.AssertMatricesApproximatelyEqualAbs(CoreMatrix.Identity(size), qr.Q, TestUtils.TOLERANCE);
                TestUtils.AssertMatricesApproximatelyEqualAbs(diagonalMatrix, qr.R, TestUtils.TOLERANCE);
        }

        [Test]
        [Repeat(10)]
        public static void GetQR_RandomSquareMatrix_DecompositionIsValid()
        {
                int size = new Random().Next(1, MAX_MATRIX_SIZE);
                Complex[,] data = new Complex[size, size];
                for (int r = 0; r < size; r++)
                {
                        for (int c = 0; c < size; c++)
                        {
                                data[r, c] = TestUtils.GetRandomReal();
                        }
                }
                CoreMatrix A = new CoreMatrix(data);

                QRUtils.QR qr = QRFinder.getQR(A);

                CoreMatrix QR = qr.Q * qr.R;
                TestUtils.AssertMatricesApproximatelyEqual(A, QR, TestUtils.TOLERANCE);

                CoreMatrix Q_transpose_Q = qr.Q.Transpose() * qr.Q;
                TestUtils.AssertMatricesApproximatelyEqual(CoreMatrix.Identity(size), Q_transpose_Q, TestUtils.TOLERANCE);

                for (int r = 1; r < size; r++)
                {
                        for (int c = 0; c < r; c++)
                        {
                                Assert.That(TestUtils.AreApproximatelyEqual(Complex.Zero, qr.R[r, c], TestUtils.TOLERANCE),
                                        $"R[{r},{c}] is not zero. Expected: 0, Actual: {qr.R[r, c]}");
                        }
                }
        }

        [Test]
        public static void GetQR_KnownMatrix_ReturnsCorrectQR()
        {
                Complex[,] data = new Complex[,]
                {
                        { new Complex(12, 0), new Complex(-51, 0), new Complex(4, 0) },
                        { new Complex(6, 0), new Complex(167, 0), new Complex(-68, 0) },
                        { new Complex(-4, 0), new Complex(24, 0), new Complex(-41, 0) }
                };
                CoreMatrix A = new CoreMatrix(data);

                QRUtils.QR actualQR = QRFinder.getQR(A);

                // Verify A = QR
                CoreMatrix QR = actualQR.Q * actualQR.R;
                TestUtils.AssertMatricesApproximatelyEqual(A, QR, TestUtils.TOLERANCE);

                // Verify Q is orthogonal (Q^T * Q = I)
                CoreMatrix Q_transpose_Q = actualQR.Q.Transpose() * actualQR.Q;
                TestUtils.AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), Q_transpose_Q, TestUtils.TOLERANCE);

                // Verify R is upper triangular
                for (int r = 1; r < A.RowCount; r++)
                {
                        for (int c = 0; c < r; c++)
                        {
                                Assert.That(TestUtils.AreApproximatelyEqual(Complex.Zero, actualQR.R[r, c], TestUtils.TOLERANCE),
                                        $"R[{r},{c}] is not zero. Expected: 0, Actual: {actualQR.R[r, c]}");
                        }
                }
        }

        [Test]
        public static void GetQR_MatrixWithZeroColumn_HandlesCorrectly()
        {
                Complex[,] data = new Complex[,]
                {
                        { new Complex(1, 0), new Complex(0, 0) },
                        { new Complex(2, 0), new Complex(0, 0) }
                };
                CoreMatrix A = new CoreMatrix(data);

                QRUtils.QR qr = QRFinder.getQR(A);

                CoreMatrix QR = qr.Q * qr.R;
                TestUtils.AssertMatricesApproximatelyEqual(A, QR, TestUtils.TOLERANCE);

                CoreMatrix Q_transpose_Q = qr.Q.Transpose() * qr.Q;
                TestUtils.AssertMatricesApproximatelyEqual(CoreMatrix.Identity(A.RowCount), Q_transpose_Q, TestUtils.TOLERANCE);

                for (int r = 1; r < A.RowCount; r++)
                {
                        for (int c = 0; c < r; c++)
                        {
                                Assert.That(TestUtils.AreApproximatelyEqual(Complex.Zero, qr.R[r, c], TestUtils.TOLERANCE),
                                        $"R[{r},{c}] is not zero. Expected: 0, Actual: {qr.R[r, c]}");
                        }
                }
        }
}
