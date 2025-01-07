using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//[CustomEditor(typeof(AssemblyProjectScriptableObject))]
public class AssemblyProjectInspector : Editor
{
    public VisualTreeAsset m_assetTree;

    public override VisualElement CreateInspectorGUI()
    {

        if (m_assetTree == null) return base.CreateInspectorGUI();
        var inspector = new VisualElement();
        m_assetTree.CloneTree(inspector);

        VisualElement InspectorFoldout = inspector.Q("Default_Inspector");

        // Attach a default Inspector to the Foldout.
        InspectorElement.FillDefaultInspector(InspectorFoldout, serializedObject, this);

        return inspector;


    }
}
