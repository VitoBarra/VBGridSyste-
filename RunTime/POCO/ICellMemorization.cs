namespace VitoBarra.GridSystem.POCO.CellType
{
    public interface ICellMemorization<T,C> where C:BaseCellType
    {
        T Get(C cell);
        void Set(T data,C cell);
        void Resize(C cell);

        bool IsValidCord(C cell);
    }
}