using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Hexagonal
{
    public struct HexagonalCell:ICellType
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