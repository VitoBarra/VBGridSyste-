using System;
using UnityEngine;

namespace VitoBarra.GridSystem.Poco
{
    public class GridToWord
    {
        private Vector2 Center;
        private float TileSize;


        int Width, Height;
        private ViewType ViewType = ViewType.D2;

        public GridToWord(Vector2 _center, float _tileSize)
        {
            Center = _center;
            TileSize = _tileSize;
        }

        public GridToWord(Vector2Int _center, float _tileSize, ViewType viewType) : this(
            _center, _tileSize)
        {
            ViewType = viewType;
        }

        public GridToWord SetBounds(int _width, int _height)
        {
            if (_width <= 0) _width = 1;
            if (_height <= 0) _width = 1;
            Width = _width;
            Height = _height;
            return this;
        }


        public GridToWord SetTileSize(float tileSize)
        {
            TileSize = tileSize;
            return this;
        }

        public GridToWord SetCenter(Vector2 center)
        {
            Center = center;
            return this;
        }


        public Vector3 GetWordPositionGridEdge(int i, int j)
        {
            CalculateWordPosition(out var x, out var y, out var z, i, j);
            return new Vector3(x, y, z);
        }

        private void CalculateWordPosition(out float x, out float y, out float z, int i, int j, float offset = 0)
        {
            z = 0;
            y = 0;
            x = (i + Center.x) * TileSize + offset;
            var refAxis = (j + Center.y) * TileSize + offset;
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

        public Vector3 GetWordPositionCenterCell(int i, int j)
        {
            CalculateWordPosition(out var x, out var y, out var z, i, j, TileSize / 2);
            return new Vector3(x, y, z);
        }

        public Vector2Int GetNearestCell(Vector3 worldPosition)
        {
            var i = Math.Clamp(Mathf.FloorToInt((worldPosition.x - Center.x * TileSize) / TileSize), 0, Width - 1);
            var refAxis = ViewType == ViewType.D2 ? worldPosition.y : worldPosition.z;
            var j = Math.Clamp(Mathf.FloorToInt((refAxis - Center.y * TileSize) / TileSize), 0, Height - 1);
            return new Vector2Int(i, j);
        }

        public GridToWord SetDimension(ViewType viewType)
        {
            ViewType = viewType;
            return this;
        }
    }
}