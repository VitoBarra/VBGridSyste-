using System;
using UnityEngine;

namespace VitoBarra.Unity.GeneralSystem
{
    public class GridMap
    {
        protected bool[,] OccupiedMap;
        private int Width, Height;
        private Vector2Int Center;
        private Vector3 a;


        public GridMap(int _width, int _height, Vector2Int _center)
        {
            Width = _width;
            Height = _height;
            Center = _center;
            OccupiedMap = new bool[_width, _height];
            

            for (var i = Center[0]; i < _width; i++)
            for (var j = Center[1]; j < _height; j++)
                OccupiedMap[i, j] = true;
        }


        public void OccupiesPosition(float x, float z)
        {
            if (!(IsValidCord(x, z)))
                throw new ArgumentOutOfRangeException();
            OccupiedMap[(int)x, (int)z] = false;
        }


        public bool IsValidCord(float x, float z)
        {
            return (x >= Center[0] && x <= Width && z >= Center[1] -1 && z  <= Height -1);
        }

        public bool IsPostionFree(int x, int z)
        {
            return OccupiedMap[x,z];
        }
    }

    public class GridMap<T> : GridMap
    {
        private T[,] DataMap;

        public GridMap(int _width, int _height, Vector2Int _center) : base(_width, _height, _center)
        {
            DataMap = new T[_width, _height];
        }

        public void OccupiesPosition(float x, float z, T data)
        {
            OccupiesPosition(x, z);
            //Debug.Log($"y del cubo prima: {z +1}");
            //Debug.Log($"y del cubo: {(int)Math.Floor(z +1)}");
            DataMap[(int)x, (int)Math.Floor(z +1)] = data;
        }
        
        public T GetData(float x, float z)
        {
            //Debug.Log($"In GridMap passo: {(int)z}");
            return DataMap[(int)x, (int)Math.Floor(z +1)];
        }

        public (bool Pos,T Data) GetAllData(int x, int z)
        {
            return (OccupiedMap[x,z],DataMap[x,z]);
        }


    }
}