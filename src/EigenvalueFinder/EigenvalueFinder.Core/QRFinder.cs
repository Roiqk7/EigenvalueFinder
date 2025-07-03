using EigenvalueFinder.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex; // Use Complex for Math.NET types
using System;
using System.Numerics; // For System.Numerics.Complex

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
        public static QRUtils.QR getQR(EigenvalueFinder.Core.Matrix A)
        {
                if (A == null)
                {
                        throw new ArgumentNullException(nameof(A), "Input matrix A cannot be null for QR decomposition.");
                }

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
                        // Math.NET uses 0-based indexing, so j corresponds to j in the algorithm.
                        Matrix x = new Matrix(m - j, 1);
                        for (int row = j; row < m; row++)
                        {
                                x[row - j, 0] = R[row, j];
                        }

                        // Calculate ||x||_2
                        // For a vector x = [x1, x2, ..., xk]^T, ||x||_2 = sqrt(|x1|^2 + |x2|^2 + ... + |xk|^2)
                        // Using Math.NET's Norm(2) method on the internal DenseMatrix
                        double x_norm_2 = ((DenseMatrix)x.GetInternalMatrix()).Column(0).L2Norm();


                        // e1 is the first standard basis vector of appropriate size (m-j)
                        Matrix e1 = Matrix.Identity(m - j, 0);

                        // Calculate ||x||_2 * e1
                        Matrix x_norm_2_e1 = new Complex(x_norm_2, 0) * e1;

                        // 4. if x != ||x||_2 * e1 then (check for numerical stability, allow for small differences)
                        // We need to check if x is "approximately not equal" to ||x||_2 * e1
                        // A common way is to check if the difference between them is significant.
                        // If x is very close to ||x||_2 * e1, we can skip the Householder reflection.
                        // This handles the case where the vector is already aligned with e1,
                        // meaning it has only one non-zero component at the first position.

                        // Calculate the difference and its norm
                        Matrix diff = x - x_norm_2_e1;
                        double diff_norm = ((DenseMatrix)diff.GetInternalMatrix()).Column(0).L2Norm();

                        // Use a small epsilon for comparison due to floating point arithmetic
                        double epsilon = 1e-9;

                        if (diff_norm > epsilon) // If x is not approximately equal to ||x||_2 * e1
                        {
                                // 5. x := x - ||x||_2 * e1
                                x = x - x_norm_2_e1;

                                // 6. H(x) := I_{m-j+1} - (2 / (x^T * x)) * x * x^T
                                // Calculate x^T * x (dot product of x with itself)
                                // This will be a 1x1 matrix, which can be implicitly converted to Complex
                                Complex x_transpose_x = x.Transpose() * x;

                                // If x_transpose_x is very close to zero, it means x was a zero vector
                                // and the reflection would be undefined.
                                // However, due to the previous 'if' condition, x won't be zero here unless
                                // there's a numerical issue.
                                if (Complex.Abs(x_transpose_x) < epsilon)
                                {
                                        // This case should ideally not be reached if the above diff_norm check is robust.
                                        // If it is, it means x is effectively a zero vector, and H(x) should be Identity.
                                        // For robustness, we can just continue to the next iteration, as Identity * Matrix = Matrix.
                                        continue;
                                }

                                Matrix H_x_term = (new Complex(2, 0) / x_transpose_x) * x * x.Transpose();
                                Matrix H_x = Matrix.Identity(m - j) - H_x_term;

                                // 7. H := (I_{j-1}   0)
                                //         (0       H(x))
                                // Create an identity matrix of size m x m.
                                // Then embed H(x) into the bottom-right corner.
                                Matrix H = Matrix.Identity(m);

                                for (int row = 0; row < H_x.RowCount; row++)
                                {
                                        for (int col = 0; col < H_x.ColumnCount; col++)
                                        {
                                                H[j + row, j + col] = H_x[row, col];
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
