using System.Collections.Generic;
using VitoBarra.GeneralUtility.DataStructure;
using VitoBarra.GridSystem.Framework;
using NotImplementedException = System.NotImplementedException;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCellMatrix<T> : ICellMemorization<T, SquareCell>
    {
        DynamicMatrix<T> Matrix;
        public int Row;
        public int Column;
        public List<SquareCell> OccupiedPosition = new List<SquareCell>();

        public SquareCellMatrix(int row, int column, T defaultValue = default)
        {
            Matrix = new DynamicMatrix<T>(row, column, defaultValue);
            Row = row;
            Column = column;
        }

        public T this[SquareCell cell]
        {
            get => Get(cell);
            set => Set(value, cell);
        }

        public void ClearAll()
        {
            foreach (var cell in OccupiedPosition)
                Clear(cell);
        }

        public void Clear(SquareCell cellCord)
        {
            Matrix.SetDefault(cellCord.Row, cellCord.Column);
        }

        public T this[int row, int col]
        {
            get => Get(new SquareCell(row, col));
            set => Set(value, new SquareCell(row, col));
        }


        public T Get(SquareCell cell) => Matrix[cell.Row, cell.Column];

        public void Set(T data, SquareCell cell)
        {
            Matrix[cell.Row, cell.Column] = data;
            OccupiedPosition.Add(cell);
        }

        public void Resize(SquareCell cell)
        {
            Row = cell.Row;
            Column = cell.Column;
            Matrix.Resize(cell.Row, cell.Column);
        }

        public bool IsValidCord(SquareCell cell) => Matrix.IsValidCord(cell.Row, cell.Column);
    }
}