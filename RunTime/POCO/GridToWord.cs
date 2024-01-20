using System;
using UnityEngine;

namespace VitoBarra.Unity.GeneralSystem
{
    public enum ViewType
    {
        D2,
        D3
    }


    public class GridToWord
    {
        private Vector2Int Center;
        private float TileSize;

        private ViewType ViewType = ViewType.D2;

        public GridToWord(Vector2Int _center, float _tileSize)
        {
            Center = _center;
            TileSize = _tileSize;
        }

        public GridToWord(Vector2Int _center, float _tileSize, ViewType viewType) : this(_center, _tileSize)
        {
            ViewType = viewType;
        }


        public Vector3 GetWordPositionGridEdge(float i, float j)
        {
            CalculateWordPosition(out var x, out var y, out var z, i, j);
            return new Vector3(x, y, z);
        }

        private void CalculateWordPosition(out float x, out float y, out float z, float i, float j, float offset = 0)
        {
            //e' controintuitiva, i e j sono le coordinate nella griglia o in cosa?
            z = 0;
            y = 0;
            x = (i - Center.x) * TileSize + offset;
            var refAxis = (j - Center.y) * TileSize + offset;
            switch (ViewType)
            {
                case ViewType.D2:
                    y = refAxis;
                    break;
                case ViewType.D3:
                    z = -refAxis;
                    break;
            }
        }

        public Vector3 GetWordPositionCenterCell(float i, float j)
        {
            CalculateWordPosition(out var x, out var y, out var z, i, j, TileSize / 2);
            return new Vector3(x, y, z);
        }

        public Vector2Int GetCell(Vector3 worldPosition)
        {
            var i = Mathf.FloorToInt((Center.x + worldPosition.x) / TileSize);
            var refAxis = ViewType == ViewType.D2 ? worldPosition.y : worldPosition.z;
            var j = Mathf.FloorToInt((Center.y + refAxis) / TileSize);
            
            return new Vector2Int(i, j);
        }

        public void SetDimension(ViewType viewType)
        {
            ViewType = viewType;
        }
    }
}