using System;
using UnityEngine;

public class AssemblyProcessController : MonoBehaviour, IAssemblyProcessController
{
    public Action<AssemblyStep> OnDisplayStep {  get; set; }
    [SerializeField] private AppData _appData;

    private AssemblyProjectScriptableObject _currentProject;
    private int _currentStepIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentProject = _appData.project;
        _currentStepIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextStep();

        }
#endif

    }

    public void LoadFirstStep()
    {
        _currentStepIndex = 0;
        OnDisplayStep.Invoke(_currentProject.GetStep(0));
    }

    public void LoadNextStep()
    {
        _currentStepIndex++;
        var step = _currentProject.GetStep(_currentStepIndex);
        OnDisplayStep?.Invoke(step);
    }

}
