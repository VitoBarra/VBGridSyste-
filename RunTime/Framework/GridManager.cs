using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VitoBarra.GeneralUtility.FeatureFullValue;

namespace VitoBarra.GridSystem.Framework
{
    [ExecuteInEditMode]
    public abstract class GridManager<TCell,TData> : MonoBehaviour
    {
        //Visual Debug
        [SerializeField] protected bool DrawPlaceHolder;
        [SerializeField] protected float TileSize;
        [SerializeField] protected TraceableValue<float> TileSizeTrac;
        [HideInInspector] public ViewDimension viewDimension = ViewDimension.D2;
        public Action OnGridChange;

        #region WorldPosition

        public abstract Vector3 GetWordPositionCenterCell(TCell cell);
        public abstract TCell GetNearestCell(Vector3 position);
        public abstract Vector3 GetNearestCellCenter(Vector3 position);
        #endregion


        #region Data

        public abstract bool MoveBetweenCells(IList<TCell> oldCells, IList<TCell> newCells);
        public abstract bool MoveBetweenCells(TCell oldCell, TCell newCell);
        public abstract void DeleteAtCell(TCell cell);
        public abstract void DeleteAtCell(IList<TCell>  cells);
        public abstract TData GetAtCell(TCell cell);
        public abstract IList<TData> GetAtCell(IList<TCell>  cells);

        public abstract void OccupiesCell(TData placeable, TCell cellToOccupy);
        public abstract void OccupiesCell(TData placeable, IList<TCell> cellsToOccupy);

        #endregion

        #region Conditioon

        public abstract bool IsValidCord(TCell cell);

        #endregion
    }
}