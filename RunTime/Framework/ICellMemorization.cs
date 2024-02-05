namespace VitoBarra.GridSystem.Framework
{
    public interface ICellMemorization<T,C> where C:ICellType
    {
        T Get(C cell);
        void Set(T data,C cell);
        void Resize(C cell);

        bool IsValidCord(C cell);
    }
}