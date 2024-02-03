using System;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GridSystem.POCO;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.Poco
{
    public sealed class GridMap<T>
    {
        private ICellMemorization<(bool presence, T data), SquareCell> DataMap;


        public GridMap(int _width, int _height,T defaultValue=default)
        {
            DataMap = new SquareCellMatrix<(bool presence, T data)>(_width, _height, (false, defaultValue));
        }

        public void ClearPosition(SquareCell cellCord)
        {
            DataMap.Set((false, default), cellCord);
        }

        public bool IsPositionFree(SquareCell cellCord)
        {
            return !DataMap.Get(cellCord).presence;
        }

        public void OccupiesPosition(T data, SquareCell cellCord, bool force = false)
        {
            if (!IsPositionFree(cellCord) && !force) throw new Exception("Position already occupied");
            DataMap.Set((true, data), cellCord);
        }

        public void MoveLogicObject(SquareCell oldSquareCell, SquareCell newSquareCell, bool force = false)
        {
            var oldCellData = DataMap.Get(oldSquareCell).data;
            ClearPosition(oldSquareCell);
            OccupiesPosition(oldCellData, newSquareCell, force);
        }

        public void MoveLogicObject(IList<SquareCell> oldSquareCell, IList<SquareCell> newSquareCell,
            bool force = false)
        {
            var oldCellsData = new List<(bool, T data)>();
            for (var index = 0; index < oldSquareCell.Count; index++)
            {
                var oldCell = oldSquareCell[index];
                oldCellsData.Add(DataMap.Get(oldCell));
                ClearPosition(oldCell);
            }

            for (int i = 0; i < newSquareCell.Count; i++)
            {
                OccupiesPosition(oldCellsData[i].data, newSquareCell[i], force);
            }
        }

        public T GetData(SquareCell cell)
        {
            return DataMap.Get(cell).data;
        }

        public void Resize(int width, int height)
        {
            DataMap.Resize(new SquareCell(width, height));
        }

        public bool IsTheSame(SquareCell cell, T data)
        {
            var data1 = DataMap.Get(cell).data;
            return data1 != null && data1.Equals(data);
        }

        public bool IsValidCord(SquareCell newCell)
        {
            return DataMap.IsValidCord(newCell);
        }
    }
}