using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public class SquareCell : AbstractCell
    {
        public readonly int row;
        public readonly int col;

        public SquareCell(int i, int j)
        {
            row = i;
            col = j;
        }

        public override bool Equals(AbstractCell other)
        {
            return other is SquareCell cell && row == cell.row && col == cell.col;
        }
    }
}