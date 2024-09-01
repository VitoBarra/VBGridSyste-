using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VitoBarra.GeneralUtility.FeatureFullValue;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    [ExecuteInEditMode]
    public class SquaredGridManager : GridManager<SquareCell, GameObject>
    {
        private TraceableInt RowTrac, ColTrac ;

        [SerializeField] public int Row;
        [SerializeField] public int Column;


        private TraceableValue<Vector2> CellOffsetTrac;
        [SerializeField] public Vector2 CellOffset;

        private Vector2Hook GridWordPositionHook;

        private SquareGrid<GameObject> SquareGrid;
        public Vector3 ShapeSize;

        public Action<int, int> OnResize;


        private void Awake()
        {
            SetUp();
        }

        private void Update()
        {
            UpdateGridPosition();
        }

        public void SetUp()
        {
            var gridWordPosition = transform.position;
            GridWordPositionHook ??= new Vector2Hook(gridWordPosition.x, gridWordPosition.y);
            SquareGrid ??= new SquareGrid<GameObject>(Row, Column, GridWordPositionHook, TileSize, CellOffset, ViewDimension.D2);

            RowTrac = new TraceableInt(Row);
            ColTrac = new TraceableInt(Column);
            TileSizeTrac = new TraceableValue<float>(TileSize);
            CellOffsetTrac = new TraceableValue<Vector2>(CellOffset);
            OnResize = null;
        }


        #region Data

        public override bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> newCells)
        {
            return SquareGrid.MoveBetweenCells(oldCells, newCells);
        }

        public override bool MoveBetweenCells(SquareCell oldCell, SquareCell newCell)
        {
            return SquareGrid.MoveBetweenCells(oldCell, newCell);
        }

        public override void DeleteAtCell(SquareCell cell)
        {
            SquareGrid.Delete(cell);
        }

        public override void DeleteAtCell(IList<SquareCell> cells)
        {
            if (cells is null) return;
            SquareGrid.Delete(cells);
        }

        public override GameObject GetAtCell(SquareCell cell)
        {
            return SquareGrid.GetData(cell);
        }

        public override IList<GameObject> GetAtCell(IList<SquareCell> cells)
        {
            var data = new List<GameObject>();
            foreach (var cell in cells)
                data.Add(GetAtCell(cell));

            return data;
        }

        public GameObject GetAtCell(int row, int col)
        {
            return GetAtCell(new SquareCell(row, col));
        }

        public override void OccupiesCell(GameObject placeable, IList<SquareCell> cellsToOccupy)
        {
            if (placeable is null) return;
            foreach (var cell in cellsToOccupy)
                OccupiesCell(placeable, cell);
        }

        public override void OccupiesCell(GameObject placeable, SquareCell cellToOccupy)
        {
            SquareGrid.OccupiesCell(placeable.gameObject, cellToOccupy);
        }

        public void TryOccupiesCell(GameObject placeable, SquareCell cellToOccupy)
        {
            try
            {
                OccupiesCell(placeable.gameObject, cellToOccupy);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + $"In position: {cellToOccupy.Row},{cellToOccupy.Column}");
            }
        }

        public void ClearGrid()
        {
            SquareGrid.ClearGrid();
        }

        public override bool IsValidCord(SquareCell cell)
        {
            return SquareGrid.IsValidCord(cell);
        }

        #endregion


        #region Placement

        public override SquareCell GetNearestCell(Vector3 position)
        {
            return SquareGrid.GetNearestCell(position);
        }

        public override Vector3 GetNearestCellCenter(Vector3 position , out SquareCell cell)
        {
            if (SquareGrid != null)
                return SquareGrid.GetNearestCellCenter(position, out cell);

            cell = null;
            return position;

        }


        public override Vector3 GetWordPositionCenterCell(SquareCell cell)
        {
            return SquareGrid.GetWordPositionCenterCell(cell);
        }


        public void UpdateGridPosition()
        {
            var gridWordPosition = transform.position;
            GridWordPositionHook.Set(gridWordPosition.x, gridWordPosition.y);
        }

        #endregion


        private void OnValidate()
        {
            if (Column <= 0) Column = 1;
            if (Row <= 0) Row = 1;
            if (SquareGrid == null) SetUp();

            ColTrac.Value = Column;
            RowTrac.Value = Row;
            TileSizeTrac.Value = TileSize;
            CellOffsetTrac.Value = CellOffset;

            if (TileSizeTrac.IsValueChanged || CellOffsetTrac.IsValueChanged)
            {
                SquareGrid?.SetWorldInformation(TileSize, CellOffset);
                OnGridChange?.Invoke();
            }

            if (ColTrac.IsValueChanged || RowTrac.IsValueChanged)
            {
                SquareGrid?.ReShape(Row, Column);
                OnResize?.Invoke(Row, Column);
            }

            ShapeSize = new Vector3(TileSize / 1.8f, TileSize / 1.8f, 0);
        }

        public void OnDrawGizmos()
        {
            if (SquareGrid == null) return;


            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column ; j++)
                {
                    Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(i, j),
                        SquareGrid.GetWordPositionGridEdge(i, j + 1));
                    Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(i, j),
                        SquareGrid.GetWordPositionGridEdge(i + 1, j));
                }
            }

            Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(0, Column),
                SquareGrid.GetWordPositionGridEdge(Row, Column));
            Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(Row, 0),
                SquareGrid.GetWordPositionGridEdge(Row, Column));

            if (!DrawPlaceHolder) return;


            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    var cell = new SquareCell(i, j);
                    Gizmos.color = SquareGrid.IsPositionFree(cell) ? Color.green : Color.red;
                    Gizmos.DrawCube(SquareGrid.GetWordPositionCenterCell(cell), ShapeSize);
                }
            }
        }


    }
}