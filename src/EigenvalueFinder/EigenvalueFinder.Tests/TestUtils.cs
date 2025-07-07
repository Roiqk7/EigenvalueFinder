using EigenvalueFinder.Core;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

using CoreMatrix = EigenvalueFinder.Core.Matrix;

namespace EigenvalueFinder.Tests;

public static class TestUtils
{
        /// <summary>
        /// Tolerance used for floating point comparisons in assertions.
        /// </summary>
        public static double TOLERANCE = 1e-5;

        /// <summary>
        /// Generates a random complex number with both real and imaginary parts.
        /// </summary>
        /// <returns>A new Complex number with random real and imaginary components.</returns>
        public static Complex GetRandomComplex()
        {
                Random rnd = new Random();
                return new Complex(rnd.NextDouble() * 100 - 50, rnd.NextDouble() * 100 - 50);
        }

        /// <summary>
        /// Generates a random real number (a Complex number with a zero imaginary part).
        /// </summary>
        /// <returns>A new Complex number with a random real component and zero imaginary component.</returns>
        public static Complex GetRandomReal()
        {
                Random rnd = new Random();
                return new Complex(rnd.NextDouble() * 100 - 50, 0);
        }

        /// <summary>
        /// Generates a random purely imaginary number (a Complex number with a zero real part).
        /// </summary>
        /// <returns>A new Complex number with zero real component and a random imaginary component.</returns>
        public static Complex GetRandomImaginary()
        {
                Random rnd = new Random();
                return new Complex(0, rnd.NextDouble() * 100 - 50);
        }

        /// <summary>
        /// Checks if two Complex numbers are approximately equal within a specified tolerance.
        /// </summary>
        /// <param name="expected">The expected Complex number.</param>
        /// <param name="actual">The actual Complex number.</param>
        /// <param name="tolerance">The maximum allowed difference for equality.</param>
        /// <returns>True if the numbers are approximately equal; otherwise, false.</returns>
        public static bool AreApproximatelyEqual(Complex expected, Complex actual, double tolerance)
        {
                return Math.Abs(expected.Real - actual.Real) < tolerance &&
                       Math.Abs(expected.Imaginary - actual.Imaginary) < tolerance;
        }

        /// <summary>
        /// Determines if two Matrix instances are approximately equal by comparing their dimensions and elements.
        /// </summary>
        /// <param name="expected">The expected Matrix.</param>
        /// <param name="actual">The actual Matrix to compare.</param>
        /// <param name="tolerance">The tolerance used for comparing individual Complex elements.</param>
        /// <returns>True if the matrices are approximately equal, false otherwise.</returns>
        public static bool AreMatricesApproximatelyEqual(CoreMatrix expected, CoreMatrix actual, double tolerance)
        {
                if (actual.RowCount != expected.RowCount || actual.ColumnCount != expected.ColumnCount)
                {
                        return false;
                }

                for (int r = 0; r < expected.RowCount; r++)
                {
                        for (int c = 0; c < expected.ColumnCount; c++)
                        {
                                if (!TestUtils.AreApproximatelyEqual(expected[r, c], actual[r, c], tolerance))
                                {
                                        return false;
                                }
                        }
                }
                return true;
        }

        /// <summary>
        /// Determines if the row spaces of two matrices are approximately equivalent.
        /// This function assesses whether the rows of one matrix can be expressed as linear combinations
        /// of the rows of the other matrix, within a specified numerical tolerance.
        /// The approach involves comparing the ranks of the individual matrices and the rank of a
        /// combined matrix formed by vertically stacking the two input matrices.
        /// If the matrices have the same dimensions, the same rank, and the combined matrix also
        /// has that same rank, their row spaces are considered approximately equivalent.
        /// </summary>
        /// <param name="expected">The first matrix whose row space is to be compared.</param>
        /// <param name="actual">The second matrix whose row space is to be compared.</param>
        /// <returns>
        /// <c>true</c> if the row spaces of the two matrices are approximately equivalent;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if either <paramref name="expected"/>
        /// or <paramref name="actual"/> is <c>null</c>.</exception>
        public static bool AreMatricesRowSpaceEquivalent(CoreMatrix expected, CoreMatrix actual)
        {
                if (expected is null)
                {
                        throw new ArgumentNullException(nameof(expected), "Expected matrix cannot be null.");
                }
                if (actual is null)
                {
                        throw new ArgumentNullException(nameof(actual), "Actual matrix cannot be null.");
                }

                if (expected.RowCount != actual.RowCount || expected.ColumnCount != actual.ColumnCount)
                {
                        // Dimensions must be identical for row space equivalence check as implemented
                        return false;
                }

                if (expected.RowCount == 0)
                {
                        return true; // Both are empty, so row spaces are equivalent
                }

                DenseMatrix expectedInternal = expected.GetInternalMatrix();
                DenseMatrix actualInternal = actual.GetInternalMatrix();

                // Using tolerance for rank calculation
                int rankExpected = expectedInternal.Rank();
                int rankActual = actualInternal.Rank();

                if (rankExpected != rankActual)
                {
                        return false;
                }

                DenseMatrix combinedMatrix = DenseMatrix.Create(
                        expected.RowCount + actual.RowCount,
                        expected.ColumnCount,
                        (r, c) => r < expected.RowCount ? expectedInternal[r, c] : actualInternal[r - expected.RowCount, c]
                );

                int rankCombined = combinedMatrix.Rank();

                return rankCombined == rankExpected;
        }


        /// <summary>
        /// Determines if two Matrix instances are approximately equal by comparing their dimensions and elements,
        /// allowing for a sign flip in the elements.
        /// </summary>
        /// <param name="expected">The expected Matrix.</param>
        /// <param name="actual">The actual Matrix to compare.</param>
        /// <param name="tolerance">The tolerance used for comparing individual Complex elements.</param>
        /// <returns>True if the matrices are approximately equal (allowing for sign flip), false otherwise.</returns>
        public static bool AreMatricesApproximatelyEqualAbs(CoreMatrix expected, CoreMatrix actual, double tolerance)
        {
                if (actual.RowCount != expected.RowCount || actual.ColumnCount != expected.ColumnCount)
                {
                        return false;
                }

                for (int r = 0; r < expected.RowCount; r++)
                {
                        for (int c = 0; c < expected.ColumnCount; c++)
                        {
                                Complex expectedVal = expected[r, c];
                                Complex actualVal = actual[r, c];

                                // Check if actual is approximately equal to expected OR approximately equal to -expected
                                bool areEqualIgnoringSign = TestUtils.AreApproximatelyEqual(expectedVal, actualVal, tolerance)
                                                            || TestUtils.AreApproximatelyEqual(expectedVal, -actualVal, tolerance);

                                if (!areEqualIgnoringSign)
                                {
                                        return false;
                                }
                        }
                }
                return true;
        }

        /// <summary>
        /// Asserts that two Matrix instances are approximately equal by comparing their dimensions and elements.
        /// </summary>
        /// <param name="expected">The expected Matrix.</param>
        /// <param name="actual">The actual Matrix to compare.</param>
        /// <param name="tolerance">The tolerance used for comparing individual Complex elements.</param>
        public static void AssertMatricesApproximatelyEqual(CoreMatrix expected, CoreMatrix actual, double tolerance)
        {
                Assert.That(AreMatricesApproximatelyEqual(expected, actual, tolerance),
                        "Matrices are not approximately equal.");
        }

        /// <summary>
        /// Asserts that two Matrix instances are approximately equal by comparing their dimensions and elements,
        /// in absolute value.
        /// </summary>
        /// <param name="expected">The expected Matrix.</param>
        /// <param name="actual">The actual Matrix to compare.</param>
        /// <param name="tolerance">The tolerance used for comparing individual Complex elements.</param>
        public static void AssertMatricesApproximatelyEqualAbs(CoreMatrix expected, CoreMatrix actual, double tolerance)
        {
                Assert.That(AreMatricesApproximatelyEqualAbs(expected, actual, tolerance),
                        "Matrices are not approximately equal (allowing for sign flip).");
        }

        /// <summary>
        /// Extracts the eigenvalues from a list of eigenpairs.
        /// </summary>
        /// <param name="eigenpairs">The list of eigenpairs.</param>
        /// <returns>A list of complex eigenvalues.</returns>
        public static List<Complex> ExtractEigenvalues(List<QRUtils.Eigenpair> eigenpairs)
        {
                return eigenpairs.Select(ep => ep.eigenvalue).ToList();
        }

        /// <summary>
        /// Extracts the eigenvectors from a list of eigenpairs.
        /// </summary>
        /// <param name="eigenpairs">The list of eigenpairs.</param>
        /// <returns>A list of Matrix eigenvectors.</returns>
        public static List<EigenvalueFinder.Core.Matrix> ExtractEigenvectors(List<QRUtils.Eigenpair> eigenpairs)
        {
                return eigenpairs.Select(ep => ep.eigenvector).ToList();
        }

        /// <summary>
        /// Compares the eigenvalues from two lists of eigenpairs as sets, allowing for approximate equality.
        /// Provides detailed error messages if eigenvalues do not match.
        /// </summary>
        /// <param name="eigenvalues1">The first list of eigenvalues.</param>
        /// <param name="eigenvalues2">The second list of eigenvalues.</param>
        /// <param name="tolerance">The tolerance for comparing complex numbers.</param>
        /// <returns>True if the sets of eigenvalues are approximately equal, false otherwise.</returns>
        public static bool AreEigenvaluesApproximatelyEqual(List<Complex> eigenvalues1, List<Complex> eigenvalues2, double tolerance)
        {
                if (eigenvalues1.Count != eigenvalues2.Count)
                {
                        Console.WriteLine($"Eigenvalue count mismatch: List 1 has {eigenvalues1.Count} eigenvalues, List 2 has {eigenvalues2.Count}.");
                        return false;
                }

                // Create mutable copies for tracking matches
                List<Complex> tempEigenvalues2 = new List<Complex>(eigenvalues2);
                bool allMatch = true;

                for (int i = 0; i < eigenvalues1.Count; i++)
                {
                        Complex val1 = eigenvalues1[i];
                        bool foundMatch = false;
                        for (int j = 0; j < tempEigenvalues2.Count; j++)
                        {
                                if (AreApproximatelyEqual(val1, tempEigenvalues2[j], tolerance))
                                {
                                        tempEigenvalues2.RemoveAt(j); // Remove the matched eigenvalue
                                        foundMatch = true;
                                        break;
                                }
                        }
                        if (!foundMatch)
                        {
                                Console.WriteLine($"Eigenvalue '{val1}' at index {i} from first list has no approximate match in the second list.");
                                allMatch = false;
                        }
                }

                if (tempEigenvalues2.Count > 0)
                {
                        foreach (Complex remainingVal in tempEigenvalues2)
                        {
                                Console.WriteLine($"Eigenvalue '{remainingVal}' from second list has no approximate match in the first list (it was not matched).");
                        }
                        allMatch = false;
                }

                return allMatch;
        }

        /// <summary>
        /// Determines if two lists of eigenvectors span the same subspace.
        /// This is achieved by constructing a single matrix from each list of eigenvectors
        /// (where each eigenvector becomes a row in the new matrix) and then checking
        /// if the row spaces of these two constructed matrices are approximately equivalent.
        /// This approach correctly handles cases where eigenvectors might be scaled differently
        /// or presented in a different order, as long as they collectively span the same space.
        /// Provides detailed error messages for mismatches.
        /// </summary>
        /// <param name="eigenvectors1">The first list of eigenvectors. Each eigenvector should be a Matrix object,
        /// typically a column vector (N rows, 1 column) or a row vector (1 row, N columns).</param>
        /// <param name="eigenvectors2">The second list of eigenvectors. Each eigenvector should be a Matrix object,
        /// typically a column vector (N rows, 1 column) or a row vector (1 row, N columns).</param>
        /// <param name="tolerance">The numerical tolerance used for the row space equivalence check.
        /// This accounts for floating-point inaccuracies when determining linear independence and rank.</param>
        /// <returns>
        /// <c>true</c> if the two lists of eigenvectors collectively span approximately the same subspace;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if either <paramref name="eigenvectors1"/>
        /// or <paramref name="eigenvectors2"/> is <c>null</c>.</exception>
        public static bool AreEigenvectorsApproximatelyEqual(List<EigenvalueFinder.Core.Matrix> eigenvectors1, List<EigenvalueFinder.Core.Matrix> eigenvectors2, double tolerance)
        {
                if (eigenvectors1 is null)
                {
                        throw new ArgumentNullException(nameof(eigenvectors1), "First eigenvector list cannot be null.");
                }
                if (eigenvectors2 is null)
                {
                        throw new ArgumentNullException(nameof(eigenvectors2), "Second eigenvector list cannot be null.");
                }

                // If both lists are empty, they trivially span the same (null) space.
                if (eigenvectors1.Count == 0 && eigenvectors2.Count == 0)
                {
                        return true;
                }

                // If one is empty and the other is not, they cannot span the same space.
                if (eigenvectors1.Count == 0 && eigenvectors2.Count > 0)
                {
                        Console.WriteLine("First eigenvector list is empty, but second is not.");
                        return false;
                }
                if (eigenvectors1.Count > 0 && eigenvectors2.Count == 0)
                {
                        Console.WriteLine("Second eigenvector list is empty, but first is not.");
                        return false;
                }

                // Ensure all eigenvectors have consistent dimensions.
                int vectorDimension;
                bool isRowVector;

                // Determine the expected shape from the first eigenvector in the first list
                if (eigenvectors1[0].RowCount == 1)
                {
                        vectorDimension = eigenvectors1[0].ColumnCount;
                        isRowVector = true;
                }
                else if (eigenvectors1[0].ColumnCount == 1)
                {
                        vectorDimension = eigenvectors1[0].RowCount;
                        isRowVector = false;
                }
                else
                {
                        Console.WriteLine($"Eigenvector in first list at index 0 has an invalid shape: {eigenvectors1[0].RowCount}x{eigenvectors1[0].ColumnCount}. Expected 1xN or Nx1.");
                        return false;
                }

                // Validate dimensions for all eigenvectors in the first list
                for (int i = 0; i < eigenvectors1.Count; i++)
                {
                        var vec = eigenvectors1[i];
                        if ((isRowVector && (vec.RowCount != 1 || vec.ColumnCount != vectorDimension))
                            || (!isRowVector && (vec.ColumnCount != 1 || vec.RowCount != vectorDimension)))
                        {
                                Console.WriteLine($"Inconsistent eigenvector dimensions in first list at index {i}. Expected {(isRowVector ? "1x" + vectorDimension : vectorDimension + "x1")}, but found {vec.RowCount}x{vec.ColumnCount}.");
                                return false;
                        }
                }

                // Validate dimensions for all eigenvectors in the second list
                for (int i = 0; i < eigenvectors2.Count; i++)
                {
                        var vec = eigenvectors2[i];
                        if ((isRowVector && (vec.RowCount != 1 || vec.ColumnCount != vectorDimension))
                            || (!isRowVector && (vec.ColumnCount != 1 || vec.RowCount != vectorDimension)))
                        {
                                Console.WriteLine($"Inconsistent eigenvector dimensions in second list at index {i}. Expected {(isRowVector ? "1x" + vectorDimension : vectorDimension + "x1")}, but found {vec.RowCount}x{vec.ColumnCount}.");
                                return false;
                        }
                }

                // Create a single matrix from each list of eigenvectors by stacking them as rows.
                // If the input eigenvectors are column vectors, we need to handle element access appropriately.
                DenseMatrix combinedMatrix1Internal = DenseMatrix.Create(
                        eigenvectors1.Count,
                        vectorDimension,
                        (r, c) => isRowVector ? eigenvectors1[r].GetInternalMatrix()[0, c] : eigenvectors1[r].GetInternalMatrix()[c, 0]
                );
                CoreMatrix combinedMatrix1 = new CoreMatrix(combinedMatrix1Internal);

                DenseMatrix combinedMatrix2Internal = DenseMatrix.Create(
                        eigenvectors2.Count,
                        vectorDimension,
                        (r, c) => isRowVector ? eigenvectors2[r].GetInternalMatrix()[0, c] : eigenvectors2[r].GetInternalMatrix()[c, 0]
                );
                CoreMatrix combinedMatrix2 = new CoreMatrix(combinedMatrix2Internal);

                // Now, check if the row spaces of these two combined matrices are equivalent.
                if (!AreMatricesRowSpaceEquivalent(combinedMatrix1, combinedMatrix2))
                {
                        Console.WriteLine("The row spaces of the combined eigenvector matrices are not equivalent. This suggests the eigenvectors do not span the same subspace.");
                        return false;
                }

                return true;
        }
}
