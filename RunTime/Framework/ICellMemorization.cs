namespace VitoBarra.GridSystem.Framework
{
    public interface ICellMemorization<TData, TCell> where TCell : ICellType
    {
        TData Get(TCell cell);
        void Set(TData data, TCell cell);
        void Resize(TCell cell);

        bool IsValidCord(TCell cell);
    }
}