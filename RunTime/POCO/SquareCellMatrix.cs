using VitoBarra.GridSystem.POCO;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem
{
    public class SquareCellMatrix<T> : ICellMemorization<T, SquareCell>
    {
        DynamicMatrix<T> Matrix;

        public SquareCellMatrix(int width, int height, T defaultValue = default)
        {
            Matrix = new DynamicMatrix<T>(width, height, defaultValue);
        }

        public T Get(SquareCell cell)
        {
            return Matrix.Get(cell.I, cell.J);
        }

        public void Set(T data, SquareCell cell)
        {
            Matrix.Set(data, cell.I, cell.J);
        }

        public void Resize(SquareCell cell)
        {
            Matrix.Resize(cell.I, cell.J);
        }

        public bool IsValidCord(SquareCell cell)
        {
            return Matrix.IsValidCord(cell.I, cell.J);
        }
    }
}