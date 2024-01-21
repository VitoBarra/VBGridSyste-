using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace VitoBarra.GridSystem.Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        public VisualTreeAsset m_InspectorXML;

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);


            if (m_InspectorXML == null)
                m_InspectorXML =
                    AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                        "Packages/com.vitobarra.gridsystem/Editor/UI/GridManagerEditor.uxml");

            if (target is not GridManager gridManager)
                return myInspector;

            m_InspectorXML.CloneTree(myInspector);
            var button = myInspector.Q<Button>("DrawGrid");
            button.clickable.clicked += gridManager.SetUp;

            // Return the finished inspector UI
            return myInspector;
        }
    }
}