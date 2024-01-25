using UnityEngine;
using UnityEngine.UIElements;

namespace VitoBarra.GridSystem.Editor.UI
{
    public class ButtonMatrix : VisualElement
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<ButtonMatrix>
        {
        }

        public int Horizontal = 4;
        public int Vertical = 4;

        public ButtonMatrix()
        {
            var visual = new VisualElement();


            visual.style.flexDirection = FlexDirection.Row;
            visual.style.flexWrap = Wrap.Wrap;
            for (int i = 0; i < Horizontal; i++)
            {
                for (int j = 0; j < Vertical; j++)
                {
                    var toggle = new Toggle();
                    toggle.style.marginTop = 1;
                    toggle.style.marginRight = 1;
                    toggle.style.marginBottom = 1;
                    toggle.style.marginLeft = 1;
                    visual.Add(toggle);
                }
            }
            Add(visual);
        }
    }
}