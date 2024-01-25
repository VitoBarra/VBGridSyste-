using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VitoBarra.GridSystem.Editor
{
    [CustomEditor(typeof(SquareGridPlaceable))]

    public class SquareGridPlaceableEditor : UnityEditor.Editor
    {

        public VisualTreeAsset InspectorXMLFile;

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);


            if (InspectorXMLFile == null)
                InspectorXMLFile = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>( "Packages/com.vitobarra.gridsystem/Editor/UI/GridPlaceable.uxml");



            if (target is not SquareGridPlaceable gridManager)
                return myInspector;

            // InspectorXMLFile.CloneTree(myInspector);
            // var button = myInspector.Q<Button>("DrawGrid");
            // button.clickable.clicked += gridManager.SetUp;

            // Return the finished inspector UI
            return myInspector;
        }
    }
}