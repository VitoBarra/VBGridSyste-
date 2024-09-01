using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public sealed class SquareGridData<TData>
    {
        private SquareCellDataManager<TData?> DataMap;

        public SquareGridData(int row, int col, TData defaultValue = default)
        {
            DataMap = new SquareCellDataManager<TData>(row, col, defaultValue);
        }

        public void ClearPosition(SquareCell cellCord) => DataMap.ClearAtCell(cellCord);

        public void ClearAllPosition() => DataMap.ClearAll();

        public bool IsPositionFree(SquareCell cellCord)
        {
            try
            {
                return DataMap[cellCord] is null;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
        }

        public void OccupiesPosition(TData data, SquareCell cellCord, bool force = false)
        {
            if (!IsPositionFree(cellCord) && !force)
                throw new Exception("Position already occupied");
            DataMap[cellCord] =  data;
        }

        private bool IsMovementPossible(SquareCell oldCell, SquareCell newCell, TData data)
        {
            return !oldCell.Equals(newCell) && IsValidCord(newCell) &&
                   (IsTheSame(newCell, data) || IsPositionFree(newCell));

        }

        private bool IsMovementPossible(IList<SquareCell> oldCells, IList<SquareCell> newPosition, TData data)
        {
            var oldCellsData = new List<TData>();
            foreach (var cell in oldCells)
            {
                oldCellsData.Add(GetData(cell));
                ClearPosition(cell);
            }

            var isMovementPossible = !oldCells.Where((t, i) => !IsMovementPossible(t, newPosition[i], data)).Any();

            for (int i = 0; i < oldCells.Count; i++)
                OccupiesPosition(oldCellsData[i], oldCells[i]);

            return isMovementPossible;
        }


        public bool MoveLogicObject(SquareCell oldCell, SquareCell newCell, bool force = false)
        {
            var oldCellData = DataMap[oldCell];
            if (!IsMovementPossible(oldCell, newCell, oldCellData)) return false;

            ClearPosition(oldCell);
            OccupiesPosition(oldCellData, newCell, force);
            return true;
        }

        public bool MoveLogicObject(IList<SquareCell> oldCells, IList<SquareCell> newCells,
            bool force = false)
        {
            var oldCellsData = new List<TData?>();
            if (!IsMovementPossible(oldCells, newCells, DataMap[oldCells[0]])) return false;

            foreach (var oldCell in oldCells)
            {
                oldCellsData.Add(DataMap[oldCell]);
                ClearPosition(oldCell);
            }

            for (int i = 0; i < newCells.Count; i++)
            {
                OccupiesPosition(oldCellsData[i], newCells[i], force);
            }

            return true;
        }

        public TData GetData(SquareCell cell)
        {
            return DataMap[cell];
        }

        public void ReShape(int row, int col)
        {
            DataMap.ReShape(row, col);
        }
        public void ReShape(SquareCell cell)
        {
            DataMap.ReShape(cell);
        }


        public bool IsTheSame(SquareCell cell, TData data)
        {
            var data1 = DataMap[cell];
            return data1 != null && data1.Equals(data);
        }

        public bool IsValidCord(SquareCell newCell)
        {
            return DataMap.IsValidCord(newCell);
        }
    }
}