using System;

namespace MatrixFunctions
{
    public class Matrix
    {
        // properties
        public decimal[,] grid { get; set; }
        public int rows => grid?.GetLength(0) ?? 0;
        public int columns => grid?.GetLength(1) ?? 0;
        private int n => rows;
        private int m => columns;

        // constructors
        public Matrix(decimal[,] matrix)
        { grid = _Deepcopy(matrix); }

        public Matrix(Matrix a)
        { grid = _Deepcopy(a.grid); }

        // matrix grid deep copy function
        private static decimal[,] _Deepcopy(decimal[,] matrix)
        {
            // todo: how to deal with errors?
            if (matrix == null)
                throw new ArgumentException("cannot copy null matrix");

            int n = matrix.GetLength(0), m = matrix.GetLength(1);

            decimal[,] copy = new decimal[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    copy[i, j] = matrix[i, j];
            return copy;
        }

        // indexer
        public decimal this[int i, int j]
        {
            get => grid[i, j]; 
            set => grid[i, j] = value;
        }

        // operators
        public static Matrix operator +(Matrix a) => a;
        public static Matrix operator -(Matrix a)
        {
            Matrix b = new Matrix(a);
            for(int i = 0; i < b.rows; i++)
                for (int j = 0; j < b.columns; j++)
                    b[i, j] = -a[i, j];
            return b;
        }
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if(a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");
            for(int i = 0; i < a.rows; i++)
                for(int j = 0; j < a.columns; i++)
                    a[i,j] = a[i,j] + b[i,j];
            return a;
        }
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
                throw new ArgumentException("matrices must be the same size in rows and columns to add");
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.columns; j++)
                    a[i, j] -= b[i, j];
            return a;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.columns != b.rows)
                throw new ArgumentException("cannot multiply matrices with different sized columns/rows");

            int m = a.columns, n = b.rows;
            decimal[,] result = new decimal[m, n];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < m; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            return new Matrix(result);
        }
        public override string ToString() => "Matrix";
    }
}
