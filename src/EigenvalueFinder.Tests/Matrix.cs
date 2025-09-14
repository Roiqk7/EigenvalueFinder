using CoreMatrix = EigenvalueFinder.Core.Matrix; 
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace EigenvalueFinder.Tests;

[TestFixture]
public static class MatrixTests
{
        [Test]
        public static void Constructor_Dimensions_CreatesZeroMatrix()
        {
                int rows = 3;
                int cols = 4;

                CoreMatrix matrix = new CoreMatrix(rows, cols);

                Assert.That(matrix.RowCount, Is.EqualTo(rows), "RowCount mismatch.");
                Assert.That(matrix.ColumnCount, Is.EqualTo(cols), "ColumnCount mismatch.");
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                Assert.That(matrix[r, c], Is.EqualTo(Complex.Zero), $"Element at ({r},{c}) is not zero.");
                        }
                }
        }

        [Test]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        [TestCase(-1, 5)]
        [TestCase(5, -1)]
        public static void Constructor_Dimensions_ThrowsArgumentOutOfRangeExceptionForNonPositive(int rows, int cols)
        {
                Assert.Throws<ArgumentOutOfRangeException>(() => new CoreMatrix(rows, cols));
        }

        [Test]
        public static void Constructor_Complex2DArray_CreatesCorrectMatrix_RealNumbers()
        {
                Complex[,] data = {
                        { new Complex(1, 0), new Complex(2, 0) },
                        { new Complex(3, 0), new Complex(4, 0) }
                };

                CoreMatrix matrix = new CoreMatrix(data);

                Assert.That(matrix.RowCount, Is.EqualTo(2));
                Assert.That(matrix.ColumnCount, Is.EqualTo(2));
                Assert.That(matrix[0, 0], Is.EqualTo(new Complex(1, 0)));
                Assert.That(matrix[0, 1], Is.EqualTo(new Complex(2, 0)));
                Assert.That(matrix[1, 0], Is.EqualTo(new Complex(3, 0)));
                Assert.That(matrix[1, 1], Is.EqualTo(new Complex(4, 0)));
        }

        [Test]
        public static void Constructor_Complex2DArray_CreatesCorrectMatrix_ComplexNumbers()
        {
                Complex[,] data = {
                        { new Complex(1, 1), new Complex(2, 2) },
                        { new Complex(3, 3), new Complex(4, 4) }
                };

                CoreMatrix matrix = new CoreMatrix(data);

                Assert.That(matrix.RowCount, Is.EqualTo(2));
                Assert.That(matrix.ColumnCount, Is.EqualTo(2));
                Assert.That(matrix[0, 0], Is.EqualTo(new Complex(1, 1)));
                Assert.That(matrix[0, 1], Is.EqualTo(new Complex(2, 2)));
                Assert.That(matrix[1, 0], Is.EqualTo(new Complex(3, 3)));
                Assert.That(matrix[1, 1], Is.EqualTo(new Complex(4, 4)));
        }

        [Test]
        public static void Constructor_Complex2DArray_CreatesCorrectMatrix_MixedNumbers()
        {
                Complex[,] data = {
                        { new Complex(1, 0), new Complex(2, 2) },
                        { new Complex(3, 3), new Complex(4, 0) }
                };

                CoreMatrix matrix = new CoreMatrix(data);

                Assert.That(matrix.RowCount, Is.EqualTo(2));
                Assert.That(matrix.ColumnCount, Is.EqualTo(2));
                Assert.That(matrix[0, 0], Is.EqualTo(new Complex(1, 0)));
                Assert.That(matrix[0, 1], Is.EqualTo(new Complex(2, 2)));
                Assert.That(matrix[1, 0], Is.EqualTo(new Complex(3, 3)));
                Assert.That(matrix[1, 1], Is.EqualTo(new Complex(4, 0)));
        }

        [Test]
        public static void Constructor_Complex2DArray_ThrowsArgumentNullExceptionForNullData()
        {
                Complex[,] data = null;

                Assert.Throws<ArgumentNullException>(() => new CoreMatrix(data));
        }

        [Test]
        public static void Constructor_DenseMatrix_WrapsCorrectly()
        {
                DenseMatrix denseMatrix = DenseMatrix.Create(2, 2, (r, c) => new Complex(r + c, r - c));

                CoreMatrix matrix = new CoreMatrix(denseMatrix);

                Assert.That(matrix.RowCount, Is.EqualTo(denseMatrix.RowCount));
                Assert.That(matrix.ColumnCount, Is.EqualTo(denseMatrix.ColumnCount));
                for (int r = 0; r < matrix.RowCount; r++)
                {
                        for (int c = 0; c < matrix.ColumnCount; c++)
                        {
                                Assert.That(matrix[r, c], Is.EqualTo(denseMatrix[r, c]));
                        }
                }
        }

        [Test]
        public static void Constructor_DenseMatrix_ThrowsArgumentNullExceptionForNullDenseMatrix()
        {
                DenseMatrix denseMatrix = null;

                Assert.Throws<ArgumentNullException>(() => new CoreMatrix(denseMatrix));
        }

        [Test]
        public static void Identity_Size_CreatesCorrectIdentityMatrix_SmallSize()
        {
                int size = 3;
                Complex[,] expectedData = {
                        { Complex.One, Complex.Zero, Complex.Zero },
                        { Complex.Zero, Complex.One, Complex.Zero },
                        { Complex.Zero, Complex.Zero, Complex.One }
                };
                CoreMatrix expectedMatrix = new CoreMatrix(expectedData);

                CoreMatrix identityMatrix = CoreMatrix.Identity(size);

                TestUtils.AssertMatricesApproximatelyEqual(expectedMatrix, identityMatrix, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Identity_Size_CreatesCorrectIdentityMatrix_LargerSize()
        {
                int size = 5;
                DenseMatrix expectedDenseMatrix = DenseMatrix.CreateIdentity(size);
                CoreMatrix expectedMatrix = new CoreMatrix(expectedDenseMatrix);

                CoreMatrix identityMatrix = CoreMatrix.Identity(size);

                TestUtils.AssertMatricesApproximatelyEqual(expectedMatrix, identityMatrix, TestUtils.TOLERANCE);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public static void Identity_Size_ThrowsArgumentOutOfRangeExceptionForNonPositiveSize(int size)
        {
                Assert.Throws<ArgumentOutOfRangeException>(() => CoreMatrix.Identity(size));
        }

        [Test]
        public static void Identity_SizeIndex_CreatesCorrectStandardBasisVector_FirstElement()
        {
                int size = 4;
                int index = 0;
                Complex[,] expectedData = {
                        { Complex.One },
                        { Complex.Zero },
                        { Complex.Zero },
                        { Complex.Zero }
                };
                CoreMatrix expectedVector = new CoreMatrix(expectedData);

                CoreMatrix basisVector = CoreMatrix.Identity(size, index);

                TestUtils.AssertMatricesApproximatelyEqual(expectedVector, basisVector, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Identity_SizeIndex_CreatesCorrectStandardBasisVector_MiddleElement()
        {
                int size = 4;
                int index = 2;
                Complex[,] expectedData = {
                        { Complex.Zero },
                        { Complex.Zero },
                        { Complex.One },
                        { Complex.Zero }
                };
                CoreMatrix expectedVector = new CoreMatrix(expectedData);

                CoreMatrix basisVector = CoreMatrix.Identity(size, index);

                TestUtils.AssertMatricesApproximatelyEqual(expectedVector, basisVector, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Identity_SizeIndex_CreatesCorrectStandardBasisVector_LastElement()
        {
                int size = 4;
                int index = 3;
                Complex[,] expectedData = {
                        { Complex.Zero },
                        { Complex.Zero },
                        { Complex.Zero },
                        { Complex.One }
                };
                CoreMatrix expectedVector = new CoreMatrix(expectedData);

                CoreMatrix basisVector = CoreMatrix.Identity(size, index);

                TestUtils.AssertMatricesApproximatelyEqual(expectedVector, basisVector, TestUtils.TOLERANCE);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(5, -1)]
        [TestCase(5, 5)]
        public static void Identity_SizeIndex_ThrowsArgumentOutOfRangeExceptionForInvalidArgs(int size, int index)
        {
                Assert.Throws<ArgumentOutOfRangeException>(() => CoreMatrix.Identity(size, index));
        }

        [Test]
        public static void Indexer_GetAndSet_WorksCorrectly_RealNumbers()
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);
                Complex value = new Complex(10, 0);

                matrix[0, 0] = value;
                Complex retrievedValue = matrix[0, 0];

                Assert.That(retrievedValue, Is.EqualTo(value));
        }

        [Test]
        public static void Indexer_GetAndSet_WorksCorrectly_ComplexNumbers()
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);
                Complex value = new Complex(5, -3);

                matrix[1, 1] = value;
                Complex retrievedValue = matrix[1, 1];

                Assert.That(retrievedValue, Is.EqualTo(value));
        }

        [Test]
        public static void Indexer_GetAndSet_WorksCorrectly_MixedNumbers()
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);
                Complex realValue = new Complex(10, 0);
                Complex complexValue = new Complex(5, -3);

                matrix[0, 0] = realValue;
                matrix[0, 1] = complexValue;
                Complex retrievedReal = matrix[0, 0];
                Complex retrievedComplex = matrix[0, 1];

                Assert.That(retrievedReal, Is.EqualTo(realValue));
                Assert.That(retrievedComplex, Is.EqualTo(complexValue));
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(2, 0)]
        [TestCase(0, 2)]
        public static void Indexer_Get_ThrowsArgumentOutOfRangeException(int row, int col) 
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentOutOfRangeException>(() => { var val = matrix[row, col]; });
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(2, 0)]
        [TestCase(0, 2)]
        public static void Indexer_Set_ThrowsArgumentOutOfRangeException(int row, int col)
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentOutOfRangeException>(() => { matrix[row, col] = Complex.One; });
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_CorrectResult_RealNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                Complex[,] dataB = { { new Complex(5, 0), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 0) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(19, 0), new Complex(22, 0) }, { new Complex(43, 0), new Complex(50, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A * B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_CorrectResult_ComplexNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 1), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 4) } };
                Complex[,] dataB = { { new Complex(5, 5), new Complex(6, 6) }, { new Complex(7, 7), new Complex(8, 8) } } ;
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(0, 38), new Complex(0, 44) }, { new Complex(0, 86), new Complex(0, 100) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A * B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_CorrectResult_MixedNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 0), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 0) } };
                Complex[,] dataB = { { new Complex(5, 5), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 8) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = {
                        { new Complex(19, 19), new Complex(6, 32) },
                        { new Complex(28, 30), new Complex(50, 50) }
                };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A * B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_ThrowsArgumentNullExceptionForNullLeft()
        {
                CoreMatrix left = null;
                CoreMatrix right = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentNullException>(() => { var temp = left * right; });
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_ThrowsArgumentNullExceptionForNullRight()
        {
                CoreMatrix left = new CoreMatrix(2, 2);
                CoreMatrix right = null;

                Assert.Throws<ArgumentNullException>(() => { var temp = left * right; });
        }

        [Test]
        public static void Operator_Multiply_MatrixMatrix_ThrowsArgumentExceptionForDimensionMismatch()
        {
                CoreMatrix A = new CoreMatrix(2, 3);
                CoreMatrix B = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentException>(() => { var temp = A * B; });
        }

        [Test]
        public static void Operator_Multiply_ScalarMatrix_CorrectResult_RealScalar()
        {
                Complex scalar = new Complex(2, 0);
                Complex[,] data = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = { { new Complex(2, 0), new Complex(4, 0) }, { new Complex(6, 0), new Complex(8, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = scalar * matrix;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_ScalarMatrix_CorrectResult_ComplexScalar()
        {
                Complex scalar = new Complex(0, 2);
                Complex[,] data = { { new Complex(1, 1), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 4) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = {
                        { new Complex(-2, 2), new Complex(-4, 4) },
                        { new Complex(-6, 6), new Complex(-8, 8) }
                };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = scalar * matrix;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_ScalarMatrix_ThrowsArgumentNullExceptionForNullMatrix()
        {
                Complex scalar = Complex.One;
                CoreMatrix matrix = null;

                Assert.Throws<ArgumentNullException>(() => { var temp = scalar * matrix; });
        }

        [Test]
        public static void Operator_Multiply_MatrixScalar_CorrectResult()
        {
                Complex scalar = new Complex(2, 0);
                Complex[,] data = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = { { new Complex(2, 0), new Complex(4, 0) }, { new Complex(6, 0), new Complex(8, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = matrix * scalar;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Multiply_MatrixScalar_ThrowsArgumentNullExceptionForNullMatrix()
        {
                Complex scalar = Complex.One;
                CoreMatrix matrix = null;

                Assert.Throws<ArgumentNullException>(() => { var temp = matrix * scalar; });
        }


        [Test]
        public static void Operator_Add_MatrixMatrix_CorrectResult_RealNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                Complex[,] dataB = { { new Complex(5, 0), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 0) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(6, 0), new Complex(8, 0) }, { new Complex(10, 0), new Complex(12, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A + B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Add_MatrixMatrix_CorrectResult_ComplexNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 1), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 4) } };
                Complex[,] dataB = { { new Complex(5, 5), new Complex(6, 6) }, { new Complex(7, 7), new Complex(8, 8) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(6, 6), new Complex(8, 8) }, { new Complex(10, 10), new Complex(12, 12) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A + B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Add_MatrixMatrix_CorrectResult_MixedNumbers()
        {
                Complex[,] dataA = { { new Complex(1, 0), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 0) } };
                Complex[,] dataB = { { new Complex(5, 5), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 8) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = {
                        { new Complex(6, 5), new Complex(8, 2) },
                        { new Complex(10, 3), new Complex(12, 8) }
                };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A + B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Add_MatrixMatrix_ThrowsArgumentNullExceptionForNullLeft()
        {
                CoreMatrix left = null;
                CoreMatrix right = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentNullException>(() => { var temp = left + right; });
        }

        [Test]
        public static void Operator_Add_MatrixMatrix_ThrowsArgumentNullExceptionForNullRight()
        {
                CoreMatrix left = new CoreMatrix(2, 2);
                CoreMatrix right = null;

                Assert.Throws<ArgumentNullException>(() => { var temp = left + right; });
        }

        [Test]
        public static void Operator_Add_MatrixMatrix_ThrowsInvalidOperationExceptionForDimensionMismatch()
        {
                CoreMatrix A = new CoreMatrix(2, 3);
                CoreMatrix B = new CoreMatrix(2, 2);

                Assert.Throws<InvalidOperationException>(() => { var temp = A + B; });
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_CorrectResult_RealNumbers()
        {
                Complex[,] dataA = { { new Complex(5, 0), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 0) } };
                Complex[,] dataB = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(4, 0), new Complex(4, 0) }, { new Complex(4, 0), new Complex(4, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A - B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_CorrectResult_ComplexNumbers()
        {
                Complex[,] dataA = { { new Complex(5, 5), new Complex(6, 6) }, { new Complex(7, 7), new Complex(8, 8) } };
                Complex[,] dataB = { { new Complex(1, 1), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 4) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = { { new Complex(4, 4), new Complex(4, 4) }, { new Complex(4, 4), new Complex(4, 4) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A - B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_CorrectResult_MixedNumbers()
        {
                Complex[,] dataA = { { new Complex(5, 5), new Complex(6, 0) }, { new Complex(7, 0), new Complex(8, 8) } };
                Complex[,] dataB = { { new Complex(1, 0), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 0) } };
                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                Complex[,] expectedData = {
                        { new Complex(4, 5), new Complex(4, -2) },
                        { new Complex(4, -3), new Complex(4, 8) }
                };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix result = A - B;

                TestUtils.AssertMatricesApproximatelyEqual(expected, result, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_ThrowsArgumentNullExceptionForNullLeft()
        {
                CoreMatrix left = null;
                CoreMatrix right = new CoreMatrix(2, 2);

                Assert.Throws<ArgumentNullException>(() => { var temp = left - right; });
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_ThrowsArgumentNullExceptionForNullRight()
        {
                CoreMatrix left = new CoreMatrix(2, 2);
                CoreMatrix right = null;

                Assert.Throws<ArgumentNullException>(() => { var temp = left - right; });
        }

        [Test]
        public static void Operator_Subtract_MatrixMatrix_ThrowsInvalidOperationExceptionForDimensionMismatch()
        {
                CoreMatrix A = new CoreMatrix(2, 3);
                CoreMatrix B = new CoreMatrix(2, 2);

                Assert.Throws<InvalidOperationException>(() => { var temp = A - B; });
        }

        [Test]
        public static void Transpose_CorrectResult_SquareMatrix_RealNumbers()
        {
                Complex[,] data = { { new Complex(1, 0), new Complex(2, 0) }, { new Complex(3, 0), new Complex(4, 0) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = { { new Complex(1, 0), new Complex(3, 0) }, { new Complex(2, 0), new Complex(4, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix transposedMatrix = matrix.Transpose();

                TestUtils.AssertMatricesApproximatelyEqual(expected, transposedMatrix, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Transpose_CorrectResult_RectangularMatrix_ComplexNumbers()
        {
                Complex[,] data = {
                        { new Complex(1, 1), new Complex(2, 2), new Complex(3, 3) },
                        { new Complex(4, 4), new Complex(5, 5), new Complex(6, 6) }
                };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = {
                        { new Complex(1, 1), new Complex(4, 4) },
                        { new Complex(2, 2), new Complex(5, 5) },
                        { new Complex(3, 3), new Complex(6, 6) }
                };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix transposedMatrix = matrix.Transpose();

                TestUtils.AssertMatricesApproximatelyEqual(expected, transposedMatrix, TestUtils.TOLERANCE);
        }

        [Test]
        public static void Transpose_CorrectResult_Vector_MixedNumbers()
        {
                Complex[,] data = { { new Complex(1, 0) }, { new Complex(2, 2) }, { new Complex(3, 0) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex[,] expectedData = { { new Complex(1, 0), new Complex(2, 2), new Complex(3, 0) } };
                CoreMatrix expected = new CoreMatrix(expectedData);

                CoreMatrix transposedMatrix = matrix.Transpose();

                TestUtils.AssertMatricesApproximatelyEqual(expected, transposedMatrix, TestUtils.TOLERANCE);
        }

        [Test]
        public static void ImplicitConversion_MatrixToComplex_CorrectResult_RealNumber()
        {
                Complex[,] data = { { new Complex(5.5, 0) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex result = matrix;

                Assert.That(result, Is.EqualTo(new Complex(5.5, 0)));
        }

        [Test]
        public static void ImplicitConversion_MatrixToComplex_CorrectResult_ComplexNumber()
        {
                Complex[,] data = { { new Complex(5.5, -2.1) } };
                CoreMatrix matrix = new CoreMatrix(data);

                Complex result = matrix;

                Assert.That(result, Is.EqualTo(new Complex(5.5, -2.1)));
        }

        [Test]
        public static void ImplicitConversion_MatrixToComplex_ThrowsArgumentNullExceptionForNullMatrix()
        {
                CoreMatrix matrix = null;

                Assert.Throws<ArgumentNullException>(() => { Complex val = matrix; });
        }

        [Test]
        public static void ImplicitConversion_MatrixToComplex_ThrowsInvalidCastExceptionForNon1x1Matrix()
        {
                CoreMatrix matrix = new CoreMatrix(2, 2);

                Assert.Throws<InvalidCastException>(() => { Complex val = matrix; });
        }

        [Test]
        public static void Clone_CreatesDeepCopy()
        {
                Complex[,] originalData = { { new Complex(1, 1), new Complex(2, 2) }, { new Complex(3, 3), new Complex(4, 4) } };
                CoreMatrix originalMatrix = new CoreMatrix(originalData);

                CoreMatrix clonedMatrix = originalMatrix.Clone();

                Assert.That(clonedMatrix, Is.Not.SameAs(originalMatrix), "Cloned matrix should be a different instance.");
                TestUtils.AssertMatricesApproximatelyEqual(originalMatrix, clonedMatrix, TestUtils.TOLERANCE);

                originalMatrix[0, 0] = new Complex(99, 99);
                Assert.That(clonedMatrix[0, 0], Is.EqualTo(new Complex(1, 1)), "Cloned matrix element should not change after original modification.");
        }

        [Test]
        [Repeat(50)]
        public static void Operator_Multiply_MatrixMatrix_RandomizedComparisonWithMathNet()
        {
                Random rnd = new Random();
                int rowsA = rnd.Next(2, 5);
                int colsA = rnd.Next(2, 5);
                int rowsB = colsA;
                int colsB = rnd.Next(2, 5);

                Complex[,] dataA = new Complex[rowsA, colsA];
                Complex[,] dataB = new Complex[rowsB, colsB];

                for (int r = 0; r < rowsA; r++)
                {
                        for (int c = 0; c < colsA; c++)
                        {
                                dataA[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                for (int r = 0; r < rowsB; r++)
                {
                        for (int c = 0; c < colsB; c++)
                        {
                                dataB[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                DenseMatrix mathNetA = DenseMatrix.OfArray(dataA);
                DenseMatrix mathNetB = DenseMatrix.OfArray(dataB);

                CoreMatrix customResult = A * B;
                DenseMatrix mathNetResult = mathNetA.Multiply(mathNetB) as DenseMatrix;

                Assert.That(mathNetResult, Is.Not.Null, "MathNet multiplication result was null.");
                TestUtils.AssertMatricesApproximatelyEqual(new CoreMatrix(mathNetResult), customResult, TestUtils.TOLERANCE);
        }

        [Test]
        [Repeat(50)]
        public static void Operator_Multiply_ScalarMatrix_RandomizedComparisonWithMathNet()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 5);
                int cols = rnd.Next(2, 5);
                Complex scalar = TestUtils.GetRandomComplex();

                Complex[,] data = new Complex[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                data[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                CoreMatrix customMatrix = new CoreMatrix(data);
                DenseMatrix mathNetMatrix = DenseMatrix.OfArray(data);

                CoreMatrix customResult = scalar * customMatrix;
                DenseMatrix mathNetResult = mathNetMatrix.Multiply(scalar) as DenseMatrix;

                Assert.That(mathNetResult, Is.Not.Null, "MathNet scalar multiplication result was null.");
                TestUtils.AssertMatricesApproximatelyEqual(new CoreMatrix(mathNetResult), customResult, TestUtils.TOLERANCE);
        }

        [Test]
        [Repeat(50)]
        public static void Operator_Add_MatrixMatrix_RandomizedComparisonWithMathNet()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 5);
                int cols = rnd.Next(2, 5);

                Complex[,] dataA = new Complex[rows, cols];
                Complex[,] dataB = new Complex[rows, cols];

                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                dataA[r, c] = TestUtils.GetRandomComplex();
                                dataB[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                DenseMatrix mathNetA = DenseMatrix.OfArray(dataA);
                DenseMatrix mathNetB = DenseMatrix.OfArray(dataB);

                CoreMatrix customResult = A + B;
                DenseMatrix mathNetResult = mathNetA.Add(mathNetB) as DenseMatrix;

                Assert.That(mathNetResult, Is.Not.Null, "MathNet addition result was null.");
                TestUtils.AssertMatricesApproximatelyEqual(new CoreMatrix(mathNetResult), customResult, TestUtils.TOLERANCE);
        }

        [Test]
        [Repeat(50)]
        public static void Operator_Subtract_MatrixMatrix_RandomizedComparisonWithMathNet()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 5);
                int cols = rnd.Next(2, 5);

                Complex[,] dataA = new Complex[rows, cols];
                Complex[,] dataB = new Complex[rows, cols];

                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                dataA[r, c] = TestUtils.GetRandomComplex();
                                dataB[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                CoreMatrix A = new CoreMatrix(dataA);
                CoreMatrix B = new CoreMatrix(dataB);

                DenseMatrix mathNetA = DenseMatrix.OfArray(dataA);
                DenseMatrix mathNetB = DenseMatrix.OfArray(dataB);

                CoreMatrix customResult = A - B;
                DenseMatrix mathNetResult = mathNetA.Subtract(mathNetB) as DenseMatrix;

                Assert.That(mathNetResult, Is.Not.Null, "MathNet subtraction result was null.");
                TestUtils.AssertMatricesApproximatelyEqual(new CoreMatrix(mathNetResult), customResult, TestUtils.TOLERANCE);
        }

        [Test]
        [Repeat(50)]
        public static void Transpose_RandomizedComparisonWithMathNet()
        {
                Random rnd = new Random();
                int rows = rnd.Next(2, 5);
                int cols = rnd.Next(2, 5);

                Complex[,] data = new Complex[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                data[r, c] = TestUtils.GetRandomComplex();
                        }
                }

                CoreMatrix customMatrix = new CoreMatrix(data);
                DenseMatrix mathNetMatrix = DenseMatrix.OfArray(data);

                CoreMatrix customTransposed = customMatrix.Transpose();
                DenseMatrix mathNetTransposed = mathNetMatrix.Transpose() as DenseMatrix;

                Assert.That(mathNetTransposed, Is.Not.Null, "MathNet transpose result was null.");
                TestUtils.AssertMatricesApproximatelyEqual(new CoreMatrix(mathNetTransposed), customTransposed, TestUtils.TOLERANCE);
        }
}
