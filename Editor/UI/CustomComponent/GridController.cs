using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using VitoBarra.GridSystem.POCO;
using VitoBarra.GridSystem.POCO.CellType;

namespace VitoBarra.GridSystem.Editor.UI
{
    public class GridController : VisualElement
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<GridController, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription Col = new UxmlIntAttributeDescription { name = "Columns", defaultValue = 3 };
            UxmlIntAttributeDescription Row = new UxmlIntAttributeDescription { name = "Rows", defaultValue = 3 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((GridController)ve).Rows = Row.GetValueFromBag(bag, cc);
                ((GridController)ve).Columns = Col.GetValueFromBag(bag, cc);
            }
        }

        private int rows;

        public int Rows
        {
            get => rows;
            set
            {
                rows = value;
                InitializeGrid();
            }
        }

        private int columns;

        public int Columns
        {
            get => columns;
            set
            {
                columns = value;
                InitializeGrid();
            }
        }

        public DynamicMatrix<UnityEngine.UI.Toggle> Grid;

        public GridController()
        {
            Grid = new DynamicMatrix<UnityEngine.UI.Toggle>(Columns, Rows);
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            this.Clear();
            this.style.marginTop = 5;
            this.style.marginBottom = 5;
            this.style.marginLeft = 5;
            this.style.marginRight = 5;
            Grid.Resize(Columns, Rows);

            var gridBox = new VisualElement();
            gridBox.name = "GridBox";
            gridBox.style.flexDirection = FlexDirection.Row;
            // gridBox.style.width = Length.Percent(80)  ;
            gridBox.style.marginTop = 5;
            gridBox.style.marginRight = 5;
            gridBox.style.marginBottom = 5;
            gridBox.style.marginLeft = 5;

            for (int i = 0; i < Columns; i++)
            {
                var column = new VisualElement();
                column.style.width = Length.Percent(100.0f / Columns);
                column.style.height = Length.Percent(100);
                for (int j = 0; j < Rows; j++)
                {
                    var rowElement = new VisualElement();
                    rowElement.style.height = Length.Percent(100);
                    rowElement.style.width = Length.Percent(100);
                    rowElement.style.alignContent = Align.Center;
                    rowElement.style.justifyContent = Justify.Center;
                    rowElement.style.alignSelf = Align.Center;

                    var toggle = new Toggle();


                    toggle.style.alignSelf = Align.Center;

                    toggle.style.marginTop = 0;
                    toggle.style.marginRight = 0;
                    toggle.style.marginBottom = 0;
                    toggle.style.marginLeft = 0;
                    rowElement.Add(toggle);
                    column.Add(rowElement);
                }

                gridBox.Add(column);
            }

            Add(gridBox);
        }
    }
}