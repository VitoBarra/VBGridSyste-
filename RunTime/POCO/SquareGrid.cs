using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.Poco
{
    public class SquareGrid<T>
    {
        private GridMap<T> LogicGridMap;
        private GridToWord GridToWord;

        public SquareGrid(GridMap<T> logicGridMap, GridToWord gridToWord)
        {
            LogicGridMap = logicGridMap;
            GridToWord = gridToWord;
        }

        public SquareGrid(int width, int height, Vector2Hook gridWordPositionHook, float tileSize,
            Vector2 cellOffset,
            ViewType viewType)
        {
            LogicGridMap = new GridMap<T>(width, height);
            GridToWord = new GridToWord(gridWordPositionHook, tileSize, cellOffset, viewType);
            GridToWord.SetDimension(viewType).SetBounds(width, height);
        }

        #region SetUp

        public SquareGrid<T> Resize(int width, int height)
        {
            LogicGridMap.Resize(width, height);
            GridToWord.SetBounds(width, height);
            return this;
        }

        public SquareGrid<T> SetWorldInformation(float tileSize, Vector2 cellOffset)
        {
            GridToWord.SetTileSize(tileSize).SetCellOffset(cellOffset);
            return this;
        }

        public SquareGrid<T> SetPositionHook(Vector2Hook gridWordPositionHook)
        {
            GridToWord.SetWordOffset(gridWordPositionHook);
            return this;
        }

        #endregion


        #region DataLogic

        public bool IsPositionFree(SquareCell cellCord)
        {
            return LogicGridMap.IsPositionFree(cellCord);
        }


        public void OccupiesPosition(T data, SquareCell cellCord, bool force = false)
        {
            LogicGridMap.OccupiesPosition(data, cellCord, force);
        }

        private bool IsMovementPossible(SquareCell oldCell, SquareCell newCell, T data)
        {
            return !oldCell.Equals(newCell) && LogicGridMap.IsValidCord(newCell) &&
                   (LogicGridMap.IsTheSame(newCell, data) ||
                    LogicGridMap.IsPositionFree(newCell));
        }


        private bool IsMovementPossible(IList<SquareCell> oldCell, IList<SquareCell> newPosition, T data)
        {
            return !oldCell.Where((t, i) => !IsMovementPossible(t, newPosition[i], data)).Any();
        }


        public bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> newCells, T data)
        {
            if (!IsMovementPossible(oldCells, newCells, data)) return false;

            LogicGridMap.MoveLogicObject(oldCells, newCells);

            return true;
        }

        public bool MoveBetweenCells(SquareCell oldCell, SquareCell newCell, T data)
        {
            if (!IsMovementPossible(oldCell, newCell, data)) return false;

            LogicGridMap.MoveLogicObject(oldCell, newCell);
            return true;
        }

        #endregion

        #region WordLogic

        public SquareCell GetNearestCell(Vector3 position)
        {
            return GridToWord.GetNearestCell(position);
        }

        public Vector3 GetWordPositionCenterCell(SquareCell cell)
        {
            return GridToWord.GetWordPositionCenterCell(cell);
        }

        public Vector3 GetWordPositionGridEdge(int p0, int p1)
        {
            return GridToWord.GetWordPositionGridEdge(new SquareCell(p0, p1));
        }

        public Vector3 GetNearestCellCenter(Vector3 position)
        {
            SquareCell generatedCell = GridToWord.GetNearestCell(position);
            return GridToWord.GetWordPositionCenterCell(generatedCell);
        }

        #endregion
    }
}