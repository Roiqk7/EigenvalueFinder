using NUnit.Framework;
using EigenvalueFinder.Core;
using System;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class MatrixTests
{
        // --- Constructor Tests ---

        [Test]
        public void Matrix_Constructor_CreatesZeroMatrix_WithCorrectDimensions()
        {
                Console.WriteLine("Testing Matrix_Constructor_CreatesZeroMatrix_WithCorrectDimensions.");

                int rows = 3;
                int cols = 4;

                EigenvalueFinder.Core.Matrix matrix = new EigenvalueFinder.Core.Matrix(rows, cols);

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(rows, matrix.RowCount, "Row count should match constructor argument.");
                        Assert.AreEqual(cols, matrix.ColumnCount, "Column count should match constructor argument.");
                        for (int r = 0; r < rows; r++)
                        {
                                for (int c = 0; c < cols; c++)
                                {
                                        Assert.AreEqual(0.0, matrix[r, c], "All elements should be initialized to zero.");
                                }
                        }
                });
                Console.WriteLine("Matrix constructed successfully with zeros.");
        }

        [Test]
        public void Matrix_Constructor_ThrowsOnNonPositiveDimensions()
        {
                Console.WriteLine("Testing Matrix_Constructor_ThrowsOnNonPositiveDimensions.");

                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Matrix(0, 5)), "Constructor should throw for zero rows.");
                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Matrix(5, 0)), "Constructor should throw for zero columns.");
                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Matrix(-1, 5)), "Constructor should throw for negative rows.");
                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Matrix(5, -1)), "Constructor should throw for negative columns.");
                Console.WriteLine("Constructor correctly threw exceptions for invalid dimensions.");
        }

        [Test]
        public void Matrix_Constructor_FromArray_CreatesCorrectMatrix()
        {
                Console.WriteLine("Testing Matrix_Constructor_FromArray_CreatesCorrectMatrix.");

                double[,] data = {
                    {1.0, 2.0, 3.0},
                    {4.0, 5.0, 6.0}
                };
                int rows = 2;
                int cols = 3;

                EigenvalueFinder.Core.Matrix matrix = new EigenvalueFinder.Core.Matrix(data);

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(rows, matrix.RowCount, "Row count should match array dimensions.");
                        Assert.AreEqual(cols, matrix.ColumnCount, "Column count should match array dimensions.");
                        for (int r = 0; r < rows; r++)
                        {
                                for (int c = 0; c < cols; c++)
                                {
                                        Assert.AreEqual(data[r, c], matrix[r, c], $"Element at ({r},{c}) should match array value.");
                                }
                        }
                });
                Console.WriteLine("Matrix constructed successfully from array.");
        }

        [Test]
        public void Matrix_Constructor_FromArray_ThrowsOnNullArray()
        {
                Console.WriteLine("Testing Matrix_Constructor_FromArray_ThrowsOnNullArray.");

                double[,] data = null;

                Assert.Throws<ArgumentNullException>(new TestDelegate(() => new EigenvalueFinder.Core.Matrix(data)), "Constructor should throw for null array.");
                Console.WriteLine("Constructor correctly threw ArgumentNullException for null array.");
        }

        // --- Indexer Tests ---

        [Test]
        public void Matrix_Indexer_SetAndGetValue()
        {
                Console.WriteLine("Testing Matrix_Indexer_SetAndGetValue.");

                EigenvalueFinder.Core.Matrix matrix = new EigenvalueFinder.Core.Matrix(2, 2);
                double value = 123.45;
                int row = 1;
                int col = 0;

                matrix[row, col] = value;
                double retrievedValue = matrix[row, col];

                Assert.AreEqual(value, retrievedValue, "Indexer should correctly set and retrieve value.");
                Console.WriteLine($"Value {value} set and retrieved at ({row},{col}).");
        }

        // --- Operator Overloading Tests ---

        [Test]
        public void Matrix_Multiplication_MatrixByMatrix_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Multiplication_MatrixByMatrix_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                EigenvalueFinder.Core.Matrix B = new EigenvalueFinder.Core.Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                EigenvalueFinder.Core.Matrix expectedC = new EigenvalueFinder.Core.Matrix(new double[,] { { 1*5 + 2*7, 1*6 + 2*8 }, { 3*5 + 4*7, 3*6 + 4*8 } });

                EigenvalueFinder.Core.Matrix C = A * B;

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(2, C.RowCount);
                        Assert.AreEqual(2, C.ColumnCount);
                        Assert.AreEqual(19.0, C[0, 0], 1e-9);
                        Assert.AreEqual(22.0, C[0, 1], 1e-9);
                        Assert.AreEqual(43.0, C[1, 0], 1e-9);
                        Assert.AreEqual(50.0, C[1, 1], 1e-9);
                });
                Console.WriteLine("Matrix-Matrix multiplication successful.");

                EigenvalueFinder.Core.Matrix D = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2, 3 } });
                Assert.Throws<ArgumentException>(new TestDelegate(() => { var result = A * D; }), "Multiplication should throw for incompatible dimensions.");
                Console.WriteLine("Matrix-Matrix multiplication correctly threw for incompatible dimensions.");
        }

        [Test]
        public void Matrix_Multiplication_MatrixByScalar_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Multiplication_MatrixByScalar_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                double scalar = 2.0;
                EigenvalueFinder.Core.Matrix expectedB = new EigenvalueFinder.Core.Matrix(new double[,] { { 2, 4 }, { 6, 8 } });

                EigenvalueFinder.Core.Matrix B = A * scalar;

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(expectedB[0, 0], B[0, 0], 1e-9);
                        Assert.AreEqual(expectedB[0, 1], B[0, 1], 1e-9);
                        Assert.AreEqual(expectedB[1, 0], B[1, 0], 1e-9);
                        Assert.AreEqual(expectedB[1, 1], B[1, 1], 1e-9);
                });
                Console.WriteLine("Matrix-Scalar multiplication successful.");
        }

        [Test]
        public void Matrix_Multiplication_ScalarByMatrix_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Multiplication_ScalarByMatrix_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                double scalar = 2.0;
                EigenvalueFinder.Core.Matrix expectedB = new EigenvalueFinder.Core.Matrix(new double[,] { { 2, 4 }, { 6, 8 } });

                EigenvalueFinder.Core.Matrix B = scalar * A;

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(expectedB[0, 0], B[0, 0], 1e-9);
                        Assert.AreEqual(expectedB[0, 1], B[0, 1], 1e-9);
                        Assert.AreEqual(expectedB[1, 0], B[1, 0], 1e-9);
                        Assert.AreEqual(expectedB[1, 1], B[1, 1], 1e-9);
                });
                Console.WriteLine("Scalar-Matrix multiplication successful.");
        }

        [Test]
        public void Matrix_Addition_MatrixByMatrix_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Addition_MatrixByMatrix_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                EigenvalueFinder.Core.Matrix B = new EigenvalueFinder.Core.Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                EigenvalueFinder.Core.Matrix expectedC = new EigenvalueFinder.Core.Matrix(new double[,] { { 6, 8 }, { 10, 12 } });

                EigenvalueFinder.Core.Matrix C = A + B;

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(expectedC[0, 0], C[0, 0], 1e-9);
                        Assert.AreEqual(expectedC[0, 1], C[0, 1], 1e-9);
                        Assert.AreEqual(expectedC[1, 0], C[1, 0], 1e-9);
                        Assert.AreEqual(expectedC[1, 1], C[1, 1], 1e-9);
                });
                Console.WriteLine("Matrix-Matrix addition successful.");

                EigenvalueFinder.Core.Matrix D = new EigenvalueFinder.Core.Matrix(new double[,] { { 1 }, { 2 }, { 3 } });
                Assert.Throws<ArgumentException>(new TestDelegate(() => { var result = A + D; }), "Addition should throw for incompatible dimensions.");
                Console.WriteLine("Matrix-Matrix addition correctly threw for incompatible dimensions.");
        }

        [Test]
        public void Matrix_Subtraction_MatrixByMatrix_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Subtraction_MatrixByMatrix_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                EigenvalueFinder.Core.Matrix B = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                EigenvalueFinder.Core.Matrix expectedC = new EigenvalueFinder.Core.Matrix(new double[,] { { 4, 4 }, { 4, 4 } });

                EigenvalueFinder.Core.Matrix C = A - B;

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(expectedC[0, 0], C[0, 0], 1e-9);
                        Assert.AreEqual(expectedC[0, 1], C[0, 1], 1e-9);
                        Assert.AreEqual(expectedC[1, 0], C[1, 0], 1e-9);
                        Assert.AreEqual(expectedC[1, 1], C[1, 1], 1e-9);
                });
                Console.WriteLine("Matrix-Matrix subtraction successful.");
        }

        // --- Other Methods Tests ---

        [Test]
        public void Matrix_Transpose_PerformsCorrectly()
        {
                Console.WriteLine("Testing Matrix_Transpose_PerformsCorrectly.");

                EigenvalueFinder.Core.Matrix A = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
                EigenvalueFinder.Core.Matrix expectedAT = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } });

                EigenvalueFinder.Core.Matrix AT = A.Transpose();

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(expectedAT.RowCount, AT.RowCount);
                        Assert.AreEqual(expectedAT.ColumnCount, AT.ColumnCount);
                        for (int r = 0; r < expectedAT.RowCount; r++)
                        {
                                for (int c = 0; c < expectedAT.ColumnCount; c++)
                                {
                                        Assert.AreEqual(expectedAT[r, c], AT[r, c], 1e-9, $"Element at ({r},{c}) should match transposed value.");
                                }
                        }
                });
                Console.WriteLine("Matrix transpose successful.");
        }

        [Test]
        public void Matrix_Clone_CreatesDeepCopy()
        {
                Console.WriteLine("Testing Matrix_Clone_CreatesDeepCopy.");

                EigenvalueFinder.Core.Matrix original = new EigenvalueFinder.Core.Matrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });

                EigenvalueFinder.Core.Matrix clone = original.Clone();

                Assert.Multiple(() =>
                {
                        Assert.AreNotSame(original, clone, "Cloned matrix should be a different instance.");
                        Assert.AreEqual(original.RowCount, clone.RowCount, "Cloned matrix row count should match original.");
                        Assert.AreEqual(original.ColumnCount, clone.ColumnCount, "Cloned matrix column count should match original.");
                        for (int r = 0; r < original.RowCount; r++)
                        {
                                for (int c = 0; c < original.ColumnCount; c++)
                                {
                                        Assert.AreEqual(original[r, c], clone[r, c], "Cloned matrix elements should match original.");
                                }
                        }
                });

                clone[0, 0] = 99.0;
                Assert.AreNotEqual(original[0, 0], clone[0, 0], "Modifying clone should not affect original.");
                Console.WriteLine("Matrix clone created a deep copy.");
        }

        // --- Implicit Conversion Tests ---

        [Test]
        public void Matrix_ImplicitConversionToDouble_ReturnsScalarFor1x1Matrix()
        {
                Console.WriteLine("Testing Matrix_ImplicitConversionToDouble_ReturnsScalarFor1x1Matrix.");

                EigenvalueFinder.Core.Matrix oneByOneMatrix = new EigenvalueFinder.Core.Matrix(new double[,] { { 7.5 } });

                double scalarValue = oneByOneMatrix;

                Assert.AreEqual(7.5, scalarValue, 1e-9, "1x1 Matrix should implicitly convert to its single double value.");
                Console.WriteLine($"1x1 Matrix converted to double: {scalarValue}.");
        }

        [Test]
        public void Matrix_ImplicitConversionToDouble_ThrowsForNon1x1Matrix()
        {
                Console.WriteLine("Testing Matrix_ImplicitConversionToDouble_ThrowsForNon1x1Matrix.");

                EigenvalueFinder.Core.Matrix twoByTwoMatrix = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                EigenvalueFinder.Core.Matrix oneByTwoMatrix = new EigenvalueFinder.Core.Matrix(new double[,] { { 1, 2 } });
                EigenvalueFinder.Core.Matrix twoByOneMatrix = new EigenvalueFinder.Core.Matrix(new double[,] { { 1 }, { 2 } });

                Assert.Throws<InvalidCastException>(new TestDelegate(() => { double val = twoByTwoMatrix; }), "2x2 Matrix should throw on implicit conversion to double.");
                Assert.Throws<InvalidCastException>(new TestDelegate(() => { double val = oneByTwoMatrix; }), "1x2 Matrix should throw on implicit conversion to double.");
                Assert.Throws<InvalidCastException>(new TestDelegate(() => { double val = twoByOneMatrix; }), "2x1 Matrix should throw on implicit conversion to double.");
                Console.WriteLine("Non-1x1 Matrix correctly threw InvalidCastException on implicit conversion to double.");
        }
}
