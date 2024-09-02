using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VitoBarra.GridSystem.Framework;
using Vector3 = System.Numerics.Vector3;

namespace VitoBarra.GridSystem.Square
{
    [ExecuteInEditMode]
    public class SquareGridSnappable : GridSnappable<SquareCell>
    {
        SquaredGridManager GridManager;
        public SquereGridSnappableElement SnappableElementPrefab;

        public int VerticalMaxSpan = 1;
        public int HorizontalMaxSpan = 1;
        [SerializeField]
        private List<bool> PositionBitMap= new List<bool>();
        


        private void Awake()
        {
            GridManager = GetComponentInParent<SquaredGridManager>();
            GridManager.OnGridChange += HoldOnGrid;

        }

        private void OnEnable()
        {
            if (!UpDateCellAndPosition()) return;
            GridManager.OccupiesCell(gameObject, GetAllOccupiedCell(Cell));

            Instansiate();
        }

        private void Instansiate()
        {
            for (int i = 0; i < VerticalMaxSpan; i++)
            for (int j = 0; j < HorizontalMaxSpan; j++)
                if (PositionBitMap[i * HorizontalMaxSpan + j])
                {
                    var instance= Instantiate(SnappableElementPrefab, GridManager.GetWordPositionCenterCell(Cell.Add(i,j)), Quaternion.identity, transform);
                    instance.SnappablePivo = this;
                }
        }

        private void OnDisable()
        {
            ClearChildren();
            GridManager?.DeleteAtCell(GetAllOccupiedCell());
        }

        public override bool UpDateCellAndPosition()
        {
            return UpDateCellAndPosition(GridManager);
        }

        protected override void HoldOnGrid()
        {
            transform.position = GridManager.GetWordPositionCenterCell(Cell);
        }

        public override void SnapToGrid()
        {
            var nearestCell =GridManager.GetNearestCell(transform.position);
            var isMovementPossible =
                GridManager.MoveBetweenCells(GetAllOccupiedCell(), GetAllOccupiedCell(nearestCell));
            if (isMovementPossible)
                Cell = nearestCell;
            transform.position = GridManager.GetWordPositionCenterCell(Cell);
            OnCellSet?.Invoke(Cell);
        }

        [CanBeNull]
        public IList<SquareCell> GetAllOccupiedCell()
        {
            return Cell is null ? null : GetAllOccupiedCell(Cell);
        }

        private IList<SquareCell> GetAllOccupiedCell(SquareCell generatedCell)
        {
            if (generatedCell is null) return null;
            var result = new List<SquareCell>();


            for (int i = 0; i < VerticalMaxSpan; i++)
            for (int j = 0; j < HorizontalMaxSpan; j++)
                if (PositionBitMap[i * HorizontalMaxSpan + j])
                    result.Add(generatedCell.Add( i,  j));

            return result;
        }


        private void OnMouseDrag()
        {
            if (Camera.main == null) return;
            var screenCord = Input.mousePosition;
            screenCord.z = Camera.main.nearClipPlane + 1;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(screenCord);
        }

        private void OnMouseUpAsButton()
        {
            SnapToGrid();
        }

        private void ClearChildren()
        {
            var totalchlids = transform.childCount;
            for (int i = 0; i < totalchlids; i++)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
        private void OnDestroy()
        {
            if (GridManager is null) return;
            GridManager.OnGridChange -= HoldOnGrid;

        }

        private void OnValidate()
        {
            var countDifference = PositionBitMap.Count - (VerticalMaxSpan * HorizontalMaxSpan);
            if (countDifference == 0) return;

            switch (countDifference)
            {
                case > 0:
                    PositionBitMap.RemoveRange(PositionBitMap.Count - countDifference, countDifference);
                    break;
                case < 0:
                    for (int i = 0; i < -countDifference; i++)
                        PositionBitMap.Add(true);
                    break;
            }

        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position,  0.3f);
        }
    }
}