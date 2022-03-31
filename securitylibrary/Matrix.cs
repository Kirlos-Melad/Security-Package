using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    enum Mode
    {
        Row,
        Column
    }
    class Matrix
    {
        private int[,] matrix;
        public int this[int row, int column]
        {
            get { return matrix[row, column]; }
            set { matrix[row, column] = value; }
        }

        public int rowSize { get; }
        public int columnSize { get; }

        public static readonly int MOD = 26;

        public Matrix(int[,] matrix)
        {
            this.matrix = matrix;
            rowSize = matrix.GetLength(0);
            columnSize = matrix.GetLength(1);
        }

        public static Matrix CreateMatrix(List<int> array, int rowSize, int columnSize, Mode mode)
        {
            int[,] mat = new int[rowSize, columnSize];

            int row = 0, column = 0;

            foreach (int element in array)
            {
                mat[row, column] = element;

                if (mode == Mode.Row)
                {
                    row = (row + 1) % rowSize;
                    if (row == 0)
                    {
                        column++;
                    }
                }
                else if (mode == Mode.Column)
                {
                    column = (column + 1) % columnSize;
                    if (column == 0)
                    {
                        row++;
                    }
                }
            }

            return new Matrix(mat);
        }

        public Matrix Clone()
        {
            return new Matrix((int[,])matrix.Clone());
        }

        public static Matrix CreateMatrix(List<int> array, Mode mode)
        {
            int size = (int)Math.Sqrt(array.Count);

            if (size != (int)Math.Ceiling(Math.Sqrt(array.Count)))
                throw new Exception(String.Format("Can't implicitly find the matrix dimensions for a list of size {0}", array.Count));

            return CreateMatrix(array, size, size, mode);
        }

        public static Matrix Identity(int n)
        {
            Matrix identity = new Matrix(new int[n, n]);
            for (int i = 0, j = 0; i < n; i++, j++)
            {
                identity[i, j] = 1;
            }

            return identity;
        }

        public List<int> GetRow(int row)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[row, x])
                .ToList();
        }

        public List<int> GetColumn(int column)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, column])
                .ToList();
        }

        public List<int> ToList(Mode mode)
        {
            int row = 0, column = 0;
            if (mode == Mode.Row)
            {
                row = this.columnSize;
                column = this.rowSize;
            }
            else if (mode == Mode.Column)
            {
                row = this.rowSize;
                column = this.columnSize;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    list.Add(Helper.Mod(this[j, i], MOD));
                }
            }

            return list;
        }

        public Matrix Add(Matrix other)
        {
            if (!((this.rowSize == other.rowSize) && (this.columnSize == other.columnSize)))
                throw new Exception(String.Format("Can't Add Matrix: {0}x{1} with Matrix: {2}x{3}", this.rowSize, this.columnSize, other.rowSize, other.columnSize));

            Matrix newMatrix = new Matrix(new int [rowSize, columnSize]);
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    newMatrix[i, j] = Helper.Mod((this[i, j] + other[i, j]), MOD);
                }
            }

            return newMatrix;
        }

        public Matrix Multiply(Matrix other)
        {
            if (this.columnSize != other.rowSize)
                throw new Exception(String.Format("Can't Multiply Matrix: {0}x{1} with Matrix: {2}x{3}", this.rowSize, this.columnSize, other.rowSize, other.columnSize));

            int rows = this.rowSize;
            int columns = other.columnSize;
            int common = this.columnSize;

            Matrix newMatrix = new Matrix(new int[rows, columns]);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    newMatrix[i, j] = 0;

                    for (int k = 0; k < common; k++)
                    {
                        newMatrix[i, j] += Helper.Mod(this[i, k] * other[k, j], MOD);
                    }
                }
            }

            return newMatrix;
        }

        public Matrix Multiply(int number)
        {
            Matrix newMatrix = this.Clone();

            for (int i = 0; i < newMatrix.rowSize; i++)
            {
                for (int j = 0; j < newMatrix.columnSize; j++)
                {
                    newMatrix[i, j] *= number; 
                }
            }

            return newMatrix;
        }

        public Matrix Transpose()
        {
            int[,] newMatrix = new int[columnSize, rowSize];

            for(int i = 0; i < rowSize; i++)
            {
                for(int j = 0; j < columnSize; j++)
                {
                    newMatrix[j, i] = matrix[i, j];
                }
            }

            return new Matrix(newMatrix);
        }

        private int Determinant2x2()
        {
            return (matrix[0, 0] * matrix[1, 1]) - (matrix[1, 0] * matrix[0, 1]);
        }

        private Matrix Adjoint2x2()
        {
            Matrix newMatrix = new Matrix(new int[2,2]);
            //swap
            newMatrix[0, 0] = this[1, 1];
            newMatrix[1, 1] = this[0, 0];
            //negative
            newMatrix[0, 1] = -this[0, 1];
            newMatrix[1, 0] = -this[1, 0];

            return newMatrix;
        }

        private int Determinant3x3()
        {
            int determinant = 0;
            for (int i = 0; i < 3; i++)
            {
                determinant += (matrix[0, i] * (matrix[1, (i + 1) % 3] * matrix[2, (i + 2) % 3] 
                    - matrix[1, (i + 2) % 3] * matrix[2, (i + 1) % 3]));
            }

            return determinant;
        }

        private Matrix Cofactor3x3()
        {
            Matrix newMatrix = new Matrix(new int[3, 3]);

            newMatrix[0, 0] = ((this[1, 1] * this[2, 2]) - (this[2, 1] * this[1, 2]));
            newMatrix[0, 1] = -1 * ((this[1, 0] * this[2, 2]) - (this[2, 0] * this[1, 2]));
            newMatrix[0, 2] = ((this[1, 0] * this[2, 1]) - (this[2, 0] * this[1, 1]));
            newMatrix[1, 0] = -1 * ((this[0, 1] * this[2, 2]) - (this[2, 1] * this[0, 2]));
            newMatrix[1, 1] = ((this[0, 0] * this[2, 2]) - (this[2, 0] * this[0, 2]));
            newMatrix[1, 2] = -1 * ((this[0, 0] * this[2, 1]) - (this[2, 0] * this[0, 1]));
            newMatrix[2, 0] = ((this[0, 1] * this[1, 2]) - (this[1, 1] * this[0, 2]));
            newMatrix[2, 1] = -1 * ((this[0, 0] * this[1, 2]) - (this[1, 0] * this[0, 2]));
            newMatrix[2, 2] = ((this[0, 0] * this[1, 1]) - (this[1, 0] * this[0, 1]));

            return newMatrix;
        }

        public Matrix Inverse()
        {
            if (rowSize != columnSize)
                throw new Exception("Can't find inverse of non-square matrix");
            if (!(rowSize == 2 || rowSize == 3))
                throw new Exception(String.Format("Can't find inverse of matrix{0}x{1}", rowSize, columnSize));

            Matrix newMatrix = new Matrix(new int[rowSize, rowSize]);

            if (rowSize == 2)
            {
                int determinant = this.Determinant2x2();
                if (determinant == 0)
                    throw new Exception("Can't find inverse of matrix with Zero determinant");
                determinant = Helper.NumberInverse(determinant, MOD);
                newMatrix = this.Adjoint2x2().Multiply(determinant);
            }
            else if(rowSize == 3)
            {
                int determinant = this.Determinant3x3();
                if (determinant == 0)
                    throw new Exception("Can't find inverse of matrix with Zero determinant");
                determinant = Helper.NumberInverse(determinant, MOD);
                Matrix adjoint = this.Cofactor3x3().Transpose();

                newMatrix = adjoint.Multiply(determinant);
            }

            return newMatrix;
        }
    }

}