using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DictationPanelController : MonoBehaviour, IDictationPanelController
{
    private IAssemblyProcessMonitorUseCase _useCase;
    private bool _dictationProcessIsDone;

    public void SetUseCase(IAssemblyProcessMonitorUseCase useCase)
    {
        _useCase = useCase;
        _useCase.OnStartDictationProcess += OpenDictationPanel;
    }

    private void OpenDictationPanel()
    {
        Debug.Log("Open OpenDictationPanel");
        OnOpenPanel.Invoke();
    }

    public Action OnOpenPanel { get; set; }
    

    public void CloseDictation(string message)
    {
        
    }

    public void ProcessDictationData(List<string> data)
    {
        foreach (var VARIABLE in data)
        {
            if (string.IsNullOrEmpty(VARIABLE))
            {
                Debug.LogWarning("No data to save");
            }
        }
    }
}