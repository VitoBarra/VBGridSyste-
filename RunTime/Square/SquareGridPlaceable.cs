using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    [ExecuteInEditMode]
    public class SquareGridPlaceable : GridSnappable<SquareCell>
    {
        SquaredGridManager GridManager;

        public int VerticalMaxSpan = 1;
        public int HorizontalMaxSpan = 1;
        public List<bool> PositionBitMap;
        private SquareCell NearestCell => GridManager.GetNearestCell(transform.position);

        private void Awake()
        {
            GridManager = GetComponentInParent<SquaredGridManager>();
        }

        private void Start()
        {
            if (GridManager == null) return;

            GridManager.OnGridChange += HoldOnGrid;
            var generatedCell = NearestCell;
            transform.position = GridManager.GetWordPositionCenterCell(generatedCell);
            GridManager.OccupiesCell(gameObject, GetAllOccupiedCell(generatedCell));
            PinCell = generatedCell;
        }


        private void OnEnable()
        {
            if (GridManager != null)
                transform.position = GridManager.GetNearestCellCenter(transform.position);
        }


        protected override void HoldOnGrid()
        {
            transform.position = GridManager.GetWordPositionCenterCell(PinCell);
        }

        protected override void SnapToGrid()
        {
            var IsMovementPossibile = GridManager.MoveBetweenCells(GetAllOccupiedCell(),GetAllOccupiedCell(NearestCell), gameObject);
            if (IsMovementPossibile)
                PinCell = NearestCell;
            transform.position = GridManager.GetWordPositionCenterCell(PinCell);
            OnCellSet?.Invoke(PinCell);
        }

        public override IList<SquareCell> GetAllOccupiedCell()
        {
            return PinCell == null ? null : GetAllOccupiedCell(PinCell);
        }

        private IList<SquareCell> GetAllOccupiedCell(SquareCell generatedCell)
        {
            var result = new List<SquareCell>();



            for (int i = 0; i < VerticalMaxSpan; i++)
            for (int j = 0; j < HorizontalMaxSpan; j++)
                if (PositionBitMap[i * HorizontalMaxSpan + j])
                    result.Add(new SquareCell(generatedCell.Row + i, generatedCell.Column + j));

            return result;
        }


        private void OnMouseDrag()
        {
            var screenCord = Input.mousePosition;
            if (Camera.main == null) return;
            screenCord.z = Camera.main.nearClipPlane + 1;
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(screenCord);
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

        private void OnValidate()
        {
            PositionBitMap ??= new List<bool>();
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
    }
}