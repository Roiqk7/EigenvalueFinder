using System.Numerics;

namespace EigenvalueFinder.Core;
/*
public static class QRSolver
{
        // Tolerance for convergence and numerical comparisons
        private const double TOLERANCE = 1e-9;
        private const int MAX_ITERATIONS = 500; // Maximum iterations for the QR algorithm

        /// <summary>
        /// Finds the eigenvalues and eigenvectors of a given square matrix A using the QR algorithm.
        /// </summary>
        /// <param name="A">The input matrix for which to find eigenpairs. This matrix will be modified during the process.</param>
        /// <returns>A list of Eigenpair structs, each containing an eigenvalue and its corresponding eigenvector.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input matrix A is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the input matrix A is not a square matrix.</exception>
        public static List<QRUtils.Eigenpair> FindEigenpairs(Matrix A)
        {
                if (A == null)
                {
                        throw new ArgumentNullException(nameof(A), "Input matrix A cannot be null for finding eigenpairs.");
                }
                if (A.RowCount != A.ColumnCount)
                {
                        throw new ArgumentException("Input matrix must be a square matrix for finding eigenpairs.");
                }

                // 1: A_0 = A, iteration = 0
                int n = A.RowCount;
                Matrix Ak = A.Clone(); // A_k in the algorithm, initialized to A_0 = A
                Matrix QAccumulated = Matrix.Identity(n); // Accumulates the Q matrices for eigenvectors

                int iteration = 0;
                // 2: While not finished, do:
                while (iteration < MAX_ITERATIONS && !IsUpperTriangular(Ak, TOLERANCE))
                {
                        // 3: A_k = Q_k * R_k
                        QRUtils.QR qrDecomposition = QRFinder.getQR(Ak);
                        Matrix Qk = qrDecomposition.Q;
                        Matrix Rk = qrDecomposition.R;

                        // 4: A_{k+1} := R_k * Q_k
                        Ak = Rk * Qk;

                        // Accumulate Q matrices for eigenvectors: QAccumulated := QAccumulated * Q_k
                        QAccumulated = QAccumulated * Qk;

                        // 5: i := i + 1
                        iteration++;
                }

                List<Complex> eigenvalues = GetEigenvalues(Ak);

                // TODO: Find complex eigen values and inverse iteration

                return eigenpairs;
        }
}
*/
