using System;
using UnityEngine;
using VitoBarra.GridSystem.POCO;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.Poco
{
    public class GridMap
    {
        protected ICellMemorization<bool> OccupiedMap;
        protected int Width, Height;


        public GridMap(int _width, int _height)
        {
            Width = _width;
            Height = _height;
            OccupiedMap = new DynamicMatrix<bool>(_width, _height, true);
        }


        public void OccupiesPosition(int i, int j)
        {
            OccupiedMap.Set(false, i, j);
        }

        public virtual void ClearPosition(int i, int j)
        {
            OccupiedMap.Set(true, i, j);
        }


        public bool IsPositionFree(int i, int z)
        {
            return OccupiedMap.Get(i, z);
        }
    }

    public class GridMap<T> : GridMap
    {
        private ICellMemorization<T> DataMap;

        public GridMap(int _width, int _height) : base(_width, _height)
        {
            DataMap = new DynamicMatrix<T>(_width, _height);
        }

        public void OccupiesPosition(int i, int j, T data, bool force = false)
        {
            if (DataMap.Get(i, j) != null && !force) throw new Exception("Position already occupied");
            OccupiesPosition(i, j);
            DataMap.Set(data, i, j);
        }

        public override void ClearPosition(int i, int j)
        {
            base.ClearPosition(i, j);
            DataMap.Set(default, i, j);
        }


        public void MoveLogicObject(int i1, int j1, int i2, int j2)
        {
            OccupiesPosition(i2, j2, DataMap.Get(i1, j1));
            ClearPosition(i1, j1);
        }

        public T GetData(int i, int j)
        {
            return DataMap.Get(i, j);
        }

        public (bool Pos, T Data) GetAllData(int i, int j)
        {
            return (OccupiedMap.Get(i, j), DataMap.Get(i, j));
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            OccupiedMap.Resize(width, height);
            DataMap.Resize(width, height);
        }
    }
}