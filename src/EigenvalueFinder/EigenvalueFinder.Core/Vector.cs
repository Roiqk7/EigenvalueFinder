using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Linq;
using System.Text;

namespace EigenvalueFinder.Core;
/// <summary>
/// Defines the type of vector: Column (N x 1) or Row (1 x N).
/// </summary>
public enum VectorType
{
        Column, // Represents an N x 1 matrix
        Row     // Represents a 1 x N matrix
}

public class Vector : Matrix
{
        private VectorType _vectorType;

        // --- Properties ---

        /// <summary>
        /// Gets the size (number of elements) of the vector.
        /// This is RowCount for a Column vector and ColumnCount for a Row vector.
        /// </summary>
        public int Size => _vectorType == VectorType.Column ? RowCount : ColumnCount;

        /// <summary>
        /// Gets the type of the vector (Column or Row).
        /// </summary>
        public VectorType Type => _vectorType;

        /// <summary>
        /// Provides access to vector elements by a single index.
        /// It accesses the first (and only) column/row of the underlying matrix based on vector type.
        /// </summary>
        public new double this[int index]
        {
                get => _vectorType == VectorType.Column ? base[index, 0] : base[0, index];
                set
                {
                        if (_vectorType == VectorType.Column)
                        {
                                base[index, 0] = value;
                        }
                        else
                        {
                                base[0, index] = value;
                        }
                }
        }

        // --- Constructors ---

        /// <summary>
        /// Creates a new vector with specified size and type, initialized to zeros.
        /// </summary>
        /// <param name="size">The number of elements in the vector.</param>
        /// <param name="type">The type of vector (Column or Row). Defaults to Column.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if size is non-positive.</exception>
        public Vector(int size, VectorType type = VectorType.Column)
                : base(type == VectorType.Column ? size : 1, type == VectorType.Column ? 1 : size)
        {
                if (size <= 0)
                {
                        throw new ArgumentOutOfRangeException(nameof(size), "Vector size must be positive.");
                }
                _vectorType = type;
        }

        /// <summary>
        /// Creates a vector from a 1D array of doubles, with specified type.
        /// </summary>
        /// <param name="data">The 1D array of vector elements.</param>
        /// <param name="type">The type of vector (Column or Row). Defaults to Column.</param>
        /// <exception cref="ArgumentNullException">Thrown if the data array is null.</exception>
        public Vector(double[] data, VectorType type = VectorType.Column)
                : base(type == VectorType.Column ? (data == null ? 1 : data.Length) : 1, type == VectorType.Column ? 1 : (data == null ? 1 : data.Length))
        {
                if (data == null)
                {
                        throw new ArgumentNullException(nameof(data), "Data array cannot be null.");
                }
                _vectorType = type;
                // Populate the internal matrix from the 1D array using the base class indexer.
                for (int i = 0; i < data.Length; i++)
                {
                        if (_vectorType == VectorType.Column)
                        {
                                this[i, 0] = data[i];
                        }
                        else
                        {
                                this[0, i] = data[i];
                        }
                }
        }

        /// <summary>
        /// Wraps an existing Math.NET Numerics DenseMatrix instance as a Vector, inferring its type.
        /// The DenseMatrix must be N x 1 (Column) or 1 x N (Row).
        /// </summary>
        /// <param name="denseMatrix">The Math.NET DenseMatrix to wrap.</param>
        /// <exception cref="ArgumentNullException">Thrown if the matrix is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the matrix is not N x 1 or 1 x N.</exception>
        public Vector(DenseMatrix denseMatrix) : base(denseMatrix)
        {
                if (denseMatrix == null)
                {
                        throw new ArgumentNullException(nameof(denseMatrix), "DenseMatrix cannot be null.");
                }
                if (denseMatrix.ColumnCount == 1 && denseMatrix.RowCount >= 1)
                {
                        _vectorType = VectorType.Column;
                }
                else if (denseMatrix.RowCount == 1 && denseMatrix.ColumnCount >= 1)
                {
                        _vectorType = VectorType.Row;
                }
                else
                {
                        throw new ArgumentException("A DenseMatrix can only be wrapped as a Vector if it has exactly one row (1 x N) or one column (N x 1).", nameof(denseMatrix));
                }
        }

        /// <summary>
        /// Private constructor for internal use, wraps a DenseMatrix with an explicitly provided type.
        /// Used in operations like Transpose where the resulting vector type is definitively known.
        /// </summary>
        /// <param name="denseMatrix">The Math.NET DenseMatrix to wrap.</param>
        /// <param name="type">The explicit VectorType for the new vector.</param>
        /// <exception cref="ArgumentNullException">Thrown if the matrix is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the matrix dimensions do not match the provided type.</exception>
        private Vector(DenseMatrix denseMatrix, VectorType type) : base(denseMatrix)
        {
                if (denseMatrix == null)
                {
                        throw new ArgumentNullException(nameof(denseMatrix), "DenseMatrix cannot be null.");
                }

                // Validate that the denseMatrix's dimensions actually match the provided type
                if (type == VectorType.Column && denseMatrix.ColumnCount != 1)
                {
                        throw new ArgumentException("DenseMatrix must have one column for a Column vector type.", nameof(denseMatrix));
                }
                if (type == VectorType.Row && denseMatrix.RowCount != 1)
                {
                        throw new ArgumentException("DenseMatrix must have one row for a Row vector type.", nameof(denseMatrix));
                }
                _vectorType = type;
        }

        // --- Operator Overloading ---

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (double * Vector).
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="vector">The vector to multiply.</param>
        /// <returns>A new Vector with each element scaled by the scalar.</returns>
        public static Vector operator *(double scalar, Vector vector)
        {
                return new Vector(vector.ToDenseMatrix().Multiply(scalar) as DenseMatrix, vector.Type);
        }

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (Vector * double).
        /// </summary>
        /// <param name="vector">The vector to multiply.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new Vector with each element scaled by the scalar.</returns>
        public static Vector operator *(Vector vector, double scalar)
        {
                return scalar * vector; // Calls the other overload for consistency
        }

        /// <summary>
        /// Overloads the multiplication operator for Vector-Vector multiplication (general matrix multiplication).
        /// For dot product, use the DotProduct() method.
        /// </summary>
        /// <param name="left">The left-hand side vector.</param>
        /// <param name="right">The right-hand side vector.</param>
        /// <returns>A new Matrix representing the product.</returns>
        /// <remarks>
        /// This operator performs standard matrix multiplication using the underlying DenseMatrix objects.
        /// If 'left' is N x 1 (column) and 'right' is 1 x M (row), this yields an N x M matrix (outer product).
        /// If 'left' is 1 x N (row) and 'right' is N x 1 (column), this yields a 1 x 1 matrix (dot product as a matrix).
        /// Other combinations will result in dimension mismatch errors from Math.NET.
        /// </remarks>
        public static Matrix operator *(Vector left, Vector right)
        {
                return new Matrix(left.ToDenseMatrix().Multiply(right.ToDenseMatrix()) as DenseMatrix);
        }

        /// <summary>
        /// Overloads the addition operator for Vector-Vector addition.
        /// </summary>
        /// <param name="left">The left-hand side vector.</param>
        /// <param name="right">The right-hand side vector.</param>
        /// <returns>A new Vector representing the sum.</returns>
        /// <exception cref="ArgumentException">Thrown if vectors have different sizes or types.</exception>
        public static Vector operator +(Vector left, Vector right)
        {
                if (left.Size != right.Size || left.Type != right.Type)
                {
                        throw new ArgumentException("Vectors must have the same size and type for addition.");
                }
                // Perform the addition using the base Matrix operator and wrap the result back into a Vector
                return new Vector(left.ToDenseMatrix().Add(right.ToDenseMatrix()) as DenseMatrix, left.Type);
        }

        /// <summary>
        /// Overloads the subtraction operator for Vector-Vector subtraction.
        /// </summary>
        /// <param name="left">The left-hand side vector.</param>
        /// <param name="right">The right-hand side vector.</param>
        /// <returns>A new Vector representing the difference.</returns>
        /// <exception cref="ArgumentException">Thrown if vectors have different sizes or types.</exception>
        public static Vector operator -(Vector left, Vector right)
        {
            if (left.Size != right.Size || left.Type != right.Type)
            {
                throw new ArgumentException("Vectors must have the same size and type for subtraction.");
            }
            // Perform the subtraction using the base Matrix operator and wrap the result back into a Vector
            return new Vector(left.ToDenseMatrix().Subtract(right.ToDenseMatrix()) as DenseMatrix, left.Type);
        }

        // --- Vector-Specific Operations ---

        /// <summary>
        /// Computes the dot product of this vector with another vector.
        /// Ensures one vector is in row form (1xN) and the other in column form (Nx1) for multiplication.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The dot product as a double.</returns>
        /// <exception cref="ArgumentException">Thrown if vectors have different sizes.</exception>
        /// <exception cref="InvalidOperationException">Thrown if internal logic errors lead to non-1x1 result.</exception>
        public double DotProduct(Vector other)
        {
                if (Size != other.Size)
                {
                        throw new ArgumentException("Vectors must have the same size for dot product.");
                }

                // Get the underlying DenseMatrix representations of 'this' and 'other'
                DenseMatrix thisDenseMatrix = ToDenseMatrix();
                DenseMatrix otherDenseMatrix = other.ToDenseMatrix();

                // Use the base Matrix<double> type for intermediate multiplication results
                MathNet.Numerics.LinearAlgebra.Matrix<double> thisForMultiplication;
                MathNet.Numerics.LinearAlgebra.Matrix<double> otherForMultiplication;

                // Ensure correct dimensions for multiplication (1xN * Nx1)
                if (_vectorType == VectorType.Column)
                {
                        thisForMultiplication = thisDenseMatrix.Transpose();
                }
                else // VectorType.Row
                {
                        thisForMultiplication = thisDenseMatrix;
                }

                if (other._vectorType == VectorType.Row)
                {
                        otherForMultiplication = otherDenseMatrix.Transpose();
                }
                else // VectorType.Column
                {
                        otherForMultiplication = otherDenseMatrix;
                }

                // Perform the multiplication (1xN) * (Nx1)
                // The Multiply method returns MathNet.Numerics.LinearAlgebra.Matrix<double>
                MathNet.Numerics.LinearAlgebra.Matrix<double> resultMatrixMathNet = thisForMultiplication.Multiply(otherForMultiplication);

                // The result should always be a 1x1 matrix for a dot product.
                // Access the element directly from the MathNet.Numerics.LinearAlgebra.Matrix<double>
                if (resultMatrixMathNet.RowCount != 1 || resultMatrixMathNet.ColumnCount != 1)
                {
                        throw new InvalidOperationException("Dot product resulted in a matrix larger than 1x1. This indicates an internal logic error.");
                }
                return resultMatrixMathNet[0, 0];
        }

        /// <summary>
        /// Computes the Euclidean (L2) norm of the vector.
        /// For an N x 1 or 1 x N matrix, the Frobenius norm of the matrix is equivalent to the L2 norm of the vector.
        /// </summary>
        public double L2Norm()
        {
                return ToDenseMatrix().FrobeniusNorm();
        }

        /// <summary>
        /// Normalizes the vector to unit Euclidean (L2) length.
        /// </summary>
        /// <returns>A new Vector instance representing the normalized vector.</returns>
        /// <exception cref="InvalidOperationException">Thrown when attempting to normalize a zero vector, as its direction is undefined.</exception>
        public Vector Normalize()
        {
                double norm = L2Norm();
                if (norm == 0.0)
                {
                        throw new InvalidOperationException("Cannot normalize a zero vector.");
                }
                return new Vector(ToDenseMatrix().Divide(norm) as DenseMatrix);
        }

        /// <summary>
        /// Transposes the vector. This changes a column vector to a row vector and vice-versa.
        /// </summary>
        /// <returns>A new Vector representing the transposed vector.</returns>
        public new Vector Transpose()
        {
                DenseMatrix transposedDenseMatrix = ToDenseMatrix().Transpose() as DenseMatrix;

                // Determine the new VectorType based on the original type for explicit setting.
                VectorType newType = (_vectorType == VectorType.Column) ? VectorType.Row : VectorType.Column;

                return new Vector(transposedDenseMatrix, newType);
        }

        // --- Cloning ---

        /// <summary>
        /// Creates a deep copy of the current Vector instance.
        /// </summary>
        /// <returns>A new Vector object that is a deep copy of this instance.</returns>
        public new Vector Clone()
        {
                return new Vector(base.Clone().ToDenseMatrix());
        }

        // --- Representation ---

        /// <summary>
        /// Returns a formatted string representation of the vector.
        /// </summary>
        public override string ToString()
        {
                // Uses the vector indexer to build a string, similar to a standard vector representation.
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < Size; i++)
                {
                        sb.Append($"{this[i]:F4}");
                        if (i < Size - 1)
                        {
                                sb.Append(", ");
                        }
                }
                sb.Append("]");
                return sb.ToString();
        }
}
