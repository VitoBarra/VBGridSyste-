using VitoBarra.GridSystem.Framework;
using NotImplementedException = System.NotImplementedException;

namespace VitoBarra.GridSystem.Hexagonal
{
    public class HexagonalAbstractCell:AbstractCell
    {
        public readonly int I;
        public readonly int J;
        public readonly int K;

        public HexagonalAbstractCell(int i, int j, int k)
        {
            I = i;
            J = j;
            K = k;
        }

        public override bool Equals(AbstractCell other)
        {
            return other is HexagonalAbstractCell cell && I == cell.I && J == cell.J && K == cell.K;
        }
    }
}