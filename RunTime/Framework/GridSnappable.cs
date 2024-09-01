using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Framework
{
    public abstract class GridSnappable<TCell> : MonoBehaviour where TCell : AbstractCell
    {
        public  TCell Cell { get; protected set; }
        public Action<TCell> OnCellSet;


        protected abstract void SnapToGrid();
        protected abstract void HoldOnGrid();


        protected bool UpDateCellAndPosition<TData>(GridManager<TCell,TData> gridManager)
        {
            if (gridManager is null) return false;
            var newPostion = gridManager.GetNearestCellCenter(transform.position, out var cell);
            if (cell is null) return false;
            transform.position = newPostion;
            Cell = cell;
            return true;
        }

    }
}