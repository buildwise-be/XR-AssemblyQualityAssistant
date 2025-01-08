using System;
using System.Collections.Generic;
using _project.Scripts.Controllers;
using TMPro;
using UnityEngine;

public class StepsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _debugText;
    public int CurrentStepIndex 
    {
        get { return _currentStepIndex; }
        private set { }
    }

    [SerializeField]
    private List<AssemblyStep> _steps = new List<AssemblyStep>();

    private int _currentStepIndex = 0;

    public void NextStep()
    {
        if (_debugText != null)
            _debugText.text += "\n_currentStepIndex++";
        _currentStepIndex++;
    }

    // Public getter for _steps
    public List<AssemblyStep> GetSteps()
    {
        return _steps;
    }

    // Public setter for _steps
    public void SetSteps(List<AssemblyStep> steps)
    {
        _steps = steps;
    }

    public void StopStepAnimation()
    {
        Debug.Log("StopStepAnimation");
        Debug.Log("CurrentStepIndex: " + _currentStepIndex);

        /*foreach (StepAnimator animator in FindObjectsOfType<StepAnimator>())
        {
            if (animator.Step == _currentStepIndex + 1)
            {
                Debug.Log("StopAnimation");
                animator.LeaveStep();
                break;
            }   
        }*/
    }
}