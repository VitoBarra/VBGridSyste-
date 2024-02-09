using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Framework
{
    public abstract class GridSnappable<T> : MonoBehaviour where T : ICellType
    {
        public T PinCell { get; protected set; }
        public Action<T> OnCellSet;

        protected abstract void SnapToGrid();
        protected abstract void HoldOnGrid();
        public abstract IList<T> GetAllCell();

    }
}