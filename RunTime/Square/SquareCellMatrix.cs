using VitoBarra.GeneralUtility.DataStructure;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCellMatrix<T> : ICellMemorization<T, SquareCell>
    {
        DynamicMatrix<T> Matrix;

        public SquareCellMatrix(int width, int height, T defaultValue = default)
        {
            Matrix = new DynamicMatrix<T>(width, height, defaultValue);
        }

        public T this[SquareCell cell]
        {
            get => Get(cell);
            set => Set(value, cell);
        }
        public T this[int i, int j]
        {
            get => Get(new SquareCell(i, j));
            set => Set(value, new SquareCell(i, j));
        }


        public T Get(SquareCell cell) => Matrix[cell.row, cell.col];

        public void Set(T data, SquareCell cell) => Matrix[cell.row, cell.col] = data;

        public void Resize(SquareCell cell) => Matrix.Resize(cell.row, cell.col);

        public bool IsValidCord(SquareCell cell) => Matrix.IsValidCord(cell.row, cell.col);
    }
}