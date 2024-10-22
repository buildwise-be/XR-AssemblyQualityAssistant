using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AssemblyProcessEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/BuildWize/Assembly Process Editor &g")]
    public static void ShowExample()
    {
        AssemblyProcessEditor wnd = GetWindow<AssemblyProcessEditor>();
        wnd.titleContent = new GUIContent("Assembly Process Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
