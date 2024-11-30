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
    private SerializedProperty skipHousePlacementSerializedProperty;
    private SerializedProperty _property;
    private SerializedProperty selectedAssemblyProject;
    private SerializedProperty skipAllSerializedProperty;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _property = property;
        skipHousePlacementSerializedProperty = property.FindPropertyRelative("_skipHousePlacementPhase");
        selectedAssemblyProject = property.FindPropertyRelative("_assemblyProject");
        skipAllSerializedProperty = property.FindPropertyRelative("_skipAll");
        var container = new VisualElement();

        m_assetTree.CloneTree(container);

        var skipHouseToggle = container.Q<Toggle>("SkipHousePhase-toggle");
        var skipAllToggle = container.Q<Toggle>("SkipAll-toggle");
        var objectField = container.Q<ObjectField>("AssemblyProject-ObjectField");
        skipHouseToggle.RegisterValueChangedCallback(OnValueChanged);
        skipAllToggle.RegisterValueChangedCallback(OnSkipAllValueChanged);
        objectField.RegisterValueChangedCallback(OnAssemblyProjectSelection);
       
        _optionsContainer = container.Q<VisualElement>("Options-VisualElement");
        skipHouseToggle.value = skipHousePlacementSerializedProperty.boolValue;
        skipAllToggle.value = skipAllSerializedProperty.boolValue;
        objectField.value = selectedAssemblyProject.objectReferenceValue;
        
        _optionsContainer.style.display = _optionsContainer.style.display = skipHousePlacementSerializedProperty.boolValue?DisplayStyle.Flex:DisplayStyle.None;
        
        

        return container;

    }

    private void OnSkipAllValueChanged(ChangeEvent<bool> evt)
    {
        skipAllSerializedProperty.boolValue = evt.newValue;
        _property.serializedObject.ApplyModifiedProperties();
        _property.serializedObject.Update();
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
        skipHousePlacementSerializedProperty.boolValue = evt.newValue;
        _property.serializedObject.ApplyModifiedProperties();
        _property.serializedObject.Update();


    }
}
