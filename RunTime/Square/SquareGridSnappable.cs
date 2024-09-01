using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VitoBarra.GridSystem.Framework;

namespace VitoBarra.GridSystem.Square
{
    [ExecuteInEditMode]
    public class SquareGridSnappable : GridSnappable<SquareCell>
    {
        SquaredGridManager GridManager;
        public SquereGridSnappableElement SnappableElementPrefab;

        public int VerticalMaxSpan = 1;
        public int HorizontalMaxSpan = 1;
        [SerializeField]private List<bool> PositionBitMap= new List<bool>();


        private SquareCell NearestCell => GridManager.GetNearestCell(transform.position);

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

        bool UpDateCellAndPosition()
        {
            return UpDateCellAndPosition(GridManager);
        }

        protected override void HoldOnGrid()
        {
            transform.position = GridManager.GetWordPositionCenterCell(Cell);
        }

        protected override void SnapToGrid()
        {
            var isMovementPossible =
                GridManager.MoveBetweenCells(GetAllOccupiedCell(), GetAllOccupiedCell(NearestCell));
            if (isMovementPossible)
                Cell = NearestCell;
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
            for (int i = 0; i < transform.childCount; i++)
                DestroyImmediate(transform.GetChild(i).gameObject);
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
    }
}