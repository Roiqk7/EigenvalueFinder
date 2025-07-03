using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;
using System.Text;

namespace EigenvalueFinder.Core;

public class Matrix
{
        // The private Math.NET Numerics DenseMatrix instance that this class envelops.
        private DenseMatrix m_internalMatrix;

        // --- Properties ---

        public int RowCount => m_internalMatrix.RowCount;
        public int ColumnCount => m_internalMatrix.ColumnCount;

        // Indexer for easy element access (like a 2D array)
        public Complex this[int row, int col]
        {
                get => m_internalMatrix[row, col];
                set => m_internalMatrix[row, col] = value;
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
                if (rows <= 0)
                {
                        throw new ArgumentOutOfRangeException(nameof(rows), "Matrix row count must be positive.");
                }
                if (columns <= 0)
                {
                        throw new ArgumentOutOfRangeException(nameof(columns), "Matrix column count must be positive.");
                }
                m_internalMatrix = DenseMatrix.Create(rows, columns, Complex.Zero);
        }

        /// <summary>
        /// Creates a matrix from a 2D array of doubles.
        /// </summary>
        /// <param name="data">The 2D array of matrix elements.</param>
        /// <exception cref="ArgumentNullException">Thrown if the data array is null.</exception>
        public Matrix(Complex[,] data)
        {
                m_internalMatrix = DenseMatrix.OfArray(data ?? throw new ArgumentNullException(nameof(data), "Data array cannot be null."));
        }

        /// <summary>
        /// Wraps an existing Math.NET Numerics DenseMatrix instance.
        /// This constructor is crucial for operations that return DenseMatrix results from Math.NET.
        /// </summary>
        /// <param name="matrix">The Math.NET DenseMatrix to wrap.</param>
        /// <exception cref="ArgumentNullException">Thrown if the internal matrix is null.</exception>
        public Matrix(DenseMatrix matrix)
        {
                m_internalMatrix = matrix ?? throw new ArgumentNullException(nameof(matrix), "Matrix cannot be null.");
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

        /// <summary>
        /// Creates a new standard basis vector (a vector with a single '1' at a specified position and zeros elsewhere).
        /// </summary>
        /// <param name="size">The length of the vector (e.g., for a vector of 5 elements, size would be 5).</param>
        /// <param name="index">The zero-based index where the '1' should be placed (e.g., for the 3rd element, index would be 2).</param>
        /// <returns>A new Matrix instance representing the standard basis vector.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if size is non-positive .</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if index is out of bounds.</exception>
        public static Matrix Identity(int size, int index)
        {
                if (size <= 0)
                {
                        throw new ArgumentOutOfRangeException(nameof(size), "Vector size must be positive.");
                }
                if (index < 0 || index >= size)
                {
                        throw new ArgumentOutOfRangeException(nameof(index), message: "Index must be within the bounds of the vector size (0 to size - 1).");
                }

                DenseMatrix internalVector = DenseMatrix.Create(size, 1, Complex.Zero);
                internalVector[index, 0] = Complex.One;

                return new Matrix(internalVector);
        }

        // --- Operator Overloading ---

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix instance representing the product of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if matrix dimensions are incompatible for multiplication or if the result is unexpected.</exception>
        public static Matrix operator *(Matrix left, Matrix right)
        {
                if (left is null)
                {
                        throw new ArgumentNullException(nameof(left));
                }
                if (right is null)
                {
                        throw new ArgumentNullException(nameof(right));
                }

                return new Matrix(
                        left.m_internalMatrix.Multiply(right.m_internalMatrix) as DenseMatrix
                        ?? throw new InvalidOperationException("Matrix multiplication failed.")
                );
        }

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (Complex * Matrix).
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <returns>A new Matrix with each element scaled by the scalar.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if multiplication fails or if the result is unexpected.</exception>
        public static Matrix operator *(Complex scalar, Matrix matrix)
        {
                if (matrix is null)
                {
                        throw new ArgumentNullException(nameof(matrix));
                }

                return new Matrix(
                        matrix.m_internalMatrix.Multiply(scalar) as DenseMatrix
                        ?? throw new InvalidOperationException("Matrix scalar multiplication failed.")
                );
        }

        /// <summary>
        /// Overloads the multiplication operator for scalar multiplication (Matrix * Complex).
        /// </summary>
        /// <param name="matrix">The matrix to multiply.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>A new Matrix with each element scaled by the scalar.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if multiplication fails or if the result is unexpected.</exception>
        public static Matrix operator *(Matrix matrix, Complex scalar)
        {
                return scalar * matrix;
        }

        /// <summary>
        /// Overloads the addition operator for Matrix-Matrix addition.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix instance representing the sum of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if matrix dimensions are incompatible for addition or if the result is unexpected.</exception>
        public static Matrix operator +(Matrix left, Matrix right)
        {
                if (left is null)
                {
                        throw new ArgumentNullException(nameof(left));
                }
                if (right is null)
                {
                        throw new ArgumentNullException(nameof(right));
                }
                if (!HaveSameDimensions(left, right))
                {
                        throw new InvalidOperationException("Matrix dimensions mismatch.");
                }

                return new Matrix(
                        left.m_internalMatrix.Add(right.m_internalMatrix) as DenseMatrix
                        ?? throw new InvalidOperationException("Matrix addition failed.")
                );
        }

        /// <summary>
        /// Overloads the subtraction operator for Matrix-Matrix addition.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>A new Matrix instance representing the difference of the two matrices.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if matrix dimensions are incompatible for subtraction or if the result is unexpected.</exception>
        public static Matrix operator -(Matrix left, Matrix right)
        {
                if (left is null)
                {
                        throw new ArgumentNullException(nameof(left));
                }
                if (right is null)
                {
                        throw new ArgumentNullException(nameof(right));
                }
                if (!HaveSameDimensions(left, right))
                {
                        throw new InvalidOperationException("Matrix dimensions mismatch.");
                }

                return new Matrix(
                        left.m_internalMatrix.Subtract(right.m_internalMatrix) as DenseMatrix
                        ?? throw new InvalidOperationException("Matrix subtraction failed likely due to dimensions mismatch.")
                );
        }

        // --- Additional matrix operations ---

        /// <summary>
        /// Transposes the matrix.
        /// </summary>
        /// <returns>A new Matrix representing the transpose.</returns>
        /// <exception cref="NullReferenceException">Thrown if matrix transposition failed.</exception>
        public Matrix Transpose()
        {
                return new Matrix(
                        m_internalMatrix.Transpose() as DenseMatrix
                        ?? throw new ArgumentNullException(nameof(m_internalMatrix), "Matrix transposition failed.")
                );
        }

        // --- Implicit Conversion Operators ---

        /// <summary>
        /// Implicitly converts a 1x1 Matrix to a Complex.
        /// This allows the result of a dot product (which is a 1x1 matrix) to be directly used as a scalar.
        /// </summary>
        /// <param name="matrix">The Matrix to convert.</param>
        /// <returns>The single Complex value from the 1x1 matrix.</returns>
        /// <exception cref="NullReferenceException">Thrown if matrix is null.</exception>
        /// <exception cref="InvalidCastException">Thrown if the matrix is not 1x1.</exception>
        public static implicit operator Complex(Matrix matrix)
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

        // --- Additional methods

        /// <summary>
        /// Checks if the input matrices have same dimensions.
        /// </summary>
        /// <param name="left">The left-hand side matrix.</param>
        /// <param name="right">The right-hand side matrix.</param>
        /// <returns>Whether the input matrices have the same dimensions.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either input matrix is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if matrix dimensions are incompatible for subtraction or if the result is unexpected.</exception>
        private static bool HaveSameDimensions(Matrix left, Matrix right)
        {
                if (left is null)
                {
                        throw new ArgumentNullException(nameof(left));
                }
                if (right is null)
                {
                        throw new ArgumentNullException(nameof(right));
                }

                return left.RowCount == right.RowCount && left.ColumnCount == right.ColumnCount;
        }

        /// <summary>
        /// Creates a deep copy of the current Matrix instance.
        /// </summary>
        /// <returns>A new Matrix object that is a deep copy of this instance.</returns>
        public Matrix Clone()
        {
                return new Matrix(
                        m_internalMatrix.Clone() as DenseMatrix
                        ?? throw new ArgumentNullException(nameof(m_internalMatrix), "Matrix cloning failed.")
                );
        }

        /// <summary>
        /// Returns the underlying Math.NET Numerics DenseMatrix instance.
        /// This allows for direct access to Math.NET functionalities when needed.
        /// </summary>
        /// <returns>The internal DenseMatrix.</returns>
        public DenseMatrix GetInternalMatrix()
        {
                return m_internalMatrix;
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
                                sb.Append($"{m_internalMatrix[r, c].ToString("R")}\t");
                        }
                        sb.AppendLine();
                }
                return sb.ToString();
        }
}
