using System.Numerics;
using System.Collections.Generic;
using System;

namespace EigenvalueFinder.Core;

public static class QRSolver
{
        private const double TOLERANCE = 1e-9;
        private const int MAX_ITERATIONS = 500;

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

                int n = A.RowCount;
                Matrix Ak = A.Clone();
                Matrix QAccumulated = Matrix.Identity(n);

                int iteration = 0;
                while (iteration < MAX_ITERATIONS && !IsUpperTriangular(Ak, TOLERANCE))
                {
                        QRUtils.QR qrDecomposition = QRFinder.getQR(Ak);
                        Matrix Qk = qrDecomposition.Q;
                        Matrix Rk = qrDecomposition.R;

                        Ak = Rk * Qk;

                        QAccumulated = QAccumulated * Qk;

                        iteration++;
                }

                List<Complex> eigenvalues = GetEigenvalues(Ak);
                List<Matrix> eigenvectors = GetEigenvectors(QAccumulated);

                List<QRUtils.Eigenpair> eigenpairs = new List<QRUtils.Eigenpair>();

                // TODO: Doesnt actually find the eigen pairs, just randomply pairs them together
                for (int i = 0; i < eigenvalues.Count && i < eigenvectors.Count; i++)
                {
                        eigenpairs.Add(new QRUtils.Eigenpair { eigenvalue = eigenvalues[i], eigenvector = eigenvectors[i] });
                }

                return eigenpairs;
        }

        /// <summary>
        /// Extracts the columns of a matrix as individual column vectors (Matrix objects).
        /// Each column of the QAccumulated matrix from the QR algorithm represents an eigenvector.
        /// </summary>
        /// <param name="QAccumulated">The accumulated Q matrix from the QR algorithm.</param>
        /// <returns>A list of Matrix objects, where each Matrix is a column vector representing an eigenvector.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input matrix is null.</exception>
        public static List<Matrix> GetEigenvectors(Matrix QAccumulated)
        {
                if (QAccumulated == null)
                {
                        throw new ArgumentNullException(nameof(QAccumulated), "QAccumulated matrix cannot be null for extracting eigenvectors.");
                }

                List<Matrix> eigenvectors = new List<Matrix>();
                int n = QAccumulated.RowCount;

                for (int j = 0; j < n; j++)
                {
                        Matrix eigenvector = new Matrix(n, 1);
                        for (int i = 0; i < n; i++)
                        {
                                eigenvector[i, 0] = QAccumulated[i, j];
                        }
                        eigenvectors.Add(eigenvector);
                }
                return eigenvectors;
        }

        /// <summary>
        /// Checks if a matrix is upper triangular within a given tolerance.
        /// An upper triangular matrix has all elements below the main diagonal approximately zero.
        /// </summary>
        /// <param name="matrix">The matrix to check.</param>
        /// <param name="tolerance">The tolerance for considering a value as zero.</param>
        /// <returns>True if the matrix is upper triangular, false otherwise.</returns>
        /// TODO: Should also handle 2x2 blocks
        private static bool IsUpperTriangular(Matrix matrix, double tolerance)
        {
                for (int i = 0; i < matrix.RowCount; i++)
                {
                        for (int j = 0; j < i; j++)
                        {
                                if (Complex.Abs(matrix[i, j]) > tolerance)
                                {
                                        return false;
                                }
                        }
                }
                return true;
        }

        /// <summary>
        /// Extracts the eigenvalues from a quasi-upper triangular matrix (Ak after convergence).
        /// This function handles both 1x1 diagonal blocks (real eigenvalues) and 2x2 diagonal blocks
        /// (complex conjugate eigenvalues) that can result from the QR algorithm on real matrices.
        /// </summary>
        /// <param name="quasiUpperTriangularMatrix">The matrix from which to extract eigenvalues.</param>
        /// <returns>A list of Complex numbers representing the eigenvalues.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input matrix is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the input matrix is not square.</exception>
        private static List<Complex> GetEigenvalues(Matrix quasiUpperTriangularMatrix)
        {
                if (quasiUpperTriangularMatrix == null)
                {
                        throw new ArgumentNullException(nameof(quasiUpperTriangularMatrix), "Matrix cannot be null for eigenvalue extraction.");
                }
                if (quasiUpperTriangularMatrix.RowCount != quasiUpperTriangularMatrix.ColumnCount)
                {
                        throw new ArgumentException("Matrix must be square to extract eigenvalues.");
                }

                List<Complex> eigenvalues = new List<Complex>();
                int n = quasiUpperTriangularMatrix.RowCount;
                int i = 0;

                while (i < n)
                {
                        if (i + 1 < n && Complex.Abs(quasiUpperTriangularMatrix[i + 1, i]) > TOLERANCE)
                        {
                                Complex a = quasiUpperTriangularMatrix[i, i];
                                Complex b = quasiUpperTriangularMatrix[i, i + 1];
                                Complex c = quasiUpperTriangularMatrix[i + 1, i];
                                Complex d = quasiUpperTriangularMatrix[i + 1, i + 1];

                                Complex trace = a + d;
                                Complex determinant = (a * d) - (b * c);

                                Complex discriminant = (trace * trace) - (new Complex(4, 0) * determinant);

                                Complex sqrtDiscriminant = Complex.Sqrt(discriminant);

                                Complex lambda1 = (trace + sqrtDiscriminant) / new Complex(2, 0);
                                Complex lambda2 = (trace - sqrtDiscriminant) / new Complex(2, 0);

                                eigenvalues.Add(lambda1);
                                eigenvalues.Add(lambda2);

                                i += 2;
                        }
                        else
                        {
                                eigenvalues.Add(quasiUpperTriangularMatrix[i, i]);
                                i++;
                        }
                }
                return eigenvalues;
        }
}
