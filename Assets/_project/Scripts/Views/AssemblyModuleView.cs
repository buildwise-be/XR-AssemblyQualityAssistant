using System;
using _project.Scripts.Controllers;
using UnityEngine;

public class AssemblyModuleView : MonoBehaviour
{
    [SerializeField]  GameObject[] _subModules;

    private IAssemblyProcessController _controller;
    [SerializeField] bool _isAssemblyProcessAdditive = true;
    private int _lastStepIndex;

    public void SetController(IAssemblyProcessController controller)
    {
        _controller = controller;
        _controller.OnDisplayStep += DisplayStep;
    }

    private void DisplayStep(int stepIndex, AssemblyStep arg2)
    {
        if (_isAssemblyProcessAdditive)
        {
            for (var i = 0; i < _subModules.Length; i++)
            {
                var animator = _subModules[i].GetComponent<IStepAnimation>();
                animator.Setup(stepIndex);
                
                if (i < stepIndex)
                {
                    animator?.StopAnimation();
                    continue;
                }

                if (_lastStepIndex >= stepIndex)
                {
                    animator?.PlayAnimationReverse();
                }
                else
                {
                    animator?.PlayAnimation();
                }
                
                _lastStepIndex = stepIndex;
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
