using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitoBarra.GridSystem.Poco;
using VitoBarra.GridSystem.POCO.CellType;


namespace VitoBarra.GridSystem
{
    [ExecuteInEditMode]
    public abstract class GridManager<T,K> : MonoBehaviour
    {
        //Visual Debug
        [SerializeField] protected bool DrawPlaceHolder;
        [SerializeField] protected float TileSize;
        [HideInInspector] public ViewType ViewType = ViewType.D2;
        public Action OnGridChange;

        public abstract Vector3 GetCenterCell(T cell);
        public abstract T GetNearestCell(Vector3 position);

        public abstract bool MoveBetweenCells(IList<T> oldCells, IList<T> NewCells, K placeable);

        public abstract void OccupiesPosition(K placeable, IList<T> cellToOccupy);
        public abstract Vector3 GetNearestCellCenter(Vector3 position);
    }
}