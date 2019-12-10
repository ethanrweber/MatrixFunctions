using System;

namespace MatrixFunctions
{
    /// <summary>
    /// Library of matrix functions
    /// implementations are non-generic and specify type decimal to ensure reliability and accuracy
    /// </summary>
    public static class MatrixExtensions
    {
        static void Main(string[] args)
        {
            Matrix m = new Matrix(new decimal[,] { { 1, -4, 2 }, { -2, 8, -9 }, { -1, 7, 0 } }) ;
            decimal[,] m2 = {{1, 2, -1}, {2, 3, -1}, {-2, 0, -3}};
            decimal[,] m3 = {{1, 2, -1, 4}, {-2, 1, 7, 2}, {-1, -4, -1, 3}, {3, 2, -7, -1}};
            Matrix mm = new Matrix(m2);
            // test copy

            PrintMatrix(mm * mm);
            
            Console.ReadLine();
        }

        private static decimal[,] _Deepcopy(decimal[,] matrix)
        {
            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            decimal[,] copy = new decimal[n,m];
            for(int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    copy[i, j] = matrix[i, j];
            return copy;
        }

        /// <summary>
        /// returns the given matrix in reduced row echelon form using Gaussian Elimination
        /// </summary>
        /// <param name="A">n x m matrix</param>
        /// <returns>n x m matrix in reduced row echelon form</returns>
        public static decimal[,] RREF(decimal[,] A)
        {
            decimal[,] matrix = _Deepcopy(A);

            int lead = 0, 
                rowCount = matrix.GetLength(0), 
                columnCount = matrix.GetLength(1);

            for (int r = 0; r < rowCount; r++)
            {
                if (columnCount <= lead)
                    break;
                int i = r;
                while (matrix[i, lead] == 0)
                {
                    i++;
                    if (rowCount == i)
                    {
                        i = r;
                        lead++;
                        if (columnCount == lead)
                        {
                            lead--;
                            break;
                        }
                    }
                }

                for (int k = 0; k < columnCount; k++)
                {
                    decimal temp = matrix[i, k];
                    matrix[i, k] = matrix[r, k];
                    matrix[r, k] = temp;
                }

                var div = matrix[r, lead];
                if (div != 0)
                    for (int k = 0; k < columnCount; k++)
                        matrix[r, k] /= div;

                for (int j = 0; j < rowCount; j++)
                    if (j != r)
                    {
                        var mult = matrix[j, lead];
                        for (int k = 0; k < columnCount; k++)
                        {
                            matrix[j, k] = matrix[j,k] -  mult * matrix[r, k];
                            if (matrix[j, k] == (int) matrix[j, k])
                                matrix[j, k] = (int) matrix[j, k]; // remove rounding issue
                        }
                    }

                lead++;
            }

            return matrix;
        }

        /// <summary>
        /// prints a matrix to console output
        /// </summary>
        /// <param name="matrix">matrix to be printed</param>
        /// <param name="round">number of decimal points to round answers, default 2</param>
        public static void PrintMatrix(Matrix matrix, int round = 2) => PrintMatrix(matrix.grid, round);

        /// <summary>
        /// prints a matrix to console output
        /// </summary>
        /// <param name="matrix">matrix to be printed</param>
        /// <param name="round">number of decimal points to round answers, default 2</param>
        public static void PrintMatrix(decimal[,] matrix, int round = 2)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write(decimal.Round(matrix[i, j], round) + "\t");
                Console.WriteLine();
            }
        }


        /// <summary>
        /// computes the determinant of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static decimal GetDeterminant(decimal[,] matrix)
        {
            int n = matrix.GetLength(0);

            if (n == 0) throw new Exception("matrix cannot be empty");
            if (n == 1) return matrix[0, 0];

            decimal det = 0;
            for(int j = 0; j < n; j++)
                det += (decimal)Math.Pow(-1, 1 + j) * matrix[1, j] * GetDeterminant(GetSubmatrix(matrix, 1, j));
            
            return det;
        }

        /// <summary>
        /// retrieves a submatrix of the parent matrix, useful for Determinant calculations
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>returns an (m-1) x (n-1) sub-matrix excluding row 'row' and column 'col'</returns>
        public static decimal[,] GetSubmatrix(decimal[,] parent, int row, int col)
        {
            int pn = parent.GetLength(0);
            if (pn == 0) throw new Exception("matrix must have values");
            if (pn == 1) return new decimal[0,0];

            int n = pn - 1, rOffset = 0, cOffset = 0;

            decimal[,] matrix = new decimal[n, n];
            for (int i = 0; i < n; i++)
            {
                if (row == i)
                    rOffset = 1;
                for (int j = 0; j < n; j++)
                {
                    if (col == j)
                        cOffset = 1;
                    matrix[i, j] = parent[i + rOffset, j + cOffset];
                }

                cOffset = 0;
            }

            return matrix;
        }

        /// <summary>
        /// generates an identity matrix of size n x n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static decimal[,] GetIdentity(int n)
        {
            decimal[,] identity = new decimal[n,n];
            for (int i = 0; i < n; i++)
                identity[i, i] = 1;
            return identity;
        }

        /// <summary>
        /// computes the transpose of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static decimal[,] GetTranspose(decimal[,] matrix)
        {
            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            decimal[,] transpose = new decimal[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    transpose[i, j] = matrix[j, i];
            return transpose;
        }

        public static decimal[,] GetInverse(decimal[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n != matrix.GetLength(1))
                throw new Exception("cannot inverse a non- n x n matrix");

            decimal[,] toRref = new decimal[n, 2 * n];
            for(int i = 0; i < n; i++)
            { 
                for(int j = 0; j < n; j++)
                    toRref[i,j] = matrix[i,j];

                for (int j = n; j < 2 * n; j++)
                    toRref[i, j] = 0;
                toRref[i, i + n] = 1;
            }

            decimal[,] rref = RREF(toRref);

            decimal[,] result = new decimal[n,n];
            for(int i = 0; i < n; i++)
                for (int j = n; j < 2 * n; j++)
                    result[i, j - n] = rref[i, j];

            return result;
        }

        //public static decimal GetDimension(decimal[,] matrix)
        //{

        //}

        /// <summary>
        /// returns the basis of the column space of a matrix as a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static decimal[,] GetColumnSpaceBasis(decimal[,] matrix)
        {
            decimal[,] rref = RREF(matrix);
            bool[] pivotCols = new bool[rref.GetLength(1)];
            for (int j = 0; j < rref.GetLength(1); j++)
            {
                bool pivotCol = false, foundOne = false;
                for (int i = 0; i < rref.GetLength(0); i++)
                {
                    if (!foundOne)
                    {
                        if (rref[i, j] != 1)
                            continue;
                        foundOne = true;
                    }

                    pivotCol = true;
                    if (rref[i, j] != 0)
                    {
                        pivotCol = false;
                        break;
                    }
                }

                pivotCols[j] = pivotCol;
            }
            return new decimal[0,0];
        }

        public static decimal[,] GetRowSpace(decimal[,] matrix) => GetTranspose(matrix);
        //public static decimal[,] GetNullSpace(decimal[,] matrix)
        //{

        //}


        /// <summary>
        /// determines if the column vectors of a given matrix are linearly independent
        /// by computing the determinant of the matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>true if the column vectors of the matrix are linearly independent, else false</returns>
        public static bool IsLinearlyIndependent(this decimal[,] matrix)
        {
            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            // more column vectors than equations means linearly dependent
            if (m > n) return false;

            // compare rref to identity matrix
            var rref = RREF(matrix);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    if (rref[i, j] != 0)
                        return false;
                }

            return true;
        } 
        
    }
}