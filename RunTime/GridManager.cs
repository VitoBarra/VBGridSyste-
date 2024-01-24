using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GridSystem.Poco;
using VitoBarra.GridSystem.POCO.CellType;


namespace VitoBarra.GridSystem
{
    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        [SerializeField] public int Width, Height;
        [SerializeField] private float TileSize;

        [SerializeField] public Vector2 CellOffset;

        [HideInInspector] public ViewType ViewType = ViewType.D2;

        public Action OnGridChange;

        //Visual Debug
        [SerializeField] private bool DrawPlaceHolder;

        private Vector2Hook GridWordPositionHook;


        private SquareGrid<GameObject> SquareGrid;
        private Vector3 ShapeSize;

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
            UpdateGridPosition();
            SquareGrid ??=
                new SquareGrid<GameObject>(Width, Height, GridWordPositionHook, TileSize, CellOffset, ViewType);
        }


        public SquareCell GetNearestCell(Vector3 position)
        {
            return SquareGrid.GetNearestCell(position);
        }

        public bool MoveBetweenCells(IList<SquareCell> oldCells, IList<SquareCell> NewCells,
            GameObject placeable)
        {
            return SquareGrid.MoveBetweenCells(oldCells, NewCells, placeable);
        }

        public Vector3 GetNearestCellCenter(Vector3 position)
        {
            return SquareGrid?.GetNearestCellCenter(position) ?? position;
        }


        public void OccupiesPosition(GameObject placeable, IList<SquareCell> cellToOccupy)
        {
            foreach (var cell in cellToOccupy)
                SquareGrid.OccupiesPosition(placeable.gameObject, cell);
        }

        public Vector3 GetCenterCell(SquareCell cell)
        {
            return SquareGrid.GetWordPositionCenterCell(cell);
        }

        public void UpdateGridPosition()
        {
            var gridWordPosition = transform.position;
            GridWordPositionHook ??= new Vector2Hook(gridWordPosition.x, gridWordPosition.y);
            GridWordPositionHook.Set(gridWordPosition.x, gridWordPosition.y);
        }


        private void OnValidate()
        {
            if (Width <= 0) Width = 1;
            if (Height <= 0) Height = 1;

            if (SquareGrid == null) SetUp();
            else
            {
                SquareGrid.Resize(Width, Height).SetWorldInformation(TileSize, CellOffset);
                OnGridChange?.Invoke();
                ShapeSize = new Vector3(TileSize / 1.8f, TileSize / 1.8f, 0);
            }
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