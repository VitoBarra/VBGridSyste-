using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GeneralUtility.FeatureFullValue;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    /// <summary>
    /// A class that define a grid of square cell and manage the logic for convert between world and grid position
    /// This class exist to coordinate the Logic Data and the WordPositioning logic and provide a single interface
    /// to interact with the grid with generic Datatype
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    internal class SquareGrid<TData>
    {
        private readonly SquareGridData<TData> LogicSquareGridData;
        private readonly SquareGridToWord SquareGridToWord;

        internal SquareGrid(SquareGridData<TData> logicSquareGridData, SquareGridToWord squareGridToWord)
        {
            LogicSquareGridData = logicSquareGridData;
            SquareGridToWord = squareGridToWord;
        }

        public SquareGrid(int row, int col, Vector2Hook gridWordPositionHook, float tileSize,
            Vector2 cellOffset,
            ViewDimension viewDimension)
        {
            LogicSquareGridData = new SquareGridData<TData>(row, col);
            SquareGridToWord = new SquareGridToWord(gridWordPositionHook, tileSize, cellOffset, viewDimension);
            SquareGridToWord.SetDimension(viewDimension).ReShape(row, col);
        }

        #region SetUp

        public SquareGrid<TData> ReShape(SquareCell newShape)
        {
            LogicSquareGridData.ReShape(newShape);
            SquareGridToWord.ReShape(newShape);
            return this;
        }
        public SquareGrid<TData> ReShape(int row, int col)
        {
            LogicSquareGridData.ReShape(row, col);
            SquareGridToWord.ReShape(row, col);
            return this;
        }


        public SquareGrid<TData> SetWorldInformation(float tileSize, Vector2 cellOffset)
        {
            SquareGridToWord.SetTileSize(tileSize).SetCellOffset(cellOffset);
            return this;
        }

        public SquareGrid<TData> SetPositionHook(Vector2Hook gridWordPositionHook)
        {
            SquareGridToWord.SetWordOffset(gridWordPositionHook);
            return this;
        }

        #endregion


        #region DataLogic

        public TData GetData(SquareCell cellCord)
        {
            return LogicSquareGridData.GetData(cellCord);
        }

        public bool IsPositionFree(SquareCell cellCord)
        {
            return LogicSquareGridData.IsPositionFree(cellCord);
        }


        public bool OccupiesCell(TData data, SquareCell cellCord, bool force = false)
        {
            try
            {
                LogicSquareGridData.OccupiesPosition(data, cellCord, force);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + $"In position: {cellCord.Row},{cellCord.Column}");
                return false;
            }

        }




        public bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> newCells)
        {
            return LogicSquareGridData.MoveLogicObject(oldCells, newCells);
        }

        public bool MoveBetweenCells(SquareCell oldCell, SquareCell newCell)
        {
           return  LogicSquareGridData.MoveLogicObject(oldCell, newCell);
        }


        public void Delete(IList<SquareCell> cells)
        {
            foreach (var cell in cells)
                LogicSquareGridData.ClearPosition(cell);
        }

        public void Delete(SquareCell cell)
        {
            LogicSquareGridData.ClearPosition(cell);
        }

        public void ClearGrid()
        {
            LogicSquareGridData.ClearAllPosition();
        }
        public bool IsValidCord(SquareCell cell)
        {
            return LogicSquareGridData.IsValidCord(cell) && SquareGridToWord.IsValidCord(cell);
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