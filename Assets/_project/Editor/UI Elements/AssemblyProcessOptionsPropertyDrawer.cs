using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AssemblyProcessOptions))]
public class AssemblyProcessOptionsPropertyDrawer : PropertyDrawer
{
    public VisualTreeAsset m_assetTree;
    private VisualElement _optionsContainer;
    private SerializedProperty serializedProperty;
    private SerializedProperty _property;
    private SerializedProperty selectedAssemblyProject;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _property = property;
        serializedProperty = property.FindPropertyRelative("_skipHousePlacementPhase");
        selectedAssemblyProject = property.FindPropertyRelative("_assemblyProject");
        var container = new VisualElement();

        m_assetTree.CloneTree(container);

        var toggle = container.Q<Toggle>("SkipHousePhase-toggle");
        var objectField = container.Q<ObjectField>("AssemblyProject-ObjectField");
        toggle.RegisterValueChangedCallback(OnValueChanged);
        objectField.RegisterValueChangedCallback(OnAssemblyProjectSelection);
       
        _optionsContainer = container.Q<VisualElement>("Options-VisualElement");
        toggle.value = serializedProperty.boolValue;
        objectField.value = selectedAssemblyProject.objectReferenceValue;
        
        _optionsContainer.style.display = _optionsContainer.style.display = serializedProperty.boolValue?DisplayStyle.Flex:DisplayStyle.None;
        
        

        return container;

    }

    private void OnAssemblyProjectSelection(ChangeEvent<Object> evt)
    {
        selectedAssemblyProject.objectReferenceValue = evt.newValue;
        
        _property.serializedObject.ApplyModifiedProperties();
        _property.serializedObject.Update();
    }

    private void OnValueChanged(ChangeEvent<bool> evt)
    {
        _optionsContainer.visible = evt.newValue;
        _optionsContainer.style.display = evt.newValue?DisplayStyle.Flex:DisplayStyle.None;
        serializedProperty.boolValue = evt.newValue;
        _property.serializedObject.ApplyModifiedProperties();
        _property.serializedObject.Update();


    }
}
