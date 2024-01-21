namespace VitoBarra.GridSystem.POCO.CellType
{
    public interface ICellMemorization<T>
    {
        T? Get(int i, int j=-1,int k=-1, int h=-1);
        void Set(T data,int i, int j=-1,int k=-1, int h=-1);
        void Resize(int newMaxI, int newMaxJ=-1,int newMaxK=-1, int newMaxH=-1);

        bool IsValidCord(int i, int j=-1,int k=-1, int h=-1);
    }
}