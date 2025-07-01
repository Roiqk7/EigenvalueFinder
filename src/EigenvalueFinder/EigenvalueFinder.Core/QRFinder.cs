using EigenvalueFinder.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;

namespace EigenvalueFinder.Core;

public class QRFinder
{
        /// <summary>
        /// Performs QR decomposition of a matrix A using the Householder reflections method.
        /// A = Q * R, where Q is an orthogonal matrix and R is an upper triangular matrix.
        /// This implementation follows Algorithm 13.6 for Householder QR decomposition.
        /// </summary>
        /// <param name="A">The input matrix to decompose.</param>
        /// <returns>A tuple containing the orthogonal matrix Q and the upper triangular matrix R.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input matrix A is null.</exception>
        public QRUtils.QR getQR(EigenvalueFinder.Core.Matrix A)
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

                for (int j = 0; j < Math.Min(m, n); j++)
                {
                        // 3: x := R(j : m, j) - Extract the j-th column from row j downwards
                        Vector x = new Vector(m - j);
                        for (int i = j; i < m; i++)
                        {
                                x[i - j] = R[i, j];
                        }

                        // Calculate the L2 norm of x
                        double xNorm = x.L2Norm();

                        // 4: if x != ||x||2 * e1 then (Check if transformation is needed)
                        // This condition checks if the sub-column x is already a multiple of the standard basis vector e1,
                        // meaning elements below the first one are already zero.
                        // If x is [norm, 0, 0, ...], then no reflection is needed.
                        // A more robust check might involve comparing against a small epsilon to account for floating-point inaccuracies.
                        // For simplicity in direct translation, we'll use the conceptual check.
                        // A common way to check this without constructing e1 is to see if x[0] is equal to xNorm and all other elements are zero.
                        // Or more robustly, if the *vector* (x - ||x||2e1) is approximately zero.

                        // To avoid numerical issues and handle cases where x is already aligned,
                        // it's common to choose the sign of the first element to prevent cancellation.
                        double s = (x[0] >= 0) ? 1.0 : -1.0;
                        if (xNorm < 1e-12) // If x is effectively a zero vector, no transformation needed.
                        {
                                continue;
                        }

                        // 5: x := x - ||x||2 * e1 (Construct the Householder vector v)
                        // The algorithm's 'x' becomes the Householder vector 'v' here.
                        // We need a vector `e1` for the current sub-space size (m-j).
                        Vector e1 = new Vector(m - j);
                        e1[0] = 1.0;

                        Vector v = x - (s * xNorm * e1); // This `v` is the Householder vector.

                        // The Householder vector needs to be normalized for H(x) calculation.
                        // In many implementations, 'v' is just scaled by its norm, or by v.DotProduct(v) later.
                        // Let's ensure v is normalized for the formula.
                        double vNormSquared = v.DotProduct(v); // This is v^T * v

                        if (vNormSquared < 1e-12) // If v is zero (x was already aligned with e1), skip transformation.
                        {
                                continue;
                        }

                        // 6: H(x) := I_{m-j+1} - (2 / x^Tx) * xx^T (The Householder reflector in the sub-space)
                        // Note: The algorithm's 'x' at step 6 is the `v` we just computed.
                        // H_sub = I - 2 * v * v^T / (v^T * v)

                        // v*v.Transpose() computes the outer product vv^T, which is a matrix.
                        Matrix vvT = v * v.Transpose(); // Outer product returns a Matrix

                        // Calculate the scalar factor 2 / (v^T * v)
                        double scalarFactor = 2.0 / vNormSquared;

                        // Identity matrix for the sub-space
                        Matrix I_sub = Matrix.Identity(m - j);

                        // Householder sub-matrix H(x)
                        Matrix H_sub = I_sub - (scalarFactor * vvT);

                        // 7: H := ( I_{j-1} 0 ; 0 H(x) ) (Form the full Householder matrix)
                        // This conceptual H matrix is m x m.
                        // In practice, we construct the full H matrix here to perform multiplications.
                        Matrix H_full = Matrix.Identity(m); // Start with m x m identity
                        for (int row = 0; row < H_sub.RowCount; row++)
                        {
                                for (int col = 0; col < H_sub.ColumnCount; col++)
                                {
                                        H_full[j + row, j + col] = H_sub[row, col];
                                }
                        }

                        // 8: R := HR, Q := QH (Apply transformation)
                        R = H_full * R;
                        Q = Q * H_full; // Note: Q is multiplied from the right by H to accumulate Q = H_0 * H_1 * ... * H_k

                        // After each step, R should be accumulating zeros below the diagonal in column j.
                }

                return new QRUtils.QR(Q, R);
        }
}
