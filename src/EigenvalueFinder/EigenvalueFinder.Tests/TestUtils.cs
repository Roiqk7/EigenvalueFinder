using EigenvalueFinder.Core;
using System.Numerics;

namespace EigenvalueFinder.Tests;

public abstract class TestUtils
{
        /// <summary>
        /// Tolerance used for floating point comparisons in assertions.
        /// </summary>
        protected const double TOLERANCE = 1e-4;

        /// <summary>
        /// Generates a random complex number with both real and imaginary parts.
        /// </summary>
        /// <returns>A new Complex number with random real and imaginary components.</returns>
        protected Complex GetRandomComplex()
        {
                Random rnd = new Random();
                return new Complex(rnd.NextDouble() * 100 - 50, rnd.NextDouble() * 100 - 50);
        }

        /// <summary>
        /// Generates a random real number (a Complex number with a zero imaginary part).
        /// </summary>
        /// <returns>A new Complex number with a random real component and zero imaginary component.</returns>
        protected Complex GetRandomReal()
        {
                Random rnd = new Random();
                return new Complex(rnd.NextDouble() * 100 - 50, 0);
        }

        /// <summary>
        /// Generates a random purely imaginary number (a Complex number with a zero real part).
        /// </summary>
        /// <returns>A new Complex number with zero real component and a random imaginary component.</returns>
        protected Complex GetRandomImaginary()
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
        protected bool AreApproximatelyEqual(Complex expected, Complex actual, double tolerance)
        {
                return Math.Abs(expected.Real - actual.Real) < tolerance &&
                       Math.Abs(expected.Imaginary - actual.Imaginary) < tolerance;
        }

        /// <summary>
        /// Asserts that two Matrix instances are approximately equal by comparing their dimensions and elements.
        /// </summary>
        /// <param name="expected">The expected Matrix.</param>
        /// <param name="actual">The actual Matrix to compare.</param>
        /// <param name="tolerance">The tolerance used for comparing individual Complex elements.</param>
        protected void AssertMatricesApproximatelyEqual(Matrix expected, Matrix actual, double tolerance)
        {
                Assert.That(actual.RowCount, Is.EqualTo(expected.RowCount), "Row counts do not match.");
                Assert.That(actual.ColumnCount, Is.EqualTo(expected.ColumnCount), "Column counts do not match.");

                for (int r = 0; r < expected.RowCount; r++)
                {
                        for (int c = 0; c < expected.ColumnCount; c++)
                        {
                                Assert.That(AreApproximatelyEqual(expected[r, c], actual[r, c], tolerance),
                                        $"Elements at ({r},{c}) are not approximately equal. Expected: {expected[r, c]}, Actual: {actual[r, c]}");
                        }
                }
        }
}
