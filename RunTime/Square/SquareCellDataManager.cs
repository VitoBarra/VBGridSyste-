using System.Collections.Generic;
using VitoBarra.GeneralUtility.DataStructure;
using VitoBarra.GridSystem.Framework;
using NotImplementedException = System.NotImplementedException;

namespace VitoBarra.GridSystem.Square
{
    /// <summary>
    /// A class that implement a data manager for square cells use to interact with the underlying data structure
    /// </summary>
    /// <typeparam name="TData"> the Data type</typeparam>
    internal class SquareCellDataManager<TData> : AbstractCellDataManager<TData, SquareCell>
    {
        private readonly DynamicMatrix<TData> Matrix;

        public SquareCellDataManager(int row, int column, TData defaultValue = default)
        {
            Shape=new SquareCell(row,column);
            Matrix = new DynamicMatrix<TData>(row, column, defaultValue);
        }
        public SquareCellDataManager(SquareCell shape, TData defaultValue = default):this(shape.Row,shape.Column,defaultValue)
        {
        }



        public override void ClearAtCell(SquareCell cellCord)
        {
            Matrix.SetDefault(cellCord.Row, cellCord.Column);
        }

        public TData this[int row, int col]
        {
            get => Get(new SquareCell(row, col));
            set => Set(value, new SquareCell(row, col));
        }


        public override TData Get(SquareCell cell) => Matrix[cell.Row, cell.Column];

        public override void Set(TData data, SquareCell cell)
        {
            if(data is null) return;
            Matrix[cell.Row, cell.Column] = data;
            OccupiedPosition.Add(cell);
        }


        public override void ReShape(SquareCell cell)
        {
            Shape = new SquareCell(cell);
            Matrix.Resize(cell.Row, cell.Column);
        }
        public  void ReShape(int row , int col)
        {
            Shape = new SquareCell(row,col);
            Matrix.Resize(row, col);

        }

        public  override bool IsValidCord(SquareCell cell) => Matrix.IsValidCord(cell.Row, cell.Column);
    }
}