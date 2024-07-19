using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StepsManager))]
public class StepsManagerEditor : Editor
{
    private StepsManager stepsManager;
    private List<bool> stepFoldouts = new List<bool>();
    private SerializedProperty stepsProperty;

    private void OnEnable()
    {
        stepsManager = (StepsManager)target;
        stepsProperty = serializedObject.FindProperty("_steps");
        UpdateStepFoldouts();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Assembly Steps", EditorStyles.boldLabel);
        for (int i = 0; i < stepsProperty.arraySize; i++)
        {
            DrawStep(stepsProperty.GetArrayElementAtIndex(i), i);
        }
        if (GUILayout.Button("Add Step"))
        {
            AddStep();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStep(SerializedProperty stepProperty, int index)
    {
        stepFoldouts[index] = EditorGUILayout.Foldout(stepFoldouts[index], $"Step {stepProperty.FindPropertyRelative("StepID").intValue}");
        if (stepFoldouts[index])
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(stepProperty.FindPropertyRelative("StepID"));
            EditorGUILayout.PropertyField(stepProperty.FindPropertyRelative("StepIllustration"));
            SerializedProperty indicationsProperty = stepProperty.FindPropertyRelative("Indications");
            for (int i = 0; i < indicationsProperty.arraySize; i++)
            {
                DrawIndication(indicationsProperty.GetArrayElementAtIndex(i), i);
            }
            if (GUILayout.Button("Add Indication"))
            {
                AddIndication(indicationsProperty);
            }
            if (GUILayout.Button("Remove Step"))
            {
                RemoveStep(index);
                return;
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DrawIndication(SerializedProperty indicationProperty, int index)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(indicationProperty.FindPropertyRelative("IndicationID"), GUIContent.none, GUILayout.Width(30));
        EditorGUILayout.PropertyField(indicationProperty.FindPropertyRelative("IndicationText"), GUIContent.none);
        if (GUILayout.Button("Remove", GUILayout.Width(60)))
        {
            indicationProperty.DeleteCommand();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddStep()
    {
        int newIndex = stepsProperty.arraySize;
        stepsProperty.InsertArrayElementAtIndex(newIndex);
        SerializedProperty newStep = stepsProperty.GetArrayElementAtIndex(newIndex);
        newStep.FindPropertyRelative("StepID").intValue = newIndex + 1;
        newStep.FindPropertyRelative("Indications").ClearArray();
        UpdateStepFoldouts();
    }

    private void RemoveStep(int index)
    {
        stepsProperty.DeleteArrayElementAtIndex(index);
        UpdateStepIDs();
        UpdateStepFoldouts();
    }

    private void UpdateStepIDs()
    {
        for (int i = 0; i < stepsProperty.arraySize; i++)
        {
            SerializedProperty stepProperty = stepsProperty.GetArrayElementAtIndex(i);
            stepProperty.FindPropertyRelative("StepID").intValue = i + 1;
        }
    }

    private void AddIndication(SerializedProperty indicationsProperty)
    {
        int newIndex = indicationsProperty.arraySize;
        indicationsProperty.InsertArrayElementAtIndex(newIndex);
        SerializedProperty newIndication = indicationsProperty.GetArrayElementAtIndex(newIndex);
        newIndication.FindPropertyRelative("IndicationID").intValue = newIndex + 1;
        newIndication.FindPropertyRelative("IndicationText").stringValue = "";
    }

    private void UpdateStepFoldouts()
    {
        stepFoldouts.Clear();
        for (int i = 0; i < stepsProperty.arraySize; i++)
        {
            stepFoldouts.Add(false);
        }
    }
}