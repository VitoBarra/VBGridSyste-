using VitoBarra.GeneralUtility.DataStructure;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCellMatrix<T> : ICellMemorization<T, SquareCell>
    {
        DynamicMatrix<T> Matrix;

        public SquareCellMatrix(int row, int column, T defaultValue = default)
        {
            Matrix = new DynamicMatrix<T>(row, column, defaultValue);
        }

        public T this[SquareCell cell]
        {
            get => Get(cell);
            set => Set(value, cell);
        }
        public T this[int row, int col]
        {
            get => Get(new SquareCell(row, col));
            set => Set(value, new SquareCell(row, col));
        }


        public T Get(SquareCell cell) => Matrix[cell.Row, cell.Column];

        public void Set(T data, SquareCell cell) => Matrix[cell.Row, cell.Column] = data;

        public void Resize(SquareCell cell) => Matrix.Resize(cell.Row, cell.Column);

        public bool IsValidCord(SquareCell cell) => Matrix.IsValidCord(cell.Row, cell.Column);
    }
}