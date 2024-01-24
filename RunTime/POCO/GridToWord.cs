﻿using System;
using UnityEngine;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.Poco
{
    public class GridToWord
    {
        private Vector2Hook WordOffset;
        private Vector2 CellNumberOffset;
        private ViewType ViewType;
        private float TileSize;


        int Width, Height;

        public GridToWord(Vector2Hook wordOffset, float tileSize, Vector2 cellNumberOffset = default,
            ViewType viewType = ViewType.D2)
        {
            WordOffset = wordOffset;
            CellNumberOffset = cellNumberOffset;
            TileSize = tileSize;
            ViewType = viewType;
        }


        public GridToWord SetBounds(int width, int height)
        {
            if (width <= 0) width = 1;
            if (height <= 0) width = 1;
            Width = width;
            Height = height;
            return this;
        }


        public GridToWord SetTileSize(float tileSize)
        {
            TileSize = tileSize;
            return this;
        }

        public GridToWord SetCellOffset(Vector2 cellNumberOffset)
        {
            CellNumberOffset = cellNumberOffset;
            return this;
        }

        public GridToWord SetWordOffset(Vector2Hook cellNumberOffset)
        {
            WordOffset = cellNumberOffset;
            return this;
        }

        public GridToWord SetDimension(ViewType viewType)
        {
            ViewType = viewType;
            return this;
        }


        public Vector3 GetWordPositionGridEdge(SquareCell cell)
        {
            CalculateWordPosition(out var x, out var y, out var z, cell);
            return new Vector3(x, y, z);
        }

        private void CalculateWordPosition(out float x, out float y, out float z, SquareCell cell, float offset = 0f)
        {
            z = 0;
            y = 0;
            x = (cell.I + CellNumberOffset.x) * TileSize + WordOffset.i + offset;
            var refAxis = (cell.J + CellNumberOffset.y) * TileSize + WordOffset.j + offset;
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

        public Vector3 GetWordPositionCenterCell(SquareCell cell)
        {
            CalculateWordPosition(out var x, out var y, out var z, cell, TileSize / 2);
            return new Vector3(x, y, z);
        }

        public SquareCell GetNearestCell(Vector3 worldPosition)
        {
            var i = Math.Clamp(
                Mathf.FloorToInt((worldPosition.x - CellNumberOffset.x * TileSize - WordOffset.i) / TileSize), 0,
                Width - 1);
            var refAxis = ViewType == ViewType.D2 ? worldPosition.y : worldPosition.z;
            var j = Math.Clamp(Mathf.FloorToInt((refAxis - CellNumberOffset.y * TileSize - WordOffset.j) / TileSize), 0,
                Height - 1);
            return new SquareCell(i, j);
        }
    }
}