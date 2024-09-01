using System.Collections.Generic;
using UnityEngine;

namespace VitoBarra.GridSystem.Framework
{
    /// <summary>
    /// This rappresent one single object on one single cell on the grid
    /// </summary>
    /// <typeparam name="TCell"></typeparam>
    public class GridSnappableElement<TCell> : GridSnappable<TCell> where TCell : AbstractCell
    {
        public GridSnappable<TCell> SnappablePivo;

        GridManager<TCell,GameObject> GridManager;


        private void Awake()
        {
            GridManager = GetComponentInParent<GridManager<TCell,GameObject>>();
            UpDateCellAndPosition();
        }

        private void Start()
        {
            //this keep the object on the grid when the TCell dimension change or the grid shape change
            if (GridManager == null) return;
            GridManager.OnGridChange += HoldOnGrid;
        }


        private void OnEnable()
        {
            UpDateCellAndPosition();
        }

        private void UpDateCellAndPosition()
        {
            UpDateCellAndPosition(GridManager);
        }



        protected override void HoldOnGrid()
        {
            transform.position = GridManager.GetWordPositionCenterCell(Cell);
        }


        protected override void SnapToGrid()
        {
            if (SnappablePivo == null) return;
            transform.position = GridManager.GetWordPositionCenterCell(Cell);
            OnCellSet?.Invoke(Cell);
        }

        private void OnMouseDrag()
        {
            if (Camera.main == null) return;
            var screenCord = Input.mousePosition;
            screenCord.z = Camera.main.nearClipPlane + 1;
            var displacement = Camera.main.ScreenToWorldPoint(screenCord) - gameObject.transform.position;
            SnappablePivo.transform.position += displacement;
        }

        private void OnMouseUpAsButton()
        {
            SnapToGrid();
        }

        private void OnDestroy()
        {
            if (GridManager == null) return;
            GridManager.OnGridChange -= HoldOnGrid;
        }




        
    }
}