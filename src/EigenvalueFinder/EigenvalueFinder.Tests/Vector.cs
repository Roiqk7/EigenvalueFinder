using NUnit.Framework;
using EigenvalueFinder.Core;
using System;

namespace EigenvalueFinder.Tests;

[TestFixture]
public class VectorTests
{

        // --- Constructor Tests ---

        [Test]
        public void Vector_Constructor_CreatesColumnVector_WithCorrectSizeAndZeros()
        {
                Console.WriteLine("Testing Vector_Constructor_CreatesColumnVector_WithCorrectSizeAndZeros.");

                int size = 5;


                EigenvalueFinder.Core.Vector vector = new EigenvalueFinder.Core.Vector(size);

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(size, vector.Size, "Vector size should match constructor argument.");
                        Assert.AreEqual(VectorType.Column, vector.Type, "Vector type should be Column by default.");
                        Assert.AreEqual(size, vector.RowCount, "Column vector RowCount should be its size.");
                        Assert.AreEqual(1, vector.ColumnCount, "Column vector ColumnCount should be 1.");
                        for (int i = 0; i < size; i++)
                        {
                                Assert.AreEqual(0.0, vector[i], "All elements should be initialized to zero.");
                        }
                });
                Console.WriteLine($"Column vector of size {size} created successfully.");
        }

        [Test]
        public void Vector_Constructor_CreatesRowVector_WithCorrectSizeAndZeros()
        {
                Console.WriteLine("Testing Vector_Constructor_CreatesRowVector_WithCorrectSizeAndZeros.");

                int size = 3;

                EigenvalueFinder.Core.Vector vector = new EigenvalueFinder.Core.Vector(size, VectorType.Row);

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(size, vector.Size, "Vector size should match constructor argument.");
                        Assert.AreEqual(VectorType.Row, vector.Type, "Vector type should be Row.");
                        Assert.AreEqual(1, vector.RowCount, "Row vector RowCount should be 1.");
                        Assert.AreEqual(size, vector.ColumnCount, "Row vector ColumnCount should be its size.");
                        for (int i = 0; i < size; i++)
                        {
                                Assert.AreEqual(0.0, vector[i], "All elements should be initialized to zero.");
                        }
                });
                Console.WriteLine($"Row vector of size {size} created successfully.");
        }

        [Test]
        public void Vector_Constructor_FromArray_CreatesCorrectVectorAndType()
        {
                Console.WriteLine("Testing Vector_Constructor_FromArray_CreatesCorrectVectorAndType.");

                double[] data = { 1.0, 2.0, 3.0 };

                EigenvalueFinder.Core.Vector colVector = new EigenvalueFinder.Core.Vector(data, VectorType.Column);
                EigenvalueFinder.Core.Vector rowVector = new EigenvalueFinder.Core.Vector(data, VectorType.Row);

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(data.Length, colVector.Size);
                        Assert.AreEqual(VectorType.Column, colVector.Type);
                        for (int i = 0; i < data.Length; i++)
                        {
                                Assert.AreEqual(data[i], colVector[i]);
                        }
                });
                Console.WriteLine("Column vector from array created successfully.");

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(data.Length, rowVector.Size);
                        Assert.AreEqual(VectorType.Row, rowVector.Type);
                        for (int i = 0; i < data.Length; i++)
                        {
                                Assert.AreEqual(data[i], rowVector[i]);
                        }
                });
                Console.WriteLine("Row vector from array created successfully.");
        }

        [Test]
        public void Vector_Constructor_ThrowsOnNonPositiveSize()
        {
                Console.WriteLine("Testing Vector_Constructor_ThrowsOnNonPositiveSize.");

                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Vector(0)), "Constructor should throw for zero size.");
                Assert.Throws<ArgumentOutOfRangeException>(new TestDelegate(() => new EigenvalueFinder.Core.Vector(-1)), "Constructor should throw for negative size.");
                Console.WriteLine("Constructor correctly threw exceptions for invalid sizes.");
        }

        // --- Indexer Tests ---

        [Test]
        public void Vector_Indexer_SetAndGetValue_ColumnVector()
        {
                Console.WriteLine("Testing Vector_Indexer_SetAndGetValue_ColumnVector.");

                EigenvalueFinder.Core.Vector vector = new EigenvalueFinder.Core.Vector(3, VectorType.Column);
                double value = 7.89;
                int index = 1;

                vector[index] = value;
                double retrievedValue = vector[index];

                Assert.AreEqual(value, retrievedValue, "Indexer should correctly set and retrieve value for column vector.");
                Console.WriteLine($"Column vector element at {index} set to {value}.");
        }

        [Test]
        public void Vector_Indexer_SetAndGetValue_RowVector()
        {
                Console.WriteLine("Testing Vector_Indexer_SetAndGetValue_RowVector.");

                EigenvalueFinder.Core.Vector vector = new EigenvalueFinder.Core.Vector(3, VectorType.Row);
                double value = 7.89;
                int index = 1;

                vector[index] = value;
                double retrievedValue = vector[index];

                Assert.AreEqual(value, retrievedValue, "Indexer should correctly set and retrieve value for row vector.");
                Console.WriteLine($"Row vector element at {index} set to {value}.");
        }

        // --- Vector Operations ---

        [Test]
        public void Vector_DotProduct_PerformsCorrectly()
        {
                Console.WriteLine("Testing Vector_DotProduct_PerformsCorrectly.");

                EigenvalueFinder.Core.Vector v1 = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 }, VectorType.Column);
                EigenvalueFinder.Core.Vector v2 = new EigenvalueFinder.Core.Vector(new double[] { 4, 5, 6 }, VectorType.Column);
                double expectedDotProduct = 1 * 4 + 2 * 5 + 3 * 6; // 4 + 10 + 18 = 32

                double dotProduct = v1.DotProduct(v2);

                Assert.AreEqual(expectedDotProduct, dotProduct, 1e-9, "Dot product should be calculated correctly.");
                Console.WriteLine($"Dot product of {v1.ToString()} and {v2.ToString()} is {dotProduct}.");

                EigenvalueFinder.Core.Vector v3 = new EigenvalueFinder.Core.Vector(new double[] { 1, 2 }, VectorType.Row);
                EigenvalueFinder.Core.Vector v4 = new EigenvalueFinder.Core.Vector(new double[] { 3, 4 }, VectorType.Row);
                double expectedDotProduct2 = 1 * 3 + 2 * 4; // 3 + 8 = 11
                double dotProduct2 = v3.DotProduct(v4);
                Assert.AreEqual(expectedDotProduct2, dotProduct2, 1e-9, "Dot product with row vectors should be correct.");
                Console.WriteLine($"Dot product of {v3.ToString()} and {v4.ToString()} is {dotProduct2}.");

                EigenvalueFinder.Core.Vector v5 = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 }, VectorType.Row);
                EigenvalueFinder.Core.Vector v6 = new EigenvalueFinder.Core.Vector(new double[] { 4, 5, 6 }, VectorType.Column);
                double mixedDotProduct = v5.DotProduct(v6);
                Assert.AreEqual(expectedDotProduct, mixedDotProduct, 1e-9, "Dot product with mixed types should be correct.");
                Console.WriteLine($"Dot product of {v5.ToString()} and {v6.ToString()} (mixed types) is {mixedDotProduct}.");

                EigenvalueFinder.Core.Vector v7 = new EigenvalueFinder.Core.Vector(new double[] { 1, 2 });
                EigenvalueFinder.Core.Vector v8 = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 });
                Assert.Throws<ArgumentException>(new TestDelegate(() => v7.DotProduct(v8)), "Dot product should throw for different sized vectors.");
                Console.WriteLine("Dot product correctly threw for different sized vectors.");
        }

        [Test]
        public void Vector_L2Norm_PerformsCorrectly()
        {
                Console.WriteLine("Testing Vector_L2Norm_PerformsCorrectly.");

                EigenvalueFinder.Core.Vector v = new EigenvalueFinder.Core.Vector(new double[] { 3, 4, 0 }, VectorType.Column);
                double expectedNorm = Math.Sqrt(3 * 3 + 4 * 4 + 0 * 0); // Sqrt(9 + 16 + 0) = Sqrt(25) = 5


                double norm = v.L2Norm();


                Assert.AreEqual(expectedNorm, norm, 1e-9, "L2 Norm should be calculated correctly.");
                Console.WriteLine($"L2 Norm of {v.ToString()} is {norm}.");

                EigenvalueFinder.Core.Vector vr = new EigenvalueFinder.Core.Vector(new double[] { -1, 1 }, VectorType.Row);
                double expectedNormR = Math.Sqrt((-1) * (-1) + 1 * 1); // Sqrt(1 + 1) = Sqrt(2)
                double normR = vr.L2Norm();
                Assert.AreEqual(expectedNormR, normR, 1e-9, "L2 Norm of row vector should be calculated correctly.");
                Console.WriteLine($"L2 Norm of {vr.ToString()} is {normR}.");
        }

        [Test]
        public void Vector_Normalize_PerformsCorrectly()
        {
                Console.WriteLine("Testing Vector_Normalize_PerformsCorrectly.");

                EigenvalueFinder.Core.Vector v = new EigenvalueFinder.Core.Vector(new double[] { 3, 4, 0 }, VectorType.Column); // Norm is 5
                EigenvalueFinder.Core.Vector expectedNormalized = new EigenvalueFinder.Core.Vector(new double[] { 0.6, 0.8, 0.0 }, VectorType.Column);


                EigenvalueFinder.Core.Vector normalizedV = v.Normalize();


                Assert.Multiple(() =>
                {
                        Assert.AreEqual(v.Size, normalizedV.Size, "Normalized vector should have the same size.");
                        Assert.AreEqual(v.Type, normalizedV.Type, "Normalized vector should have the same type."); // Should preserve type
                        Assert.AreEqual(1.0, normalizedV.L2Norm(), 1e-9, "Normalized vector should have L2 Norm of 1.");
                        for (int i = 0; i < v.Size; i++)
                        {
                                Assert.AreEqual(expectedNormalized[i], normalizedV[i], 1e-9, $"Element {i} of normalized vector incorrect.");
                        }
                });
                Console.WriteLine($"Vector {v.ToString()} normalized to {normalizedV.ToString()}.");

                EigenvalueFinder.Core.Vector zeroVector = new EigenvalueFinder.Core.Vector(3);

                Assert.Throws<InvalidOperationException>(() =>
                {
                        zeroVector.Normalize();
                }, "Normalizing a zero vector should throw an InvalidOperationException.");

                Console.WriteLine("Zero vector correctly threw InvalidOperationException on normalize.");
        }

        [Test]
        public void Vector_Transpose_ChangesVectorTypeAndDimensions()
        {
                Console.WriteLine("Testing Vector_Transpose_ChangesVectorTypeAndDimensions.");

                EigenvalueFinder.Core.Vector colVector = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 }, VectorType.Column); // 3x1
                EigenvalueFinder.Core.Vector rowVector = new EigenvalueFinder.Core.Vector(new double[] { 4, 5 }, VectorType.Row);       // 1x2


                EigenvalueFinder.Core.Vector transposedCol = colVector.Transpose();
                EigenvalueFinder.Core.Vector transposedRow = rowVector.Transpose();

                Assert.Multiple(() =>
                {
                        Assert.AreNotSame(colVector, transposedCol, "Transposed vector should be a new instance.");
                        Assert.AreEqual(colVector.Size, transposedCol.Size, "Size should remain the same after transpose.");
                        Assert.AreEqual(VectorType.Row, transposedCol.Type, "Column vector should become Row vector after transpose.");
                        Assert.AreEqual(1, transposedCol.RowCount, "Transposed column vector RowCount should be 1.");
                        Assert.AreEqual(colVector.Size, transposedCol.ColumnCount, "Transposed column vector ColumnCount should be its size.");
                        for (int i = 0; i < colVector.Size; i++)
                        {
                                Assert.AreEqual(colVector[i], transposedCol[i], "Elements should match for transposed vector.");
                        }
                });
                Console.WriteLine($"Column vector {colVector.ToString()} transposed to Row vector {transposedCol.ToString()}.");

                Assert.Multiple(() =>
                {
                        Assert.AreNotSame(rowVector, transposedRow, "Transposed vector should be a new instance.");
                        Assert.AreEqual(rowVector.Size, transposedRow.Size, "Size should remain the same after transpose.");
                        Assert.AreEqual(VectorType.Column, transposedRow.Type, "Row vector should become Column vector after transpose.");
                        Assert.AreEqual(rowVector.Size, transposedRow.RowCount, "Transposed row vector RowCount should be its size.");
                        Assert.AreEqual(1, transposedRow.ColumnCount, "Transposed row vector ColumnCount should be 1.");
                        for (int i = 0; i < rowVector.Size; i++)
                        {
                                Assert.AreEqual(rowVector[i], transposedRow[i], "Elements should match for transposed vector.");
                        }
                });
                Console.WriteLine($"Row vector {rowVector.ToString()} transposed to Column vector {transposedRow.ToString()}.");
        }

        [Test]
        public void Vector_Clone_CreatesDeepCopyAndPreservesType()
        {
                Console.WriteLine("Testing Vector_Clone_CreatesDeepCopyAndPreservesType.");

                EigenvalueFinder.Core.Vector originalCol = new EigenvalueFinder.Core.Vector(new double[] { 1.0, 2.0, 3.0 }, VectorType.Column);
                EigenvalueFinder.Core.Vector originalRow = new EigenvalueFinder.Core.Vector(new double[] { 4.0, 5.0 }, VectorType.Row);


                EigenvalueFinder.Core.Vector clonedCol = originalCol.Clone();
                EigenvalueFinder.Core.Vector clonedRow = originalRow.Clone();

                Assert.Multiple(() =>
                {
                        Assert.AreNotSame(originalCol, clonedCol, "Cloned column vector should be a different instance.");
                        Assert.AreEqual(originalCol.Size, clonedCol.Size, "Cloned column vector size should match original.");
                        Assert.AreEqual(originalCol.Type, clonedCol.Type, "Cloned column vector type should match original.");
                        for (int i = 0; i < originalCol.Size; i++)
                        {
                                Assert.AreEqual(originalCol[i], clonedCol[i], "Cloned column vector elements should match original.");
                        }
                });
                Console.WriteLine($"Column vector clone created: {clonedCol.ToString()}.");

                Assert.Multiple(() =>
                {
                        Assert.AreNotSame(originalRow, clonedRow, "Cloned row vector should be a different instance.");
                        Assert.AreEqual(originalRow.Size, clonedRow.Size, "Cloned row vector size should match original.");
                        Assert.AreEqual(originalRow.Type, clonedRow.Type, "Cloned row vector type should match original.");
                        for (int i = 0; i < originalRow.Size; i++)
                        {
                                Assert.AreEqual(originalRow[i], clonedRow[i], "Cloned row vector elements should match original.");
                        }
                });
                Console.WriteLine($"Row vector clone created: {clonedRow.ToString()}.");

                clonedCol[0] = 99.0;
                Assert.AreNotEqual(originalCol[0], clonedCol[0], "Modifying cloned column vector should not affect original.");
                Console.WriteLine("Vector clone created a deep copy.");
        }

        [Test]
        public void Vector_OperatorMultiply_VectorByVector_DotProductMatrix()
        {
                Console.WriteLine("Testing Vector_OperatorMultiply_VectorByVector_DotProductMatrix.");

                EigenvalueFinder.Core.Vector left = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 }, VectorType.Row); // 1x3
                EigenvalueFinder.Core.Vector right = new EigenvalueFinder.Core.Vector(new double[] { 4, 5, 6 }, VectorType.Column); // 3x1
                double expectedDotProduct = 32.0;

                EigenvalueFinder.Core.Matrix resultMatrix = left * right; // Should be a 1x1 Matrix representing dot product

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(1, resultMatrix.RowCount);
                        Assert.AreEqual(1, resultMatrix.ColumnCount);
                        Assert.AreEqual(expectedDotProduct, resultMatrix[0, 0], 1e-9);
                });
                Console.WriteLine($"Vector * Vector (dot product) result: {resultMatrix[0,0]}");
        }

        [Test]
        public void Vector_OperatorMultiply_VectorByVector_OuterProductMatrix()
        {
                Console.WriteLine("Testing Vector_OperatorMultiply_VectorByVector_OuterProductMatrix.");

                EigenvalueFinder.Core.Vector left = new EigenvalueFinder.Core.Vector(new double[] { 1, 2 }, VectorType.Column); // 2x1
                EigenvalueFinder.Core.Vector right = new EigenvalueFinder.Core.Vector(new double[] { 3, 4 }, VectorType.Row); // 1x2
                EigenvalueFinder.Core.Matrix expectedOuterProduct = new EigenvalueFinder.Core.Matrix(new double[,] {
                    { 1*3, 1*4 },
                    { 2*3, 2*4 }
                }); // 2x2: {{3,4},{6,8}}

                EigenvalueFinder.Core.Matrix resultMatrix = left * right; // Should be a 2x2 Matrix (outer product)

                Assert.Multiple(() =>
                {
                        Assert.AreEqual(2, resultMatrix.RowCount);
                        Assert.AreEqual(2, resultMatrix.ColumnCount);
                        Assert.AreEqual(expectedOuterProduct[0,0], resultMatrix[0,0], 1e-9);
                        Assert.AreEqual(expectedOuterProduct[0,1], resultMatrix[0,1], 1e-9);
                        Assert.AreEqual(expectedOuterProduct[1,0], resultMatrix[1,0], 1e-9);
                        Assert.AreEqual(expectedOuterProduct[1,1], resultMatrix[1,1], 1e-9);
                });
                Console.WriteLine($"Vector * Vector (outer product) result: {resultMatrix.ToString()}");
        }

        [Test]
        public void Vector_OperatorMultiply_VectorByVector_ThrowsForIncompatibleSizes()
        {
                Console.WriteLine("Testing Vector_OperatorMultiply_VectorByVector_ThrowsForIncompatibleSizes.");

                EigenvalueFinder.Core.Vector left = new EigenvalueFinder.Core.Vector(new double[] { 1, 2, 3 }, VectorType.Column); // 3x1
                EigenvalueFinder.Core.Vector right = new EigenvalueFinder.Core.Vector(new double[] { 4, 5 }, VectorType.Column); // 2x1 (incompatible)

                Assert.Throws<ArgumentException>(new TestDelegate(() => { EigenvalueFinder.Core.Matrix result = left * right; }), "Vector * Vector should throw for incompatible dimensions.");
                Console.WriteLine("Vector * Vector correctly threw for incompatible sizes.");
        }
}
