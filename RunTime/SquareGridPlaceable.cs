using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem
{
    public class SquareGridPlaceable : MonoBehaviour
    {
        GridManager GridManager;
        private SquareCell PinCell { get; set; }
        public int VerticalMaxSpan = 1;
        public int HorizontalMaxSpan = 1;
        public List<bool> PositionBitMap;
        private SquareCell NearestCell => GridManager.GetNearestCell(transform.position);

        private void Awake()
        {
            GridManager = GetComponentInParent<GridManager>();
        }

        private void Start()
        {
            GridManager.OnGridChange += HoldOnGrid;
            var generatedCell = NearestCell;
            transform.position = GridManager.GetCenterCell(generatedCell);
            GridManager.OccupiesPosition(gameObject, GetPositionToOccupy(generatedCell));
            PinCell = generatedCell;
        }


        private void OnEnable()
        {
            if (GridManager != null)
                transform.position = GridManager.GetNearestCellCenter(transform.position);
        }


        private void HoldOnGrid()
        {
            transform.position = GridManager.GetCenterCell(PinCell);
        }

        private void SnapToGrid()
        {
            var IsMovementPossibile = GridManager.MoveBetweenCells(GetPositionToOccupy(PinCell),
                GetPositionToOccupy(NearestCell), gameObject);
            if (IsMovementPossibile)
                PinCell = NearestCell;
            transform.position = GridManager.GetCenterCell(PinCell);
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

        private IList<SquareCell> GetPositionToOccupy(SquareCell generatedCell)
        {
            var result = new List<SquareCell>();

            for (int i = 0; i < VerticalMaxSpan; i++)
            for (int j = 0; j < HorizontalMaxSpan; j++)
                if (PositionBitMap[i * HorizontalMaxSpan + j])
                    result.Add(new SquareCell(generatedCell.I + i, generatedCell.J + j));

            return result;
        }
    }
}