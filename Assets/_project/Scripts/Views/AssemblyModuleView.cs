using System;
using _project.Scripts.Controllers;
using UnityEngine;

public class AssemblyModuleView : MonoBehaviour
{
    [SerializeField]  GameObject[] _subModules;

    private IAssemblyProcessController _controller;
    [SerializeField] bool _isAssemblyProcessAdditive = true;

    public void SetController(IAssemblyProcessController controller)
    {
        _controller = controller;
        _controller.OnDisplayStep += DisplayStep;
    }

    private void DisplayStep(int arg1, AssemblyStep arg2)
    {
        if (_isAssemblyProcessAdditive)
        {
            for (var i = 0; i < _subModules.Length; i++)
            {
                if (i > arg1)
                {
                    _subModules[i].SetActive(false);
                    continue;
                }
                _subModules[i].SetActive(true);
                var animator = _subModules[i].GetComponent<StepAnimator>();
                if (i < arg1)
                {
                    if(animator) animator.StopAnimation();
                    continue;
                }
                if(animator) animator.PlayAnimation();
            }
        }
    }

    private void Awake()
    {
        foreach (var subModule in _subModules) subModule.SetActive(false);
    }
    
    private void OnDisable()
    {
        foreach (var subModule in _subModules) subModule.SetActive(false);
    }
}
