using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem
{
    public abstract class GridSnappable<T> : MonoBehaviour where T : ICellType
    {
        public T PinCell { get; protected set; }
        [FormerlySerializedAs("OnCellChange")] [FormerlySerializedAs("OnMove")] public Action<T> OnCellSet;

        protected abstract void SnapToGrid();
        protected abstract void HoldOnGrid();
        protected abstract IList<T> GetPositionToOccupy(T generatedCell);
    }
}