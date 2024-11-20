using System;
using _project.Scripts.Controllers;
using UnityEngine;

public class AnimatorBasedModuleSelector : MonoBehaviour, IModuleSelectorController
{
    [SerializeField] private AssemblyProjectScriptableObject _assemblyProject;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnModuleRayHover(GameObject sGameObject)
    {
        _animator.enabled = true;
    }

    public void OnModuleRayExit(GameObject sGameObject)
    {
        _animator.Rebind();
        _animator.Update(0f);
        _animator.enabled = false;
    }

    public void OnModuleSelection()
    {
        
    }

    public AssemblyProjectScriptableObject GetData()
    {
        return _assemblyProject;
    }
}
