using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VitoBarra.GridSystem
{
    [ExecuteInEditMode]
    public class GridPlaceable : MonoBehaviour
    {
        GridManager GridManager;


        private void Awake()
        {
            GridManager = GetComponentInParent<GridManager>();
        }

        private void Start()
        {
            GridManager.OnGridChange += HoldOnGrid;
            transform.position = GridManager.GetNearestCenterCell(transform.position);
            GridManager.OccupiesPosition(gameObject);
        }


        private void OnEnable()
        {
            transform.position = GridManager.GetNearestCenterCell(transform.position);
        }


        Vector2Int CurrentCell;
        private void HoldOnGrid()
        {
           transform.position = GridManager.GetCenterCell(CurrentCell);
        }

        private void SnapToGrid()
        {
            CurrentCell = GridManager.MoveBetweenCells(this, StartPosition, FinalPosition);
        }

        Vector3 StartPosition;
        Vector3 FinalPosition;

        private void OnMouseDown()
        {
            StartPosition = gameObject.transform.position;
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
            FinalPosition = gameObject.transform.position;
            SnapToGrid();
        }

        private void OnDestroy()
        {
            GridManager.OnGridChange -= HoldOnGrid;
        }
    }
}