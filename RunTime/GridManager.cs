using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GridSystem.Poco;


namespace VitoBarra.GridSystem
{
    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        public GridMap<GameObject> GridMap;
        public GridToWord GridToWord;
        [SerializeField] public int Width, Height;
        [SerializeField] private float TileSize;
        [SerializeField] public Vector2 Center;
        [HideInInspector] public ViewType ViewType = ViewType.D2;

        public Action OnGridChange;

        //Visual Debug
        [SerializeField]
        private bool DrawPlaceHolder;
        private Vector3 ShapeSize;

        private void Awake()
        {
            SetUp();
        }

        public void SetUp()
        {
            GridMap = new GridMap<GameObject>(Width, Height);
            GridToWord = new GridToWord(Center, TileSize);
            GridToWord.SetDimension(ViewType).SetBounds(Width, Height);
        }

        public void ResetMap(int width, int height, Vector2Int center, float tileSize)
        {
            Width = width;
            Height = height;
            Center = center;
            TileSize = tileSize;
            SetUp();
        }


        public Vector2Int MoveBetweenCells(GridPlaceable ObjectOnGrid, Vector3 oldPosition, Vector3 newPosition)
        {
            var oldCell = GridToWord.GetNearestCell(oldPosition);
            var newCell = GridToWord.GetNearestCell(newPosition);
            if (!oldCell.Equals(newCell))
                GridMap.MoveLogicObject(oldCell.x, oldCell.y, newCell.x, newCell.y);
            ObjectOnGrid.transform.position = GridToWord.GetWordPositionCenterCell(newCell.x, newCell.y);
            return newCell;
        }

        public Vector3 GetNearestCenterCell(Vector3 position)
        {
            if (GridToWord == null) return position;
            Vector2Int generatedCell = GridToWord.GetNearestCell(position);
            return GridToWord.GetWordPositionCenterCell(generatedCell.x, generatedCell.y);
        }


        public void OccupiesPosition(GameObject gameObject)
        {
            var generatedCell = GridToWord.GetNearestCell(gameObject.transform.position);
            GridMap.OccupiesPosition(generatedCell.x, generatedCell.y, gameObject);
        }

        public UnityEngine.Object GetData(int i, int j)
        {
            return GridMap.GetData(i, j);
        }

        public Vector2Int GetCurrentCell(Vector3 Position)
        {
            return GridToWord.GetNearestCell(Position);
        }


        private void OnValidate()
        {
            if (Width <= 0) Width = 1;
            if (Height <= 0) Height = 1;
            // if (TileSize <= 0) TileSize = 0.1f;

            if (GridMap == null) SetUp();
            else
            {
                GridMap.Resize(Width, Height);
                GridToWord.SetBounds(Width, Height).SetTileSize(TileSize).SetCenter(Center);
                OnGridChange?.Invoke();
                ShapeSize = new Vector3(TileSize / 1.8f, TileSize / 1.8f, 0);
            }
        }



        public void OnDrawGizmos()
        {
            if (GridMap == null) return;


            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(i, j),
                        GridToWord.GetWordPositionGridEdge(i, j + 1));
                    Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(i, j),
                        GridToWord.GetWordPositionGridEdge(i + 1, j));
                }
            }

            Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(0, Height),
                GridToWord.GetWordPositionGridEdge(Width, Height));
            Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(Width, 0),
                GridToWord.GetWordPositionGridEdge(Width, Height));

            if(!DrawPlaceHolder) return;


            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    Gizmos.color = GridMap.IsPositionFree(i, j) ? Color.green : Color.red;
                    Gizmos.DrawCube(GridToWord.GetWordPositionCenterCell(i, j), ShapeSize);
                }
            }
        }


        public Vector3 GetCenterCell(Vector2Int currentCell)
        {
            return GridToWord.GetWordPositionCenterCell(currentCell.x, currentCell.y);
        }
    }
}