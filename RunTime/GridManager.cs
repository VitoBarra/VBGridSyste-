using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VitoBarra.Unity.GeneralSystem
{
    public class GridManager : MonoBehaviour
    {
        public GridMap<GameObject> GridMap;
        public GridToWord GridToWord;
        [SerializeField] public int Width, Height;
        [SerializeField] public bool Inizialized = false;
        [SerializeField] private float TileSize;
        [SerializeField] public Vector2Int Center;
        [HideInInspector]
        public ViewType ViewType = ViewType.D2;

        public void SetUpDrawer()
        {
            
            GridMap = new GridMap<GameObject>(Width, Height, Center);
            GridToWord = new GridToWord(Center, TileSize);
            GridToWord.SetDimension(ViewType);
        }

        public void ResetMap(int width, int height,Vector2Int center,float tileSize)
        {
            Width = width;
            Height = height;
            Center = center;
            TileSize = tileSize;
            SetUpDrawer();
        }

        public Vector3 GetWordPosition(int x, int z)
        {
            return GridToWord.GetWordPositionCenterCell(x, z);
        }

        public Vector3 AdjustPosition(Vector3 Position)
        {
            Vector2Int generatedCell = GridToWord.GetCell(Position);
            //Debug.Log(GridToWord.GetWordPositionGridEdge(0,0));
            //Debug.Log(Position.ToString());
            //Debug.Log(generatedCell.ToString());
            return GridToWord.GetWordPositionCenterCell(generatedCell.x, generatedCell.y);
        }

        /*public void OccupiesPosition(int x, int z)
        {
            GridMap.OccupiesPosition(x, z);
        }*/

        public void OccupiesPosition(float x, float z, GameObject Object )
        {
            GridMap.OccupiesPosition(x, z,Object);
        }

        public UnityEngine.Object GetData(float x, float z)
        {
            //return GridMap.GetData(x, z);
            return GridMap.GetData(x , z);
        }

        public bool IsValidCord(float x, float z)
        {
            return GridMap.IsValidCord(x, z);
        }


        public void OnDrawGizmos()
        {

            if (GridMap == null) return;

            Inizialized = true;

            for (var i = 0; i < Width; i++)
            {
                for (var j = -1; j < Height - 1; j++)
                {
                    Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(i, j),
                        GridToWord.GetWordPositionGridEdge(i, j + 1));
                    Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(i, j),
                        GridToWord.GetWordPositionGridEdge(i + 1, j));
                }
            }

            Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(0, Height - 1),
                GridToWord.GetWordPositionGridEdge(Width, Height - 1));
            Gizmos.DrawLine(GridToWord.GetWordPositionGridEdge(Width, -1),
                GridToWord.GetWordPositionGridEdge(Width, Height - 1));
        }
    }
}