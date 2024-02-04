using System;
using System.Collections.Generic;
using UnityEngine;
using VitoBarra.GeneralUtility.FeatureFullValue;

namespace VitoBarra.GridSystem
{
    [ExecuteInEditMode]
    public abstract class GridManager<T,K> : MonoBehaviour
    {
        //Visual Debug
        [SerializeField] protected bool DrawPlaceHolder;
        [SerializeField] protected float TileSize;
        [SerializeField] protected TraceableValue<float> TileSizeTrac;
        [HideInInspector] public ViewType ViewType = ViewType.D2;
        public Action OnGridChange;

        public abstract Vector3 GetCenterCell(T cell);
        public abstract T GetNearestCell(Vector3 position);

        public abstract bool MoveBetweenCells(IList<T> oldCells, IList<T> NewCells, K placeable);

        public abstract void OccupiesPosition(K placeable, IList<T> cellToOccupy);
        public abstract Vector3 GetNearestCellCenter(Vector3 position);
    }
}