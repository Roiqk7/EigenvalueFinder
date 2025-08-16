using EigenvalueFinder.Core;
using System.Numerics;
using CoreMatrix = EigenvalueFinder.Core.Matrix;

namespace EigenvalueFinder.Tests;

[TestFixture]
public static class QRSolverTests
{
        const int MAX_MATRIX_SIZE = 32;

        [Test]
        public static void FindEigenpairs_ThrowsArgumentNullException()
        {
                Assert.Throws<ArgumentNullException>(() => QRSolver.FindEigenpairs(null));
        }

        [Test]
        [TestCase(1, 5)]
        [TestCase(5, 1)]
        public static void FindEigenpairs_ThrowsArgumentException(int rows, int columns)
        {
                Assert.Throws<ArgumentException>(() => QRSolver.FindEigenpairs(new Matrix(rows, columns)));
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
        public static void FindEigenpairs_IdentityMatrix_ReturnsCorrectEigenvaluesAndEigenvectors(int size)
        {
                CoreMatrix identityMatrix = CoreMatrix.Identity(size);
                List<QRUtils.Eigenpair> eigenpairs = QRSolver.FindEigenpairs(identityMatrix);

                List<Complex> actualEigenvalues = TestUtils.ExtractEigenvalues(eigenpairs);
                List<Complex> expectedEigenvalues = Enumerable.Repeat(new Complex(1, 0), size).ToList();

                Assert.That(TestUtils.AreEigenvaluesApproximatelyEqual(actualEigenvalues, expectedEigenvalues, TestUtils.TOLERANCE), Is.True, "Eigenvalues for identity matrix are incorrect.");

                List<CoreMatrix> actualEigenvectors = TestUtils.ExtractEigenvectors(eigenpairs);
                List<CoreMatrix> expectedEigenvectors = new List<CoreMatrix>();
                for (int i = 0; i < size; i++)
                {
                        expectedEigenvectors.Add(CoreMatrix.Identity(size, i));
                }
                Assert.That(TestUtils.AreEigenvectorsApproximatelyEqual(actualEigenvectors, expectedEigenvectors, TestUtils.TOLERANCE), Is.True, "Eigenvectors for identity matrix are incorrect.");
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(MAX_MATRIX_SIZE / 4)]
        [TestCase(MAX_MATRIX_SIZE / 2)]
        [TestCase(MAX_MATRIX_SIZE)]
        public static void FindEigenpairs_DiagonalMatrix_ReturnsCorrectEigenvalues(int size)
        {
                CoreMatrix diagonalMatrix = new CoreMatrix(size, size);
                for (int i = 0; i < size; i++)
                {
                        diagonalMatrix[i, i] = TestUtils.GetRandomReal();
                }
                List<QRUtils.Eigenpair> eigenpairs = QRSolver.FindEigenpairs(diagonalMatrix);

                List<Complex> actualEigenvalues = TestUtils.ExtractEigenvalues(eigenpairs);
                List<Complex> expectedEigenvalues = new List<Complex>();
                for (int i = 0; i < size; i++)
                {
                        expectedEigenvalues.Add(diagonalMatrix[i, i]);
                }

                Assert.That(TestUtils.AreEigenvaluesApproximatelyEqual(actualEigenvalues, expectedEigenvalues, TestUtils.TOLERANCE), Is.True, "Eigenvalues for diagonal matrix are incorrect.");

                List<CoreMatrix> actualEigenvectors = TestUtils.ExtractEigenvectors(eigenpairs);
                List<CoreMatrix> expectedEigenvectors = new List<CoreMatrix>();
                for (int i = 0; i < size; i++)
                {
                        expectedEigenvectors.Add(CoreMatrix.Identity(size, i));
                }
                Assert.That(TestUtils.AreEigenvectorsApproximatelyEqual(actualEigenvectors, expectedEigenvectors, TestUtils.TOLERANCE), Is.True, "Eigenvectors for diagonal matrix are incorrect.");
        }

        [Test]
        public static void FindEigenpairs_KnownMatrixWithRealEigenvalues_ReturnsCorrectResult()
        {
                Complex[,] data = { { 4, -2 }, { 1, 1 } };
                CoreMatrix matrix = new CoreMatrix(data);
                List<QRUtils.Eigenpair> eigenpairs = QRSolver.FindEigenpairs(matrix);

                List<Complex> eigenvaluesActual = TestUtils.ExtractEigenvalues(eigenpairs);
                List<Complex> eigenvaluesExpected = new List<Complex> { new Complex(3, 0), new Complex(2, 0) };

                bool allEqual = TestUtils.AreEigenvaluesApproximatelyEqual(eigenvaluesActual, eigenvaluesExpected, TestUtils.TOLERANCE);

                Assert.That(allEqual, Is.True, "Eigenvalues for known real matrix are incorrect.");

                List<CoreMatrix> eigenvectorsActual = TestUtils.ExtractEigenvectors(eigenpairs);
                Complex[,] vec1Data = { { 2 }, { 1 } };
                CoreMatrix eigenvector1Expected = new Matrix(vec1Data);
                Complex[,] vec2Data = { { 1 }, { 1 } };
                CoreMatrix eigenvector2Expected = new Matrix(vec2Data);

                List<CoreMatrix> eigenvectorsExpected = new List<CoreMatrix> { eigenvector1Expected, eigenvector2Expected };
                allEqual = TestUtils.AreEigenvectorsApproximatelyEqual(eigenvectorsActual, eigenvectorsExpected, TestUtils.TOLERANCE);

                Assert.That(allEqual, Is.True, "Eigenvectors for known real matrix are incorrect.");
        }

        [Test]
        public static void FindEigenpairs_KnownMatrixWithComplexEigenvalues_ReturnsCorrectResult()
        {
                Complex[,] data = { { 0, -1 }, { 1, 0 } };
                CoreMatrix matrix = new CoreMatrix(data);
                List<QRUtils.Eigenpair> eigenpairs = QRSolver.FindEigenpairs(matrix);

                List<Complex> eigenvaluesActual = TestUtils.ExtractEigenvalues(eigenpairs);
                List<Complex> eigenvaluesExpected = new List<Complex> { new Complex(0, 1), new Complex(0, -1) };

                bool allEqual = TestUtils.AreEigenvaluesApproximatelyEqual(eigenvaluesActual, eigenvaluesExpected, TestUtils.TOLERANCE);

                Assert.That(allEqual, Is.True, "Eigenvalues for known complex matrix are incorrect.");

                List<CoreMatrix> eigenvectorsActual = TestUtils.ExtractEigenvectors(eigenpairs);

                Complex[,] vec1Data = { { 1 }, { new Complex(0, -1) } };
                CoreMatrix vec1Expected = new CoreMatrix(vec1Data);
                Complex[,] vec2Data = { { 1 }, { new Complex(0, 1) } };
                CoreMatrix vec2Expected = new CoreMatrix(vec2Data);

                List<CoreMatrix> eigenvectorsExpected = new List<CoreMatrix> { vec1Expected, vec2Expected };

                allEqual = TestUtils.AreEigenvectorsApproximatelyEqual(eigenvectorsActual, eigenvectorsExpected, TestUtils.TOLERANCE);

                Assert.That(allEqual, Is.True, "Eigenvectors for known complex matrix are incorrect.");
        }
}
