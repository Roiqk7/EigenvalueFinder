using NUnit.Framework;
using EigenvalueFinder.Core;
using System;
using System.Text;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class VectorTests
{
        [Test]
        public void Constructor_Size_ColumnVector_CreatesZeroVector()
        {
                int size = 5;
                Vector vector = new Vector(size, VectorType.Column);

                Assert.That(vector.Size, Is.EqualTo(size));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Column));
                Assert.That(vector.RowCount, Is.EqualTo(size));
                Assert.That(vector.ColumnCount, Is.EqualTo(1));
                for (int i = 0; i < size; i++)
                {
                        Assert.That(vector[i], Is.EqualTo(0.0));
                }
        }

        [Test]
        public void Constructor_Size_RowVector_CreatesZeroVector()
        {
                int size = 5;
                Vector vector = new Vector(size, VectorType.Row);

                Assert.That(vector.Size, Is.EqualTo(size));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Row));
                Assert.That(vector.RowCount, Is.EqualTo(1));
                Assert.That(vector.ColumnCount, Is.EqualTo(size));
                for (int i = 0; i < size; i++)
                {
                        Assert.That(vector[i], Is.EqualTo(0.0));
                }
        }

        [Test]
        public void Constructor_Size_ThrowsOnNonPositiveSize()
        {
                Assert.Throws<ArgumentOutOfRangeException>(() => new Vector(0));
                Assert.Throws<ArgumentOutOfRangeException>(() => new Vector(-1));
        }

        [Test]
        public void Constructor_DataArray_ColumnVector_CreatesCorrectVector()
        {
                double[] data = { 1.0, 2.0, 3.0 };
                Vector vector = new Vector(data, VectorType.Column);

                Assert.That(vector.Size, Is.EqualTo(data.Length));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Column));
                Assert.That(vector.RowCount, Is.EqualTo(data.Length));
                Assert.That(vector.ColumnCount, Is.EqualTo(1));
                for (int i = 0; i < data.Length; i++)
                {
                        Assert.That(vector[i], Is.EqualTo(data[i]));
                }
        }

        [Test]
        public void Constructor_DataArray_RowVector_CreatesCorrectVector()
        {
                double[] data = { 1.0, 2.0, 3.0 };
                Vector vector = new Vector(data, VectorType.Row);

                Assert.That(vector.Size, Is.EqualTo(data.Length));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Row));
                Assert.That(vector.RowCount, Is.EqualTo(1));
                Assert.That(vector.ColumnCount, Is.EqualTo(data.Length));
                for (int i = 0; i < data.Length; i++)
                {
                        Assert.That(vector[i], Is.EqualTo(data[i]));
                }
        }

        [Test]
        public void Constructor_DataArray_ThrowsOnNullData()
        {
                double[] data = null;
                Assert.Throws<ArgumentNullException>(() => new Vector(data));
        }

        [Test]
        public void Constructor_DenseMatrix_ColumnVector_WrapsCorrectly()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[,] { { 1.0 }, { 2.0 }, { 3.0 } });
                Vector vector = new Vector(denseMatrix);

                Assert.That(vector.Size, Is.EqualTo(3));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Column));
                Assert.That(vector[0], Is.EqualTo(1.0));
                Assert.That(vector[1], Is.EqualTo(2.0));
                Assert.That(vector[2], Is.EqualTo(3.0));
        }

        [Test]
        public void Constructor_DenseMatrix_RowVector_WrapsCorrectly()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[,] { { 1.0, 2.0, 3.0 } });
                Vector vector = new Vector(denseMatrix);

                Assert.That(vector.Size, Is.EqualTo(3));
                Assert.That(vector.Type, Is.EqualTo(VectorType.Row));
                Assert.That(vector[0], Is.EqualTo(1.0));
                Assert.That(vector[1], Is.EqualTo(2.0));
                Assert.That(vector[2], Is.EqualTo(3.0));
        }

        [Test]
        public void Constructor_DenseMatrix_ThrowsOnNullDenseMatrix()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = null;
                Assert.Throws<ArgumentNullException>(() => new Vector(denseMatrix));
        }

        [Test]
        public void Constructor_DenseMatrix_ThrowsOnNonVectorMatrix()
        {
                MathNet.Numerics.LinearAlgebra.Double.DenseMatrix denseMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[,] { { 1.0, 2.0 }, { 3.0, 4.0 } });
                Assert.Throws<ArgumentException>(() => new Vector(denseMatrix));
        }

        [Test]
        public void Indexer_GetAndSet_WorksCorrectly()
        {
                Vector vector = new Vector(3);
                vector[0] = 10.0;
                vector[1] = 20.0;
                vector[2] = 30.0;

                Assert.That(vector[0], Is.EqualTo(10.0));
                Assert.That(vector[1], Is.EqualTo(20.0));
                Assert.That(vector[2], Is.EqualTo(30.0));
        }

        [Test]
        public void Indexer_Get_ThrowsOnOutOfRange()
        {
                Vector vector = new Vector(3);
                Assert.Throws<ArgumentOutOfRangeException>(() => { var val = vector[3]; });
                Assert.Throws<ArgumentOutOfRangeException>(() => { var val = vector[-1]; });
        }

        [Test]
        public void Indexer_Set_ThrowsOnOutOfRange()
        {
                Vector vector = new Vector(3);
                Assert.Throws<ArgumentOutOfRangeException>(() => vector[3] = 1.0);
                Assert.Throws<ArgumentOutOfRangeException>(() => vector[-1] = 1.0);
        }

        [Test]
        public void Operator_Multiply_ScalarVector_PerformsCorrectly()
        {
                Vector vector = new Vector(new double[] { 1.0, 2.0, 3.0 });
                double scalar = 2.0;
                Vector expected = new Vector(new double[] { 2.0, 4.0, 6.0 });

                Vector result = scalar * vector;

                Assert.That(result.Size, Is.EqualTo(expected.Size));
                for (int i = 0; i < result.Size; i++)
                {
                        Assert.That(result[i], Is.EqualTo(expected[i]));
                }
        }

        [Test]
        public void Operator_Multiply_VectorScalar_PerformsCorrectly()
        {
                Vector vector = new Vector(new double[] { 1.0, 2.0, 3.0 });
                double scalar = 2.0;
                Vector expected = new Vector(new double[] { 2.0, 4.0, 6.0 });

                Vector result = vector * scalar;

                Assert.That(result.Size, Is.EqualTo(expected.Size));
                for (int i = 0; i < result.Size; i++)
                {
                        Assert.That(result[i], Is.EqualTo(expected[i]));
                }
        }

        [Test]
        public void Operator_Multiply_VectorVector_ColumnTimesRow_PerformsOuterProduct()
        {
                Vector left = new Vector(new double[] { 1, 2 }, VectorType.Column);
                Vector right = new Vector(new double[] { 3, 4 }, VectorType.Row);
                Matrix expected = new Matrix(new double[,] { { 3, 4 }, { 6, 8 } });

                Matrix result = left * right;

                Assert.That(result.RowCount, Is.EqualTo(2));
                Assert.That(result.ColumnCount, Is.EqualTo(2));
                Assert.That(result[0, 0], Is.EqualTo(expected[0, 0]));
                Assert.That(result[0, 1], Is.EqualTo(expected[0, 1]));
                Assert.That(result[1, 0], Is.EqualTo(expected[1, 0]));
                Assert.That(result[1, 1], Is.EqualTo(expected[1, 1]));
        }

        [Test]
        public void Operator_Multiply_VectorVector_RowTimesColumn_PerformsDotProductAsMatrix()
        {
                Vector left = new Vector(new double[] { 1, 2 }, VectorType.Row);
                Vector right = new Vector(new double[] { 3, 4 }, VectorType.Column);
                Matrix expected = new Matrix(new double[,] { { 1 * 3 + 2 * 4 } });

                Matrix result = left * right;

                Assert.That(result.RowCount, Is.EqualTo(1));
                Assert.That(result.ColumnCount, Is.EqualTo(1));
                Assert.That(result[0, 0], Is.EqualTo(expected[0, 0]));
        }

        [Test]
        public void Operator_Multiply_VectorVector_ThrowsOnIncompatibleDimensions()
        {
                Vector left = new Vector(new double[] { 1, 2 }, VectorType.Column);
                Vector right = new Vector(new double[] { 3, 4 }, VectorType.Column);
                Assert.Throws<ArgumentException>(() => { var result = left * right; });
        }

        [Test]
        public void Operator_Add_Vectors_PerformsCorrectly()
        {
                Vector left = new Vector(new double[] { 1.0, 2.0, 3.0 });
                Vector right = new Vector(new double[] { 4.0, 5.0, 6.0 });
                Vector expected = new Vector(new double[] { 5.0, 7.0, 9.0 });

                Vector result = left + right;

                Assert.That(result.Size, Is.EqualTo(expected.Size));
                for (int i = 0; i < result.Size; i++)
                {
                        Assert.That(result[i], Is.EqualTo(expected[i]));
                }
        }

        [Test]
        public void Operator_Add_Vectors_ThrowsOnDifferentSizes()
        {
                Vector left = new Vector(new double[] { 1.0, 2.0 });
                Vector right = new Vector(new double[] { 4.0, 5.0, 6.0 });

                Assert.Throws<ArgumentException>(() => { var result = left + right; });
        }

        [Test]
        public void Operator_Add_Vectors_ThrowsOnDifferentTypes()
        {
                Vector left = new Vector(new double[] { 1.0, 2.0 }, VectorType.Column);
                Vector right = new Vector(new double[] { 1.0, 2.0 }, VectorType.Row);

                Assert.Throws<ArgumentException>(() => { var result = left + right; });
        }

        [Test]
        public void Operator_Subtract_Vectors_PerformsCorrectly()
        {
                Vector left = new Vector(new double[] { 5.0, 7.0, 9.0 });
                Vector right = new Vector(new double[] { 4.0, 5.0, 6.0 });
                Vector expected = new Vector(new double[] { 1.0, 2.0, 3.0 });

                Vector result = left - right;

                Assert.That(result.Size, Is.EqualTo(expected.Size));
                for (int i = 0; i < result.Size; i++)
                {
                        Assert.That(result[i], Is.EqualTo(expected[i]));
                }
        }

        [Test]
        public void Operator_Subtract_Vectors_ThrowsOnDifferentSizes()
        {
                Vector left = new Vector(new double[] { 5.0, 7.0 });
                Vector right = new Vector(new double[] { 4.0, 5.0, 6.0 });

                Assert.Throws<ArgumentException>(() => { var result = left - right; });
        }

        [Test]
        public void Operator_Subtract_Vectors_ThrowsOnDifferentTypes()
        {
                Vector left = new Vector(new double[] { 1.0, 2.0 }, VectorType.Column);
                Vector right = new Vector(new double[] { 1.0, 2.0 }, VectorType.Row);

                Assert.Throws<ArgumentException>(() => { var result = left - right; });
        }

        [Test]
        public void DotProduct_PerformsCorrectly()
        {
                Vector v1 = new Vector(new double[] { 1.0, 2.0, 3.0 });
                Vector v2 = new Vector(new double[] { 4.0, 5.0, 6.0 });
                double expectedDotProduct = (1.0 * 4.0) + (2.0 * 5.0) + (3.0 * 6.0);

                double actualDotProduct = v1.DotProduct(v2);

                Assert.That(actualDotProduct, Is.EqualTo(expectedDotProduct));
        }

        [Test]
        public void DotProduct_ColumnAndRowVector_PerformsCorrectly()
        {
                Vector v1 = new Vector(new double[] { 1.0, 2.0 }, VectorType.Column);
                Vector v2 = new Vector(new double[] { 3.0, 4.0 }, VectorType.Row);
                double expectedDotProduct = (1.0 * 3.0) + (2.0 * 4.0);

                double actualDotProduct = v1.DotProduct(v2);

                Assert.That(actualDotProduct, Is.EqualTo(expectedDotProduct));
        }

        [Test]
        public void DotProduct_ThrowsOnDifferentSizes()
        {
                Vector v1 = new Vector(new double[] { 1.0, 2.0 });
                Vector v2 = new Vector(new double[] { 4.0, 5.0, 6.0 });

                Assert.Throws<ArgumentException>(() => v1.DotProduct(v2));
        }

        [Test]
        public void L2Norm_CalculatesCorrectly()
        {
                Vector vector = new Vector(new double[] { 3.0, 4.0 });
                double expectedNorm = 5.0;

                double actualNorm = vector.L2Norm();

                Assert.That(actualNorm, Is.EqualTo(expectedNorm));
        }

        [Test]
        public void L2Norm_ZeroVector_ReturnsZero()
        {
                Vector vector = new Vector(new double[] { 0.0, 0.0, 0.0 });
                double expectedNorm = 0.0;

                double actualNorm = vector.L2Norm();

                Assert.That(actualNorm, Is.EqualTo(expectedNorm));
        }

        [Test]
        public void Normalize_PerformsCorrectly()
        {
                Vector vector = new Vector(new double[] { 3.0, 4.0 });
                Vector expected = new Vector(new double[] { 3.0 / 5.0, 4.0 / 5.0 });

                Vector normalizedVector = vector.Normalize();

                Assert.That(normalizedVector.Size, Is.EqualTo(expected.Size));
                Assert.That(normalizedVector[0], Is.EqualTo(expected[0]).Within(0.00001));
                Assert.That(normalizedVector[1], Is.EqualTo(expected[1]).Within(0.00001));
                Assert.That(normalizedVector.L2Norm(), Is.EqualTo(1.0).Within(0.00001));
        }

        [Test]
        public void Normalize_ThrowsOnZeroVector()
        {
                Vector vector = new Vector(new double[] { 0.0, 0.0 });
                Assert.Throws<InvalidOperationException>(() => vector.Normalize());
        }

        [Test]
        public void Transpose_ColumnToRow_PerformsCorrectly()
        {
                Vector original = new Vector(new double[] { 1.0, 2.0, 3.0 }, VectorType.Column);
                Vector transposed = original.Transpose();

                Assert.That(transposed.Size, Is.EqualTo(original.Size));
                Assert.That(transposed.Type, Is.EqualTo(VectorType.Row));
                Assert.That(transposed.RowCount, Is.EqualTo(1));
                Assert.That(transposed.ColumnCount, Is.EqualTo(original.Size));
                for (int i = 0; i < original.Size; i++)
                {
                        Assert.That(transposed[i], Is.EqualTo(original[i]));
                }
        }

        [Test]
        public void Transpose_RowToColumn_PerformsCorrectly()
        {
                Vector original = new Vector(new double[] { 1.0, 2.0, 3.0 }, VectorType.Row);
                Vector transposed = original.Transpose();

                Assert.That(transposed.Size, Is.EqualTo(original.Size));
                Assert.That(transposed.Type, Is.EqualTo(VectorType.Column));
                Assert.That(transposed.RowCount, Is.EqualTo(original.Size));
                Assert.That(transposed.ColumnCount, Is.EqualTo(1));
                for (int i = 0; i < original.Size; i++)
                {
                        Assert.That(transposed[i], Is.EqualTo(original[i]));
                }
        }

        [Test]
        public void Clone_CreatesDeepCopy()
        {
                Vector original = new Vector(new double[] { 1.0, 2.0, 3.0 });
                Vector clone = original.Clone();

                Assert.That(clone, Is.Not.SameAs(original));
                Assert.That(clone.Size, Is.EqualTo(original.Size));
                Assert.That(clone.Type, Is.EqualTo(original.Type));
                for (int i = 0; i < original.Size; i++)
                {
                        Assert.That(clone[i], Is.EqualTo(original[i]));
                }

                original[0] = 99.0;
                Assert.That(clone[0], Is.Not.EqualTo(99.0));
        }

        [Test]
        public void ToString_ReturnsFormattedString()
        {
                Vector vector = new Vector(new double[] { 1.23456, 2.34567, 3.45678 });
                string expected = "[1,2346, 2,3457, 3,4568]";

                string actual = vector.ToString();

                Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_SingleElementVector()
        {
                Vector vector = new Vector(new double[] { 7.89 });
                string expected = "[7,8900]";
                string actual = vector.ToString();
                Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_EmptyVector_NotApplicableDueToConstructorConstraints()
        {
                Vector vector = new Vector(1);
                string expected = "[0,0000]";
                string actual = vector.ToString();
                Assert.That(actual, Is.EqualTo(expected));
        }
}
