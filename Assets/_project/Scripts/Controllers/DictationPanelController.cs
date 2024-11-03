using System;
using System.Collections.Generic;
using _project.Scripts.Controllers.DTO;
using _project.Scripts.UseCases;
using UnityEngine;

namespace _project.Scripts.Controllers
{
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
            OnOpenPanel?.Invoke();
        }

        public Action OnOpenPanel { get; set; }
        public Action OnClosePanel { get; set; }
        public Action OnRefreshPanel { get; set; }
    

        public void StopDictationProcess()
        {
            OnClosePanel?.Invoke();
        }

        public void ProcessDictationData(string data)
        {
            if (!string.IsNullOrEmpty(data)) _useCase.AddRemark(data);
            OnRefreshPanel?.Invoke();
        }
        
        public IRemarksCollection GetSavedRemarks()
        {
            var data = _useCase.GetCurrentStepRemarkList();
            var messages = data.GetMessages();
            return new AssemblyRemarkCollection(messages);
        }
    }
}