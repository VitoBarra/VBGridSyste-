using VitoBarra.GridSystem.Framework;
using NotImplementedException = System.NotImplementedException;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCell : AbstractCell
    {
        public int Row;
        public int Column;

        public SquareCell(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public SquareCell(SquareCell cell)
        {
            Row = cell.Row;
            Column = cell.Column;
        }

        public override bool Equals(AbstractCell other)
        {
            return other is SquareCell cell && Row == cell.Row && Column == cell.Column;
        }

        public static SquareCell operator +(SquareCell a, SquareCell b)
        {
            return new SquareCell(a.Row + b.Row, a.Column + b.Column);
        }
        public static SquareCell operator +(SquareCell a, UnityEngine.Vector2 b)
        {
            return new SquareCell(a.Row + (int)b.x, a.Column + (int)b.y);
        }

        public SquareCell Add(int i, int j)
        {
            return new SquareCell(Row + i, Column + j);
        }
    }
}