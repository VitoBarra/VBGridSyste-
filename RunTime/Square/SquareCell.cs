using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCell : AbstractCell
    {
        public readonly int Row;
        public readonly int Column;

        public SquareCell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(AbstractCell other)
        {
            return other is SquareCell cell && Row == cell.Row && Column == cell.Column;
        }
    }
}