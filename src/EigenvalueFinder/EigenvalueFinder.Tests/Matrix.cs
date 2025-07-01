using NUnit.Framework;
using EigenvalueFinder.Core;
using System;
using System.Text;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class MatrixTests
{
        [Test]
        public void Constructor_Dimensions_CreatesZeroMatrix()
        {
                int rows = 3;
                int cols = 4;

                Matrix matrix = new Matrix(rows, cols);

                Assert.That(matrix.RowCount, Is.EqualTo(rows));
                Assert.That(matrix.ColumnCount, Is.EqualTo(cols));
                for (int r = 0; r < rows; r++)
                {
                        for (int c = 0; c < cols; c++)
                        {
                                Assert.That(matrix[r, c], Is.EqualTo(0.0));
                        }
                }
        }

        [Test]
        public void Constructor_Dimensions_ThrowsOnNonPositiveRows()
        {
                int rows = 0;
                int cols = 3;

                Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, cols));
        }

        [Test]
        public void Constructor_Dimensions_ThrowsOnNonPositiveColumns()
        {
                int rows = 3;
                int cols = 0;

                Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, cols));
        }

        [Test]
        public void Constructor_Dimensions_ThrowsOnNegativeRows()
        {
                int rows = -1;
                int cols = 3;

                Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, cols));
        }

        [Test]
        public void Constructor_Dimensions_ThrowsOnNegativeColumns()
        {
                int rows = 3;
                int cols = -1;

                Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, cols));
        }

        [Test]
        public void Constructor_2DArray_CreatesCorrectMatrix()
        {
                double[,] data = { { 1.0, 2.0 }, { 3.0, 4.0 }, { 5.0, 6.0 } };

                Matrix matrix = new Matrix(data);

                Assert.That(matrix.RowCount, Is.EqualTo(3));
                Assert.That(matrix.ColumnCount, Is.EqualTo(2));
                Assert.That(matrix[0, 0], Is.EqualTo(1.0));
                Assert.That(matrix[0, 1], Is.EqualTo(2.0));
                Assert.That(matrix[1, 0], Is.EqualTo(3.0));
                Assert.That(matrix[1, 1], Is.EqualTo(4.0));
                Assert.That(matrix[2, 0], Is.EqualTo(5.0));
                Assert.That(matrix[2, 1], Is.EqualTo(6.0));
        }

        [Test]
        public void Constructor_2DArray_ThrowsOnNullData()
        {
                double[,] data = null;

                Assert.Throws<ArgumentNullException>(() => new Matrix(data));
        }

        [Test]
        public void Constructor_DenseMatrix_WrapsCorrectly()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[,] { { 7.0, 8.0 }, { 9.0, 10.0 } });

                Matrix matrix = new Matrix(denseMatrix);

                Assert.That(matrix.RowCount, Is.EqualTo(2));
                Assert.That(matrix.ColumnCount, Is.EqualTo(2));
                Assert.That(matrix[0, 0], Is.EqualTo(7.0));
                Assert.That(matrix[1, 1], Is.EqualTo(10.0));
        }

        [Test]
        public void Constructor_DenseMatrix_ThrowsOnNullDenseMatrix()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = null;

                Assert.Throws<ArgumentNullException>(() => new Matrix(denseMatrix));
        }

        [Test]
        public void Identity_CreatesCorrectIdentityMatrix()
        {
                int size = 3;
                double[,] expectedData = { { 1.0, 0.0, 0.0 }, { 0.0, 1.0, 0.0 }, { 0.0, 0.0, 1.0 } };
                Matrix expectedMatrix = new Matrix(expectedData);

                Matrix identityMatrix = Matrix.Identity(size);

                Assert.That(identityMatrix.RowCount, Is.EqualTo(size));
                Assert.That(identityMatrix.ColumnCount, Is.EqualTo(size));
                for (int r = 0; r < size; r++)
                {
                        for (int c = 0; c < size; c++)
                        {
                                Assert.That(identityMatrix[r, c], Is.EqualTo(expectedMatrix[r, c]));
                        }
                }
        }

        [Test]
        public void Identity_ThrowsOnNonPositiveSize()
        {
                int size = 0;

                Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Identity(size));
        }

        [Test]
        public void Identity_ThrowsOnNegativeSize()
        {
                int size = -1;

                Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Identity(size));
        }

        [Test]
        public void Indexer_GetAndSet_WorksCorrectly()
        {
                Matrix matrix = new Matrix(2, 2);
                double value = 123.45;

                matrix[0, 0] = value;
                double retrievedValue = matrix[0, 0];

                Assert.That(retrievedValue, Is.EqualTo(value));
        }

        [Test]
        public void Indexer_Get_ThrowsOnOutOfRange()
        {
                Matrix matrix = new Matrix(2, 2);

                Assert.Throws<ArgumentOutOfRangeException>(() => { var val = matrix[2, 0]; });
                Assert.Throws<ArgumentOutOfRangeException>(() => { var val = matrix[0, 2]; });
        }

        [Test]
        public void Indexer_Set_ThrowsOnOutOfRange()
        {
                Matrix matrix = new Matrix(2, 2);

                Assert.Throws<ArgumentOutOfRangeException>(() => matrix[2, 0] = 1.0);
                Assert.Throws<ArgumentOutOfRangeException>(() => matrix[0, 2] = 1.0);
        }

        [Test]
        public void Operator_Multiply_MatrixMatrix_PerformsCorrectly()
        {
                Matrix left = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Matrix right = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                Matrix expected = new Matrix(new double[,] { { 19, 22 }, { 43, 50 } });

                Matrix result = left * right;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < result.RowCount; r++)
                {
                        for (int c = 0; c < result.ColumnCount; c++)
                        {
                                Assert.That(result[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Operator_Multiply_MatrixMatrix_ThrowsOnIncompatibleDimensions()
        {
                Matrix left = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
                Matrix right = new Matrix(new double[,] { { 7, 8 }, { 9, 10 } });

                Assert.Throws<ArgumentException>(() => { var result = left * right; });
        }

        [Test]
        public void Operator_Multiply_MatrixScalar_PerformsCorrectly()
        {
                Matrix matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                double scalar = 2.0;
                Matrix expected = new Matrix(new double[,] { { 2, 4 }, { 6, 8 } });

                Matrix result = matrix * scalar;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < result.RowCount; r++)
                {
                        for (int c = 0; c < result.ColumnCount; c++)
                        {
                                Assert.That(result[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Operator_Multiply_ScalarMatrix_PerformsCorrectly()
        {
                Matrix matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                double scalar = 2.0;
                Matrix expected = new Matrix(new double[,] { { 2, 4 }, { 6, 8 } });

                Matrix result = scalar * matrix;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < result.RowCount; r++)
                {
                        for (int c = 0; c < result.ColumnCount; c++)
                        {
                                Assert.That(result[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Operator_Multiply_MatrixVector_PerformsCorrectly()
        {
                Matrix matrix = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Vector vector = new Vector(new double[] { 5, 6 });
                Vector expected = new Vector(new double[] { 1 * 5 + 2 * 6, 3 * 5 + 4 * 6 });

                Vector result = matrix * vector;

                Assert.That(result.Size, Is.EqualTo(2));
                Assert.That(result[0], Is.EqualTo(expected[0]));
                Assert.That(result[1], Is.EqualTo(expected[1]));
        }

        [Test]
        public void Operator_Multiply_MatrixVector_ThrowsOnIncompatibleDimensions()
        {
                Matrix matrix = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
                Vector vector = new Vector(new double[] { 7, 8 });

                Assert.Throws<ArgumentException>(() => { var result = matrix * vector; });
        }

        [Test]
        public void Operator_Add_MatrixMatrix_PerformsCorrectly()
        {
                Matrix left = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Matrix right = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                Matrix expected = new Matrix(new double[,] { { 6, 8 }, { 10, 12 } });

                Matrix result = left + right;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < result.RowCount; r++)
                {
                        for (int c = 0; c < result.ColumnCount; c++)
                        {
                                Assert.That(result[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Operator_Add_MatrixMatrix_ThrowsOnDifferentDimensions()
        {
                Matrix left = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Matrix right = new Matrix(new double[,] { { 5, 6, 7 }, { 8, 9, 10 } });

                Assert.Throws<ArgumentException>(() => { var result = left + right; });
        }

        [Test]
        public void Operator_Subtract_MatrixMatrix_PerformsCorrectly()
        {
                Matrix left = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                Matrix right = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Matrix expected = new Matrix(new double[,] { { 4, 4 }, { 4, 4 } });

                Matrix result = left - right;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < result.RowCount; r++)
                {
                        for (int c = 0; c < result.ColumnCount; c++)
                        {
                                Assert.That(result[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Operator_Subtract_MatrixMatrix_ThrowsOnDifferentDimensions()
        {
                Matrix left = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
                Matrix right = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

                Assert.Throws<ArgumentException>(() => { var result = left - right; });
        }

        [Test]
        public void Transpose_PerformsCorrectly()
        {
                Matrix original = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
                Matrix expected = new Matrix(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } });

                Matrix transposed = original.Transpose();

                Assert.That(transposed.RowCount, Is.EqualTo(3));
                Assert.That(transposed.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < transposed.RowCount; r++)
                {
                        for (int c = 0; c < transposed.ColumnCount; c++)
                        {
                                Assert.That(transposed[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Transpose_SquareMatrix_PerformsCorrectly()
        {
                Matrix original = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
                Matrix expected = new Matrix(new double[,] { { 1, 3 }, { 2, 4 } });

                Matrix transposed = original.Transpose();

                Assert.That(transposed.RowCount, Is.EqualTo(2));
                Assert.That(transposed.ColumnCount, Is.EqualTo(2));
                for (int r = 0; r < transposed.RowCount; r++)
                {
                        for (int c = 0; c < transposed.ColumnCount; c++)
                        {
                                Assert.That(transposed[r, c], Is.EqualTo(expected[r, c]));
                        }
                }
        }

        [Test]
        public void Clone_CreatesDeepCopy()
        {
                Matrix original = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });

                Matrix clone = original.Clone();

                Assert.That(clone, Is.Not.SameAs(original));
                Assert.That(clone.RowCount, Is.EqualTo(original.RowCount));
                Assert.That(clone.ColumnCount, Is.EqualTo(original.ColumnCount));
                for (int r = 0; r < original.RowCount; r++)
                {
                        for (int c = 0; c < original.ColumnCount; c++)
                        {
                                Assert.That(clone[r, c], Is.EqualTo(original[r, c]));
                        }
                }

                original[0, 0] = 99.0;
                Assert.That(clone[0, 0], Is.Not.EqualTo(99.0));
        }

        [Test]
        public void ToDenseMatrix_ReturnsInternalDenseMatrix()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[,] { { 1.0, 2.0 } });
                Matrix matrix = new Matrix(denseMatrix);

                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix retrievedDenseMatrix = matrix.ToDenseMatrix();

                Assert.That(retrievedDenseMatrix, Is.SameAs(denseMatrix));
        }

        [Test]
        public void ImplicitConversion_ToDouble_1x1Matrix_ReturnsCorrectValue()
        {
                Matrix matrix = new Matrix(new double[,] { { 42.0 } });

                double value = matrix;

                Assert.That(value, Is.EqualTo(42.0));
        }

        [Test]
        public void ImplicitConversion_ToDouble_ThrowsOnNon1x1Matrix()
        {
                Matrix matrix2x2 = new Matrix(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });
                Matrix matrix1x2 = new Matrix(new double[,] { { 1.0, 2.0 } });
                Matrix matrix2x1 = new Matrix(new double[,] { { 1.0 }, { 2.0 } });

                Assert.Throws<InvalidCastException>(() => { double val = matrix2x2; });
                Assert.Throws<InvalidCastException>(() => { double val = matrix1x2; });
                Assert.Throws<InvalidCastException>(() => { double val = matrix2x1; });
        }

        [Test]
        public void ImplicitConversion_ToDouble_ThrowsOnNullMatrix()
        {
                Matrix matrix = null;

                Assert.Throws<ArgumentNullException>(() => { double val = matrix; });
        }

        [Test]
        public void ToString_ReturnsFormattedString()
        {
                Matrix matrix = new Matrix(new double[,] { { 1.23456, 2.34567 }, { 3.45678, 4.56789 } });
                string expected = "1,2346\t2,3457\t" + Environment.NewLine +
                                  "3,4568\t4,5679\t" + Environment.NewLine;

                string actual = matrix.ToString();

                Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_EmptyMatrix()
        {
                Matrix matrix = new Matrix(1, 1);
                matrix[0, 0] = 0.0;
                string expected = "0,0000\t" + Environment.NewLine;
                string actual = matrix.ToString();
                Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_SingleElementMatrix()
        {
                Matrix matrix = new Matrix(new double[,] { { 7.89 } });
                string expected = "7,8900\t" + Environment.NewLine;
                string actual = matrix.ToString();
                Assert.That(actual, Is.EqualTo(expected));
        }
}
