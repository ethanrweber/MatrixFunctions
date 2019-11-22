﻿using System;

namespace MatrixFunctions
{
    /// <summary>
    /// Library of matrix functions
    /// available functions include:
    ///     reduced row echelon form
    ///     matrix addition/subtraction
    ///     matrix multiplication
    ///     get determinant
    ///     get sub-matrix
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            decimal[,] m = {{1, -4, 2}, {-2, 8, -9}, {-1, 7, 0}};
            decimal[,] m2 = {{1, 2, -1, -4}, {2, 3, -1, -11}, {-2, 0, -3, 22}};
            Console.WriteLine("m:");
            PrintMatrix(m);
            Console.WriteLine("rref(m):");
            PrintMatrix(RREF(m));

            Console.WriteLine("m2:");
            PrintMatrix(m2);
            Console.WriteLine("rref(m2):");
            PrintMatrix(RREF(m2));

            Console.WriteLine("m + m:");
            PrintMatrix(AddMatrices(m, m));

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
                // swap rows i and r
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
                            matrix[j, k] -= mult * matrix[r, k];
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
        public static void PrintMatrix(decimal[,] matrix, int round=2)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write(Math.Round(matrix[i,j], round) + "\t");

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static decimal[,] AddMatrices(decimal[,] a, decimal[,] b) => _MatrixAdditionSubtraction(a, b, true);
        public static decimal[,] SubtractMatrices(decimal[,] a, decimal[,] b) => _MatrixAdditionSubtraction(a, b, false);

        /// <summary>
        /// private method for matrix addition/subtraction with matrices a,b
        /// addition or subtraction is determined by operation param (add = true, subtract = false)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private static decimal[,] _MatrixAdditionSubtraction(decimal[,] a, decimal[,] b, bool operation)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new Exception("cannot add matrices of different sizes");

            decimal[,] matrix = _Deepcopy(a);

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (operation)
                        matrix[i, j] += b[i, j];
                    else
                        matrix[i, j] -= b[i, j];

            return matrix;
        }

        public static int[,] MultiplyMatrices(int[,] a, int[,] b)
        {
            if(a.GetLength(1) != b.GetLength(0))
                throw new Exception("cannot multiply matrices of different sizes");

            int m = a.GetLength(0), n = b.GetLength(1);
            int[,] result = new int[a.GetLength(0), b.GetLength(1)];


            for(int i = 0; i < m; i++)
                for(int j = 0; j < n; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < m; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }

            return result;
        }

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
    }
}