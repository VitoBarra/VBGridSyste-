using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace VitoBarra.GridSystem.Editor
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : UnityEditor.Editor
    {
        public VisualTreeAsset InspectorXMLFile;

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement finalInspector = new VisualElement();

            InspectorElement.FillDefaultInspector(finalInspector, serializedObject, this);


            if (InspectorXMLFile == null)
                InspectorXMLFile =
                    AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                        "Packages/com.vitobarra.gridsystem/Editor/UI/GridManagerEditor.uxml");


            if (target is not GridManager gridManager)
                return finalInspector;

            InspectorXMLFile.CloneTree(finalInspector);
            var button = finalInspector.Q<Button>("DrawGrid");
            button.clickable.clicked += gridManager.SetUp;

            // Return the finished inspector UI
            return finalInspector;
        }
    }
}