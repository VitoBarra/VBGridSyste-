using VitoBarra.GridSystem.Framework;

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
    }
}