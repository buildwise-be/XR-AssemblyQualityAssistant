using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BuildWize/Assembly Project", fileName =nameof(AssemblyProjectScriptableObject))]
public class AssemblyProjectScriptableObject : ScriptableObject
{
    public string m_name;
    public int m_id;
    [SerializeField] private AssemblyStep[] _assemblySteps;

    internal AssemblyStep GetStep(int index)
    {
        return _assemblySteps[index];
    }
}
