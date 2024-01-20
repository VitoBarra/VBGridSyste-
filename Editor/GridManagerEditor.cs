using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VitoBarra.Unity.GeneralSystem;


[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public VisualTreeAsset m_InspectorXML;

    public override VisualElement CreateInspectorGUI()
    {
        
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();
    
        InspectorElement.FillDefaultInspector(myInspector, serializedObject, this);
        m_InspectorXML?.CloneTree(myInspector);
        var button = myInspector.Q<Button>("DrawGrid");
        button.clickable.clicked += ((GridManager)target).SetUpDrawer;
        // Return the finished inspector UI
        return myInspector;
    }
    

}