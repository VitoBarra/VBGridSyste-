using System;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public sealed class SquareGridMap<T>
    {
        private ICellMemorization<(bool presence, T data), SquareCell> DataMap;

        public SquareGridMap(int row, int col, T defaultValue = default)
        {
            DataMap = new SquareCellMatrix<(bool presence, T data)>(row, col, (false, defaultValue));
        }

        public void ClearPosition(SquareCell cellCord)
        {
            DataMap[cellCord] = (false, default);
        }

        public bool IsPositionFree(SquareCell cellCord)
        {
            return !DataMap[cellCord].presence;
        }

        public void OccupiesPosition(T data, SquareCell cellCord, bool force = false)
        {
            if (!IsPositionFree(cellCord) && !force)
                throw new Exception("Position already occupied");
            DataMap[cellCord] = (true, data);
        }

        public void MoveLogicObject(SquareCell oldSquareCell, SquareCell newSquareCell, bool force = false)
        {
            var oldCellData = DataMap[oldSquareCell].data;
            ClearPosition(oldSquareCell);
            OccupiesPosition(oldCellData, newSquareCell, force);
        }

        public void MoveLogicObject(IList<SquareCell> oldSquareCell, IList<SquareCell> newSquareCell,
            bool force = false)
        {
            var oldCellsData = new List<(bool, T data)>();
            foreach (var oldCell in oldSquareCell)
            {
                oldCellsData.Add(DataMap[oldCell]);
                ClearPosition(oldCell);
            }

            for (int i = 0; i < newSquareCell.Count; i++)
            {
                OccupiesPosition(oldCellsData[i].data, newSquareCell[i], force);
            }
        }

        public T GetData(SquareCell cell)
        {
            return DataMap[cell].data;
        }

        public void Resize(int row, int col)
        {
            DataMap.Resize(new SquareCell(row, col));
        }

        public bool IsTheSame(SquareCell cell, T data)
        {
            var data1 = DataMap[cell].data;
            return data1 != null && data1.Equals(data);
        }

        public bool IsValidCord(SquareCell newCell)
        {
            return DataMap.IsValidCord(newCell);
        }
    }
}