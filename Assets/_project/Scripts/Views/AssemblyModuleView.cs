using System;
using _project.Scripts.Controllers;
using UnityEngine;

public class AssemblyModuleView : MonoBehaviour
{
    [SerializeField]  GameObject[] _subModules;
    private IAssemblyProcessController _controller;
    private int _lastStepIndex;

    public void SetController(IAssemblyProcessController controller)
    {
        _controller = controller;
        _controller.OnDisplayStep += DisplayStep;
    }

    private void DisplayStep(int stepIndex, AssemblyStep arg2)
    {
        for (var i = 0; i < _subModules.Length; i++)
        {
            var stepBehaviour = _subModules[i].GetComponent<IStepBehaviour>();
            
            
            stepBehaviour.Initialize(stepIndex);
                
            if (i < stepIndex)
            {
                stepBehaviour?.LeaveStep();
                continue;
            }

            if (_lastStepIndex > stepIndex)
            {
                stepBehaviour?.EnterStepReverse();
            }
            else
            {
                stepBehaviour?.EnterStep();
            }
                
            _lastStepIndex = stepIndex;
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
