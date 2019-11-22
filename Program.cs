using System;

namespace MatrixFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal[,] m = {{1, -4, 2}, {-2, 8, -9}, {-1, 7, 0}};
            Console.WriteLine("m:");
            PrintMatrix(m);
            Console.WriteLine("rref(m):");
            PrintMatrix(RREF(m));
            Console.ReadLine();
        }

        /// <summary>
        /// returns the given matrix in reduced row echelon form using Gaussian Elimination
        /// </summary>
        /// <param name="matrix">n x m matrix</param>
        /// <returns>n x m matrix in reduced row echelon form</returns>
        public static decimal[,] RREF(decimal[,] matrix)
        {
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
                            return matrix;
                    }
                }
                // swap rows i and r
                for (int k = 0; k < columnCount; k++)
                {
                    decimal temp = matrix[i, k];
                    matrix[i, k] = matrix[r, k];
                    matrix[r, k] = temp;
                }

                if (matrix[r, lead] != 0)
                {
                    for (int k = 0; k < columnCount; k++)
                    {
                        matrix[i, k] /= matrix[r, lead];
                    }
                }

                for (i = 0; i < rowCount; i++)
                {
                    // subtract row r * m[i, lead] from row i
                    if (i != r)
                    {
                        for (int k = 0; k < columnCount; k++)
                        {
                            matrix[i, k] -= matrix[i, lead] * matrix[r, k];
                        }
                    }
                }

                lead++;
            }

            return matrix;
        }

        public static void PrintMatrix<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i,j] + "\t");
                }

                Console.WriteLine();
            }
        }

        public static int[,] MultiplyMatrices(int[,] a, int[,] b)
        {
            if(a.GetLength(1) != b.GetLength(0))
                throw new Exception("cannot multiply matrices of different sizes");


            int m = a.GetLength(0), n = b.GetLength(1);
            int[,] result = new int[a.GetLength(0), b.GetLength(1)];


            // iterate through result rows
            for(int i = 0; i < m; i++)
            {
                // iterate through result columns
                for(int j = 0; j < n; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < m; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        public static int GetDeterminant(int[,] matrix)
        {
            int n = matrix.GetLength(0);

            if(n == 0)
                throw new Exception("matrix must have values");
            if (n == 1)
                return matrix[0, 0];

            int det = 0;
            for (int j = 0; j < n; j++)
            {
                det += (int) Math.Pow(-1, 1 + j) * matrix[1, j] * GetDeterminant(GetSubmatrix(matrix, 1, j));
            }

            return det;
        }

        public static int[,] GetSubmatrix(int[,] parent, int row, int col)
        {
            int pn = parent.GetLength(0);
            if(pn == 0)
                throw new Exception("matrix must have values");
            if(pn == 1)
                return new int[0,0];

            int n = pn - 1;
            int[,] matrix = new int[n,n];
            int rOffset = 0, cOffset = 0;

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
    }
}
