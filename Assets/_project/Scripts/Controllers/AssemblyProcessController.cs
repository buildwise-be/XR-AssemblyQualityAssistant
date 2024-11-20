using System;
using _project.Scripts.UseCases;
using UnityEngine;

namespace _project.Scripts.Controllers
{
    public class AssemblyProcessController : MonoBehaviour, IAssemblyProcessController
    {
        public Action<int, AssemblyStep> OnDisplayStep {  get; set; }
        

        public Action OnShowPanel { get; set; }
        [SerializeField] private AppData _appData;

        private AssemblyProjectScriptableObject _currentProject;
        private int _currentStepIndex;
        private float _stepStartTime;

        private IAssemblyProcessMonitorUseCase _assemblyProcessMonitorUseCase;

        private AssemblyStep _currentStep;
    
        public void SetMonitor(IAssemblyProcessMonitorUseCase monitor)
        {
            _assemblyProcessMonitorUseCase = monitor;
            _currentProject = _appData.project;
            _currentStepIndex = -1;
            _assemblyProcessMonitorUseCase.OnStartAssemblyProcessEvent += StartAssembly;
            _assemblyProcessMonitorUseCase.OnShowAssemblyPanel += ShowPanel;
            _assemblyProcessMonitorUseCase.OnPrematureAssemblyEnd += EndMonitoring;
            _assemblyProcessMonitorUseCase.OnNewAssemblyProcessDataCreationEvent += CreateNewAssembly;
        }

        private void CreateNewAssembly()
        {
            _assemblyProcessMonitorUseCase.CreateNewAssemblyProcessData(_appData.project.m_guid,_appData.project.StepsCount);
        }

        private void EndMonitoring()
        {
            _assemblyProcessMonitorUseCase.EndMonitoring();
            OnEndProcessEvent?.Invoke();
        }

        public Sprite GetAssemblyStepIllustation(int index)
        {
            return _currentProject.GetStep(index).StepIllustration;
        }

        private void ShowPanel()
        {
            OnShowPanel?.Invoke();
        }

        private void StartAssembly()
        {
            LoadFirstStep();
        
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

        private void LoadFirstStep()
        {
            _currentStepIndex = 0;
            LoadStep(_currentStepIndex);
        }

        private void LoadNextStep()
        {
            _currentStepIndex++;
            if(_currentStepIndex < _currentProject.StepsCount) LoadStep(_currentStepIndex);
            else
            {
                _assemblyProcessMonitorUseCase.EndMonitoring();
                OnEndProcessEvent?.Invoke();
            }
        }

        private void LoadStep(int index)
        {
        
            _assemblyProcessMonitorUseCase.StartStepMonitoring(index, Time.time);
            _currentStep = _currentProject.GetStep(index);
            OnDisplayStep?.Invoke(index,_currentStep);
        }

        public int TotalNumberOfSteps => _currentProject.StepsCount;
        public Action OnEndProcessEvent { get; set; }

        public void ValidateStep()
        {
            _assemblyProcessMonitorUseCase.EndStepMonitoring(_currentStepIndex, Time.time);
            LoadNextStep();
        }
    

        public void GoToPreviousStep()
        {
            _currentStepIndex--;
            LoadStep(_currentStepIndex);
        }

        public void OpenDictationPanelForRemarkReporting()
        {
            _assemblyProcessMonitorUseCase.InitializeDictationForRemarkReporting();
        }

        public void OpenDictationPanelForIssueReporting()
        {
            _assemblyProcessMonitorUseCase.InitializeDictationForIssueReporting();
        }

        public AssemblyProcessDataDto[] GetAssemblyProcessData()
        {
            var data = _assemblyProcessMonitorUseCase.GetAssemblyProcessData();
            var result = new AssemblyProcessDataDto[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = new AssemblyProcessDataDto(
                    data.StepDuration[i],
                    data.StepSessions[i],
                    data.nbOfIssues[i],
                    data.nbOfRemarks[i],
                    data.Issues[i],
                    data.Remarks[i]);

            }

            return result;
        }

        public string GetAssemblyStepTitle(int i)
        {
            return _currentProject.GetStep(i).Title;
        }
    }
}