using System;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GridSystem.Poco;
using VitoBarra.GridSystem.POCO.CellType;
using VitoBarra.GeneralUtility.FeatureFullValue;

namespace VitoBarra.GridSystem
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
        private Vector3 ShapeSize;

        public Action<int, int> OnResize;


        private void Awake()
        {
            // SquareGrid =
            //     new SquareGrid<GameObject>(Width, Height, GridWordPositionHook, TileSize, CellOffset, ViewType);
        }

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
            SquareGrid ??= new SquareGrid<GameObject>(Width, Height, GridWordPositionHook, TileSize, CellOffset,
                ViewType);

            HeightTrac = new TraceableInt(Height);
            WidthTrac = new TraceableInt(Width);
            TileSizeTrac = new TraceableValue<float>(TileSize);
            CellOffsetTrac = new TraceableValue<Vector2>(CellOffset);
            OnResize = null;
        }


        public override SquareCell GetNearestCell(Vector3 position)
        {
            return SquareGrid.GetNearestCell(position);
        }

        public override bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> NewCells,
            GameObject placeable)
        {
            return SquareGrid.MoveBetweenCells(oldCells, NewCells, placeable);
        }

        public override Vector3 GetNearestCellCenter(Vector3 position)
        {
            return SquareGrid?.GetNearestCellCenter(position) ?? position;
        }


        public override void OccupiesPosition(GameObject placeable, IList<SquareCell> cellToOccupy)
        {
            foreach (var cell in cellToOccupy)
                SquareGrid.OccupiesPosition(placeable.gameObject, cell);
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

        public GameObject GetObjectAtCell(SquareCell cell)
        {
            return SquareGrid.GetData(cell);
        }

        public GameObject GetObjectAtCell(int i, int j)
        {
            return SquareGrid.GetData(new SquareCell(i, j));
        }


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