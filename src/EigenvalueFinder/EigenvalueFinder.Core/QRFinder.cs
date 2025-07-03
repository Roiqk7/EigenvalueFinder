using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;

namespace EigenvalueFinder.Core;

public static class QRFinder
{
        /// <summary>
        /// Performs QR decomposition of a matrix A using the Householder reflections method.
        /// A = QR, where Q is an orthogonal matrix and R is an upper triangular matrix.
        /// This implementation follows Algorithm for Householder QR decomposition.
        /// </summary>
        /// <param name="A">The input matrix to decompose.</param>
        /// <returns>A tuple containing the orthogonal matrix Q and the upper triangular matrix R.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input matrix A is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the input matrix A is not a square matrix.</exception>
        public static QRUtils.QR getQR(EigenvalueFinder.Core.Matrix A)
        {
                if (A == null)
                {
                        throw new ArgumentNullException(nameof(A), "Input matrix A cannot be null for QR decomposition.");
                }

                if (A.RowCount != A.ColumnCount)
                {
                        throw new ArgumentException("Input matrix must be a square matrix for QR decomposition.");
                }

                // Use a small TOLERANCE for comparison due to floating point arithmetic
                double TOLERANCE = 1e-9;

                // 1. Q := Im, R := A
                int m = A.RowCount;
                int n = A.ColumnCount;

                Matrix Q = Matrix.Identity(m);
                Matrix R = A.Clone();

                // 2. for j := 1 to min(m, n) do
                for (int j = 0; j < Math.Min(m, n); j++)
                {
                        // 3. x := R(j : m, j)
                        // Extract the sub-vector from R. This will be a column vector.
                        Matrix x = new Matrix(m - j, 1);
                        for (int row = j; row < m; row++)
                        {
                                x[row - j, 0] = R[row, j];
                        }

                        // Calculate ||x||_2
                        double xNorm2 = ((DenseMatrix)x.GetInternalMatrix()).Column(0).L2Norm();

                        Complex alphaVal;
                        if (x[0, 0].Equals(Complex.Zero))
                        {
                                alphaVal = new Complex(xNorm2, 0); // If x[0,0] is zero, use positive norm
                        }
                        else
                        {
                                alphaVal = (x[0, 0] / Complex.Abs(x[0, 0])) * new Complex(xNorm2, 0);
                        }
                        Complex alpha = -alphaVal;


                        // e1 is the first standard basis vector of appropriate size (m-j)
                        Matrix e1 = Matrix.Identity(m - j, 0);

                        // Calculate alpha * e1
                        Matrix alpha_e1 = alpha * e1;

                        // 4. if x != alpha * e1 then (check for numerical stability, allow for small differences)
                        // If x is very close to alpha * e1, we can skip the Householder reflection.
                        Matrix diff = x - alpha_e1;
                        double diff_norm = ((DenseMatrix)diff.GetInternalMatrix()).Column(0).L2Norm();

                        if (diff_norm > TOLERANCE) // If x is not approximately equal to alpha * e1
                        {
                                // 5. x := x - alpha * e1
                                x = x - alpha_e1;

                                // 6. H(x) := I_{m-j+1} - (2 / (x^T * x)) * x * x^T
                                Complex xTx = x.Transpose() * x;

                                // If xTx is very close to zero, it means x was a zero vector,
                                // and the reflection would be undefined.
                                if (Complex.Abs(xTx) < TOLERANCE)
                                {
                                        continue;
                                }

                                Matrix HxTerm = (new Complex(2, 0) / xTx) * x * x.Transpose();
                                Matrix Hx = Matrix.Identity(m - j) - HxTerm;

                                // 7. H := (I_{j-1}   0)
                                //         (0       H(x))
                                // Create an identity matrix of size m x m.
                                // Then embed H(x) into the bottom-right corner.
                                Matrix H = Matrix.Identity(m);

                                for (int row = 0; row < Hx.RowCount; row++)
                                {
                                        for (int col = 0; col < Hx.ColumnCount; col++)
                                        {
                                                H[j + row, j + col] = Hx[row, col];
                                        }
                                }

                                // 8. R := HR, Q := QH
                                R = H * R;
                                Q = Q * H;
                        }
                }

                return new QRUtils.QR(Q, R);
        }
}
