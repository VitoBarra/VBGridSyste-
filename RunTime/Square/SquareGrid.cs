using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GeneralUtility.FeatureFullValue;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    public class SquareGrid<T>
    {
        private SquareGridMap<T> LogicSquareGridMap;
        private SquareGridToWord SquareGridToWord;

        public SquareGrid(SquareGridMap<T> logicSquareGridMap, SquareGridToWord squareGridToWord)
        {
            LogicSquareGridMap = logicSquareGridMap;
            SquareGridToWord = squareGridToWord;
        }

        public SquareGrid(int row, int col, Vector2Hook gridWordPositionHook, float tileSize,
            Vector2 cellOffset,
            ViewDimension viewDimension)
        {
            LogicSquareGridMap = new SquareGridMap<T>(row, col);
            SquareGridToWord = new SquareGridToWord(gridWordPositionHook, tileSize, cellOffset, viewDimension);
            SquareGridToWord.SetDimension(viewDimension).SetBounds(row, col);
        }

        #region SetUp

        public SquareGrid<T> Resize(int row, int col)
        {
            LogicSquareGridMap.Resize(row, col);
            SquareGridToWord.SetBounds(row, col);
            return this;
        }

        public SquareGrid<T> SetWorldInformation(float tileSize, Vector2 cellOffset)
        {
            SquareGridToWord.SetTileSize(tileSize).SetCellOffset(cellOffset);
            return this;
        }

        public SquareGrid<T> SetPositionHook(Vector2Hook gridWordPositionHook)
        {
            SquareGridToWord.SetWordOffset(gridWordPositionHook);
            return this;
        }

        #endregion


        #region DataLogic

        public T GetData(SquareCell cellCord)
        {
            return LogicSquareGridMap.GetData(cellCord);
        }

        public bool IsPositionFree(SquareCell cellCord)
        {
            return LogicSquareGridMap.IsPositionFree(cellCord);
        }


        public void OccupiesPosition(T data, SquareCell cellCord, bool force = false)
        {
            try
            {
                LogicSquareGridMap.OccupiesPosition(data, cellCord, force);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + $"In position: {cellCord.Row},{cellCord.Column}");
            }
        }

        private bool IsMovementPossible(SquareCell oldCell, SquareCell newCell, T data)
        {
            return !oldCell.Equals(newCell) && LogicSquareGridMap.IsValidCord(newCell) &&
                   (LogicSquareGridMap.IsTheSame(newCell, data) ||
                    LogicSquareGridMap.IsPositionFree(newCell));
        }


        private bool IsMovementPossible(IList<SquareCell> oldCells, IList<SquareCell> newPosition, T data)
        {
            var oldCellsData = new List<T>();
            foreach (var cell in oldCells)
            {
                oldCellsData.Add(LogicSquareGridMap.GetData(cell));
                LogicSquareGridMap.ClearPosition(cell);
            }

            var isMovementPossible = !oldCells.Where((t, i) => !IsMovementPossible(t, newPosition[i], data)).Any();

            for (int i = 0; i < oldCells.Count; i++)
                LogicSquareGridMap.OccupiesPosition(oldCellsData[i], oldCells[i]);

            return isMovementPossible;
        }


        public bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> newCells, T data)
        {
            if (!IsMovementPossible(oldCells, newCells, data)) return false;

            LogicSquareGridMap.MoveLogicObject(oldCells, newCells);

            return true;
        }

        public bool MoveBetweenCells(SquareCell oldCell, SquareCell newCell, T data)
        {
            if (!IsMovementPossible(oldCell, newCell, data)) return false;

            LogicSquareGridMap.MoveLogicObject(oldCell, newCell);
            return true;
        }


        public void Delete(IList<SquareCell> cells)
        {
            foreach (var cell in cells)
                LogicSquareGridMap.ClearPosition(cell);
        }

        public void Delete(SquareCell cell)
        {
            LogicSquareGridMap.ClearPosition(cell);
        }

        public void ClearGrid()
        {
            LogicSquareGridMap.ClearAllPosition();
        }

        #endregion

        #region WordLogic

        public SquareCell GetNearestCell(Vector3 position)
        {
            return SquareGridToWord.GetNearestCell(position);
        }

        public Vector3 GetWordPositionCenterCell(SquareCell cell)
        {
            return SquareGridToWord.GetWordPositionCenterCell(cell);
        }

        public Vector3 GetWordPositionGridEdge(int p0, int p1)
        {
            return SquareGridToWord.GetWordPositionGridEdge(new SquareCell(p0, p1));
        }

        public Vector3 GetNearestCellCenter(Vector3 position)
        {
            SquareCell generatedCell = SquareGridToWord.GetNearestCell(position);
            return SquareGridToWord.GetWordPositionCenterCell(generatedCell);
        }

        #endregion


    }
}