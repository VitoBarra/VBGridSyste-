namespace VitoBarra.GridSystem.Framework
{
    public interface ICellMemorization<TData, TCell> where TCell : AbstractCell
    {
        TData Get(TCell cell);
        void Set(TData data, TCell cell);
        void Resize(TCell cell);

        bool IsValidCord(TCell cell);

        TData this[TCell cell] { get; set; }

    }
}