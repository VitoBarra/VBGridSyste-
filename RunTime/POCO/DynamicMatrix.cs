using System;
using System.Collections.Generic;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.POCO
{
    public class DynamicMatrix<T>
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

        public T Get(int i, int j)
        {
            if (!(IsValidCord(i, j)))
                throw new ArgumentOutOfRangeException();
            return Matrix[i][j];
        }

        public void Set(T data, int i, int j)
        {
            if (!(IsValidCord(i, j)))
                throw new ArgumentOutOfRangeException();
            Matrix[i][j] = data;
        }


        public bool IsValidCord(int i, int j)
        {
            return i >= 0 && i < Width && j >= 0 && j < Height;
        }

        public void Resize(int newMaxWidth, int newMaxHeight)
        {
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