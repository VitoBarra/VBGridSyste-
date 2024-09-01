using System.Collections.Generic;
using VitoBarra.GridSystem.Square;

namespace VitoBarra.GridSystem.Framework
{
    /// <summary>
    /// A class that define a common interface for data on cell an implement common access logic
    /// </summary>
    /// <typeparam name="TData">the Data type </typeparam>
    /// <typeparam name="TCell">the cell type </typeparam>
    internal abstract class AbstractCellDataManager<TData, TCell> where TCell : AbstractCell
    {
        public TCell Shape { get; protected set ; }
        protected readonly List<TCell> OccupiedPosition = new ();
        public TData this[TCell cell]
        {
            get => Get(cell);
            set => Set(value, cell);
        }

        public abstract TData Get(TCell cell);
        public abstract void Set(TData data, TCell cell);
        public void ClearAll()
        {
            foreach (var cell in OccupiedPosition)
                ClearAtCell(cell);
        }
        public abstract void ClearAtCell(TCell cellCord);
        public abstract void ReShape(TCell cell);
        public abstract bool IsValidCord(TCell cell);


    }
}