using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts.Controllers;
using UnityEngine;

[CreateAssetMenu(menuName ="BuildWize/Assembly Project", fileName =nameof(AssemblyProjectScriptableObject))]
public class AssemblyProjectScriptableObject : ScriptableObject
{
    
    [ScriptableObjectId] public string m_guid;
    public string m_name;
    [SerializeField] private AssemblyStep[] _assemblySteps;

    public int StepsCount => _assemblySteps.Length;

    internal AssemblyStep GetStep(int index)
    {
        return _assemblySteps[index];
    }
}
