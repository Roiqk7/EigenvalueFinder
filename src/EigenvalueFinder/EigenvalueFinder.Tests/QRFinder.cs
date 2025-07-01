using NUnit.Framework;
using EigenvalueFinder.Core;
using System;
using System.Text;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class QRFinderTests
{
        private const double Tolerance = 1e-9;

        private bool AreMatricesApproximatelyEqual(Matrix m1, Matrix m2, double tolerance)
        {
                if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
                {
                        return false;
                }

                for (int r = 0; r < m1.RowCount; r++)
                {
                        for (int c = 0; c < m1.ColumnCount; c++)
                        {
                                if (Math.Abs(m1[r, c] - m2[r, c]) > tolerance)
                                {
                                        return false;
                                }
                        }
                }
                return true;
        }

        private bool IsOrthogonal(Matrix Q, double tolerance)
        {
                if (Q.RowCount != Q.ColumnCount)
                {
                        return false; // Only square matrices can be orthogonal
                }

                Matrix Q_transpose = Q.Transpose();
                Matrix product = Q * Q_transpose;
                Matrix identity = Matrix.Identity(Q.RowCount);

                return AreMatricesApproximatelyEqual(product, identity, tolerance);
        }

        private bool IsUpperTriangular(Matrix R, double tolerance)
        {
                for (int r = 0; r < R.RowCount; r++)
                {
                        for (int c = 0; c < r; c++) // Iterate through elements below the main diagonal
                        {
                                if (Math.Abs(R[r, c]) > tolerance)
                                {
                                        return false;
                                }
                        }
                }
                return true;
        }

        [Test]
        public void GetQR_ThrowsOnNullMatrix()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = null;

                Assert.Throws<ArgumentNullException>(() => qrFinder.getQR(A));
        }

        [Test]
        public void GetQR_SquareMatrix_PerformsCorrectDecomposition()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 12, -51, 4 },
                        { 6, 167, -68 },
                        { -4, 24, -41 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // Verify Q is orthogonal
                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");

                // Verify R is upper triangular
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");

                // Verify A = Q * R
                Matrix reconstructedA = Q * R;
                Assert.That(AreMatricesApproximatelyEqual(A, reconstructedA, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_RectangularMatrix_MoreRowsThanCols_PerformsCorrectDecomposition()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 1, 2 },
                        { 3, 4 },
                        { 5, 6 }
                }); // 3x2 matrix

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // Verify Q is orthogonal (Q will be 3x3)
                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");

                // Verify R is upper triangular (R will be 3x2)
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                // Ensure elements below the diagonal of R are zero
                Assert.That(R[2, 0], Is.EqualTo(0.0).Within(Tolerance));
                Assert.That(R[2, 1], Is.EqualTo(0.0).Within(Tolerance));


                // Verify A = Q * R
                Matrix reconstructedA = Q * R;
                Assert.That(AreMatricesApproximatelyEqual(A, reconstructedA, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_RectangularMatrix_MoreColsThanRows_PerformsCorrectDecomposition()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 1, 2, 3 },
                        { 4, 5, 6 }
                }); // 2x3 matrix

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // Verify Q is orthogonal (Q will be 2x2)
                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");

                // Verify R is upper triangular (R will be 2x3)
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                // For a 2x3 matrix, R[1,0] should be zero.
                Assert.That(R[1, 0], Is.EqualTo(0.0).Within(Tolerance));


                // Verify A = Q * R
                Matrix reconstructedA = Q * R;
                Assert.That(AreMatricesApproximatelyEqual(A, reconstructedA, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_IdentityMatrix()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = Matrix.Identity(3);

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // For an identity matrix, Q should be identity and R should be identity
                Assert.That(AreMatricesApproximatelyEqual(Q, Matrix.Identity(3), Tolerance), Is.True, "Q should be identity for an identity input matrix.");
                Assert.That(AreMatricesApproximatelyEqual(R, Matrix.Identity(3), Tolerance), Is.True, "R should be identity for an identity input matrix.");
        }

        [Test]
        public void GetQR_DiagonalMatrix()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 5, 0, 0 },
                        { 0, 10, 0 },
                        { 0, 0, 15 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // For a diagonal matrix, Q should be identity and R should be the original matrix (or its absolute values depending on implementation)
                // Householder reflections might introduce sign changes, but the core decomposition should hold.
                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q should be orthogonal for a diagonal input matrix.");
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R should be upper triangular for a diagonal input matrix.");
                Assert.That(AreMatricesApproximatelyEqual(A, Q * R, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_AlreadyUpperTriangularMatrix()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 1, 2, 3 },
                        { 0, 4, 5 },
                        { 0, 0, 6 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                // If A is already upper triangular, Q should be identity and R should be A
                Assert.That(AreMatricesApproximatelyEqual(Q, Matrix.Identity(3), Tolerance), Is.True, "Q should be identity for an already upper triangular input matrix.");
                Assert.That(AreMatricesApproximatelyEqual(R, A, Tolerance), Is.True, "R should be equal to A for an already upper triangular input matrix.");
        }

        [Test]
        public void GetQR_MatrixWithZeros()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 0, 1, 0 },
                        { 0, 0, 1 },
                        { 1, 0, 0 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                Assert.That(AreMatricesApproximatelyEqual(A, Q * R, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_SmallMatrix_2x2()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { 1, 1 },
                        { 1, 0 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                Assert.That(AreMatricesApproximatelyEqual(A, Q * R, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_MatrixWithNegativeValues()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] {
                        { -1, -2 },
                        { -3, -4 }
                });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                Assert.That(AreMatricesApproximatelyEqual(A, Q * R, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }

        [Test]
        public void GetQR_SingleElementMatrix()
        {
                QRFinder qrFinder = new QRFinder();
                Matrix A = new Matrix(new double[,] { { 5.0 } });

                QRUtils.QR qrResult = qrFinder.getQR(A);
                Matrix Q = qrResult.Q;
                Matrix R = qrResult.R;

                Assert.That(IsOrthogonal(Q, Tolerance), Is.True, "Q matrix should be orthogonal.");
                Assert.That(IsUpperTriangular(R, Tolerance), Is.True, "R matrix should be upper triangular.");
                Assert.That(AreMatricesApproximatelyEqual(A, Q * R, Tolerance), Is.True, "A should be approximately equal to Q * R.");
        }
}
