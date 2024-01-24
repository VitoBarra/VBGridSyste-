using System;
using System.Collections.Generic;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.POCO
{
    public class DynamicMatrix<T> : ICellMemorization<T, SquareCell>
    {
        IList<IList<T>> Matrix;

        int Width, Height;

        T DefaultValue;

        public DynamicMatrix(int width, int height, T defaultValue = default)
        {
            Width = width;
            Height = height;
            DefaultValue = defaultValue;
            Matrix = new List<IList<T>>();
            for (int i = 0; i < width; i++)
            {
                var column = new List<T>();
                for (int j = 0; j < height; j++)
                    column.Add(defaultValue);
                Matrix.Add(column);
            }
        }

        public T Get(SquareCell cell)
        {
            if (!(IsValidCord(cell)))
                throw new ArgumentOutOfRangeException();
            return Matrix[cell.I][cell.J];
        }

        public void Set(T data, SquareCell cell)
        {
            if (!(IsValidCord(cell)))
                throw new ArgumentOutOfRangeException();
            Matrix[cell.I][cell.J] = data;
        }


        public bool IsValidCord(SquareCell cell)
        {
            return cell.I >= 0 && cell.I < Width && cell.J >= 0 && cell.J < Height;
        }

        public void Resize(SquareCell cell)
        {
            var newMaxWidth = cell.I;
            var newMaxHeight = cell.J;

            var widthDiff = newMaxWidth - Width;
            var heightDiff = newMaxHeight - Height;

            Width = newMaxWidth;

            switch (widthDiff)
            {
                case > 0:
                {
                    for (int i = 0; i < widthDiff; i++)
                    {
                        var column = new List<T>();
                        for (int j = 0; j < Height; j++)
                            column.Add(DefaultValue);
                        Matrix.Add(column);
                    }

                    break;
                }
                case < 0:
                {
                    for (int i = 0; i < -widthDiff; i++)
                        Matrix.RemoveAt(Matrix.Count - 1);
                    break;
                }
            }

            Height = newMaxHeight;

            switch (heightDiff)
            {
                case > 0:
                {
                    foreach (var t in Matrix)
                        for (int j = 0; j < heightDiff; j++)
                            t.Add(DefaultValue);

                    break;
                }
                case < 0:
                {
                    foreach (var t in Matrix)
                        for (int j = 0; j < -heightDiff; j++)
                            t.RemoveAt(t.Count - 1);

                    break;
                }
            }
        }
    }
}