namespace VitoBarra.GridSystem.POCO.CellType
{
    public struct HexagonalCell:BaseCellType
    {
        public readonly int I;
        public readonly int J;
        public readonly int K;

        public HexagonalCell(int i, int j, int k)
        {
            I = i;
            J = j;
            K = k;
        }
    }
}