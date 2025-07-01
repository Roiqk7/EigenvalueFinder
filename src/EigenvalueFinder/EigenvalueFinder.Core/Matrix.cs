using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Data;
using System.Text;

namespace EigenvalueFinder.Core;

public class Matrix
{
        // The private Math.NET Numerics DenseMatrix instance that this class envelops.
        private DenseMatrix _internalMatrix;

        // --- Properties ---

        public int RowCount => _internalMatrix.RowCount;
        public int ColumnCount => _internalMatrix.ColumnCount;

        // Indexer for easy element access (like a 2D array)
        public double this[int row, int col]
        {
                get => _internalMatrix[row, col];
                set => _internalMatrix[row, col] = value;
        }

        // --- Constructors ---

        /// <summary>
        /// Creates a new matrix with specified dimensions, initialized to zeros.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if dimensions are non-positive.</exception>
        public Matrix(int rows, int columns)
        {
                if (rows <= 0 || columns <= 0)
                {
                        throw new ArgumentOutOfRangeException("Matrix dimensions must be positive.");
                }
                _internalMatrix = DenseMatrix.Create(rows, columns, 0.0);
        }

        /// <summary>
        /// Creates a matrix from a 2D array of doubles.
        /// </summary>
        /// <param name="data">The 2D array of matrix elements.</param>
        /// <exception cref="ArgumentNullException">Thrown if the data array is null.</exception>
        public Matrix(double[,] data)
        {
                _internalMatrix = DenseMatrix.OfArray(data ?? throw new ArgumentNullException(nameof(data), "Data array cannot be null."));
        }

        /// <summary>
        /// Wraps an existing Math.NET Numerics DenseMatrix instance.
        /// This constructor is crucial for operations that return DenseMatrix results from Math.NET.
        /// </summary>
        /// <param name="matrix">The Math.NET DenseMatrix to wrap.</param>
        /// <exception cref="ArgumentNullException">Thrown if the internal matrix is null.</exception>
        public Matrix(DenseMatrix matrix)
        {
                _internalMatrix = matrix ?? throw new ArgumentNullException(nameof(matrix), "Matrix cannot be null.");
        }

        /// <summary>
        /// Creates a new identity matrix of the specified size.
        /// An identity matrix is a square matrix with ones on the main diagonal and zeros elsewhere.
        /// </summary>
        /// <param name="size">The dimension of the square identity matrix (e.g., for a 3x3 identity matrix, size would be 3).</param>
        /// <returns>A new Matrix instance representing the identity matrix.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the size is non-positive.</exception>
        public static Matrix Identity(int size)
        {
                if (size <= 0)
                {
                        throw new ArgumentOutOfRangeException(nameof(size), "Identity matrix size must be positive.");
                }
                return new Matrix(DenseMatrix.CreateIdentity(size));
        }

        // --- Operator Overloading ---

        /// <summary>
        /// Overloads the multiplication operator for Matrix-Matrix multiplication.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix representing the product.</returns>
        /// <exception cref="ArgumentException">Thrown if matrix dimensions are incompatible for multiplication.</exception>
        public static Matrix operator *(Matrix left, Matrix right)
        {
                if (left.ColumnCount != right.RowCount)
                {
                        throw new ArgumentException("Matrix dimensions must be compatible for multiplication (left.ColumnCount == right.RowCount).");
                }
                return new Matrix(left._internalMatrix.Multiply(right._internalMatrix) as DenseMatrix);
        }

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (Matrix * double).
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new Matrix with each element scaled by the scalar.</returns>
        public static Matrix operator *(Matrix matrix, double scalar)
        {
                return new Matrix(matrix._internalMatrix.Multiply(scalar) as DenseMatrix);
        }

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (double * Matrix).
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <returns>A new Matrix with each element scaled by the scalar.</returns>
        public static Matrix operator *(double scalar, Matrix matrix)
        {
                return matrix * scalar;
        }

        /// <summary>
        /// Overloads the multiplication operator for Matrix-Vector multiplication (Matrix * Vector).
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>A new Vector representing the product.</returns>
        /// <exception cref="ArgumentException">Thrown if matrix columns do not match vector size.</exception>
        public static Vector operator *(Matrix matrix, Vector vector)
        {
                if (matrix.ColumnCount != vector.Size)
                {
                        throw new ArgumentException("Matrix columns must match vector size for multiplication.");
                }
                return new Vector(matrix._internalMatrix.Multiply(vector.ToDenseMatrix()) as DenseMatrix);
        }

        /// <summary>
        /// Overloads the addition operator for Matrix-Matrix addition.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix representing the sum.</returns>
        /// <exception cref="ArgumentException">Thrown if matrices have different dimensions.</exception>
        public static Matrix operator +(Matrix left, Matrix right)
        {
                if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                {
                        throw new ArgumentException("Matrices must have the same dimensions for addition.");
                }
                return new Matrix(left._internalMatrix.Add(right._internalMatrix) as DenseMatrix);
        }

        /// <summary>
        /// Overloads the subtraction operator for Matrix-Matrix subtraction.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix representing the difference.</returns>
        /// <exception cref="ArgumentException">Thrown if matrices have different dimensions.</exception>
        public static Matrix operator -(Matrix left, Matrix right)
        {
                if (left.RowCount != right.RowCount || left.ColumnCount != right.ColumnCount)
                {
                        throw new ArgumentException("Matrices must have the same dimensions for subtraction.");
                }
                return new Matrix(left._internalMatrix.Subtract(right._internalMatrix) as DenseMatrix);
        }

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <returns>A new Matrix representing the transpose.</returns>
        public Matrix Transpose()
        {
                return new Matrix(_internalMatrix.Transpose() as DenseMatrix);
        }

        /// <summary>
        /// Creates a deep copy of the current Matrix instance.
        /// </summary>
        /// <returns>A new Matrix object that is a deep copy of this instance.</returns>
        public Matrix Clone()
        {
                return new Matrix(_internalMatrix.Clone() as DenseMatrix);
        }

        // --- Public access to internal Math.NET object ---

        /// <summary>
        /// Provides direct access to the underlying Math.NET Numerics DenseMatrix.
        /// This is useful when interfacing with Math.NET functions that expect a DenseMatrix (e.g., Evd).
        /// </summary>
        public DenseMatrix ToDenseMatrix()
        {
                return _internalMatrix;
        }

        // --- Implicit Conversion Operators ---

        /// <summary>
        /// Implicitly converts a 1x1 Matrix to a double.
        /// This allows the result of a dot product (which is a 1x1 matrix) to be directly used as a scalar.
        /// </summary>
        /// <param name="matrix">The Matrix to convert.</param>
        /// <returns>The single double value from the 1x1 matrix.</returns>
        /// <exception cref="InvalidCastException">Thrown if the matrix is not 1x1.</exception>
        public static implicit operator double(Matrix matrix)
        {
                if (matrix == null)
                {
                        throw new ArgumentNullException(nameof(matrix), "Cannot implicitly convert a null Matrix to double.");
                }
                if (matrix.RowCount == 1 && matrix.ColumnCount == 1)
                {
                        return matrix[0, 0];
                }
                throw new InvalidCastException($"Matrix must be 1x1 to be implicitly converted to a double. Current dimensions: {matrix.RowCount}x{matrix.ColumnCount}");
        }

        // --- Representation ---

        /// <summary>
        /// Returns a formatted string representation of the matrix.
        /// </summary>
        public override string ToString()
        {
                StringBuilder sb = new StringBuilder();
                for (int r = 0; r < RowCount; r++)
                {
                        for (int c = 0; c < ColumnCount; c++)
                        {
                                sb.Append($"{_internalMatrix[r, c]:F4}\t");
                        }
                        sb.AppendLine();
                }
                return sb.ToString();
        }
}
