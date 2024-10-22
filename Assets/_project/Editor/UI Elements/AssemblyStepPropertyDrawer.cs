using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
[CustomPropertyDrawer(typeof(AssemblyStep))]
public class AssemblyStepPropertyDrawer : PropertyDrawer
{
    public VisualTreeAsset m_assetTree;
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        m_assetTree.CloneTree(container);
        return container;

    }
}
