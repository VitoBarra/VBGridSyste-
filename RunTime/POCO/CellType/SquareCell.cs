namespace VitoBarra.GridSystem.POCO.CellType
{
    public struct SquareCell : ICellType
    {
        public readonly int I;
        public readonly int J;

        public SquareCell(int i, int j)
        {
            I = i;
            J = j;
        }
    }
}