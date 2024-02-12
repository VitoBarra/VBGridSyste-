using System;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GeneralUtility.FeatureFullValue;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    [ExecuteInEditMode]
    public class SquaredGridManager : GridManager<SquareCell, GameObject>
    {
        private TraceableInt WidthTrac, HeightTrac;
        [SerializeField] public int Width, Height;


        private TraceableValue<Vector2> CellOffsetTrac;
        [SerializeField] public Vector2 CellOffset;

        private Vector2Hook GridWordPositionHook;

        private SquareGrid<GameObject> SquareGrid;
        public Vector3 ShapeSize;

        public Action<int, int> OnResize;


        private void Start()
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
            SquareGrid ??= new SquareGrid<GameObject>(Width, Height, GridWordPositionHook, TileSize, CellOffset, ViewDimension.D2);

            HeightTrac = new TraceableInt(Height);
            WidthTrac = new TraceableInt(Width);
            TileSizeTrac = new TraceableValue<float>(TileSize);
            CellOffsetTrac = new TraceableValue<Vector2>(CellOffset);
            OnResize = null;
        }


        #region Data

        public override bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> NewCells,
            GameObject data)
        {
            return SquareGrid.MoveBetweenCells(oldCells, NewCells, data);
        }

        public override bool MoveBetweenCells(SquareCell oldCell, SquareCell newCell, GameObject data)
        {
            return SquareGrid.MoveBetweenCells(oldCell, newCell, data);
        }

        public override void DeleteAtCell(SquareCell cell)
        {
            SquareGrid.Delete(cell);
        }

        public override void DeleteAtCell(IList<SquareCell> cells)
        {
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

        public GameObject GetAtCell(int i, int j)
        {
            return GetAtCell(new SquareCell(i, j));
        }

        public override void OccupiesCell(GameObject placeable, IList<SquareCell> cellsToOccupy)
        {
            foreach (var cell in cellsToOccupy)
                OccupiesCell(placeable, cell);
        }

        public override void OccupiesCell(GameObject placeable, SquareCell cellToOccupy)
        {
            SquareGrid.OccupiesPosition(placeable.gameObject, cellToOccupy);
        }

        #endregion


        #region Placement

        public override SquareCell GetNearestCell(Vector3 position)
        {
            return SquareGrid.GetNearestCell(position);
        }

        public override Vector3 GetNearestCellCenter(Vector3 position)
        {
            return SquareGrid?.GetNearestCellCenter(position) ?? position;
        }


        public override Vector3 GetCenterCell(SquareCell cell)
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
            if (Width <= 0) Width = 1;
            if (Height <= 0) Height = 1;
            if (SquareGrid == null) SetUp();

            WidthTrac.Value = Width;
            HeightTrac.Value = Height;
            TileSizeTrac.Value = TileSize;
            CellOffsetTrac.Value = CellOffset;

            if (TileSizeTrac.IsValueChanged || CellOffsetTrac.IsValueChanged)
            {
                SquareGrid?.SetWorldInformation(TileSize, CellOffset);
                OnGridChange?.Invoke();
            }

            if (WidthTrac.IsValueChanged || HeightTrac.IsValueChanged)
            {
                SquareGrid?.Resize(Width, Height);
                OnResize?.Invoke(Width, Height);
            }

            ShapeSize = new Vector3(TileSize / 1.8f, TileSize / 1.8f, 0);
        }

        public void OnDrawGizmos()
        {
            if (SquareGrid == null) return;


            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(i, j),
                        SquareGrid.GetWordPositionGridEdge(i, j + 1));
                    Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(i, j),
                        SquareGrid.GetWordPositionGridEdge(i + 1, j));
                }
            }

            Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(0, Height),
                SquareGrid.GetWordPositionGridEdge(Width, Height));
            Gizmos.DrawLine(SquareGrid.GetWordPositionGridEdge(Width, 0),
                SquareGrid.GetWordPositionGridEdge(Width, Height));

            if (!DrawPlaceHolder) return;


            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    var cell = new SquareCell(i, j);
                    Gizmos.color = SquareGrid.IsPositionFree(cell) ? Color.green : Color.red;
                    Gizmos.DrawCube(SquareGrid.GetWordPositionCenterCell(cell), ShapeSize);
                }
            }
        }
    }
}