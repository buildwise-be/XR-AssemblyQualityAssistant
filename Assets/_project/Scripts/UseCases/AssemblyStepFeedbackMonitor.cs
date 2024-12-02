using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Entities;
using _project.Scripts.Gateways;


namespace _project.Scripts.UseCases
{
    public class AssemblyStepFeedbackMonitor : IAssemblyProcessMonitorUseCase
    {
        private AssemblyProcessDataEntity _assemblyProcessDataEntity;
        private int _currentStepIndex;
        private readonly IFeedbackDataLoaderGateway _feedbackDataLoader;
        private AssemblyRemark.TYPE _currentReportType;
        private float _stepStartTimer;

        public Action OnStartDictationProcess { get; set; }
        public Action OnStartAssemblyProcessEvent { get; set; }
        public Action OnShowAssemblyPanel { get; set; }
        public Action OnPrematureAssemblyEnd { get; set; }

        public AssemblyStepFeedbackMonitor(IFeedbackDataLoaderGateway feedbackDataLoader)
        {
            _feedbackDataLoader = feedbackDataLoader;
        }
        
        public void InitMonitoring(string projectId)
        {
            _assemblyProcessDataEntity = _feedbackDataLoader.GetAssemblyData(projectId);
            if (_assemblyProcessDataEntity != null)
            {
                ClearSessionData(_assemblyProcessDataEntity);
                OnStartAssemblyProcessEvent?.Invoke();
                OnShowAssemblyPanel?.Invoke();
                StopAssemblyOnDictationEnd = false;
                return;
            }
            
            OnNewAssemblyProcessDataCreationEvent?.Invoke();
            
            //_assemblyProcessDataEntity.m_assemblySteps = new AssemblyStepData[]
            
        }

        private void ClearSessionData(AssemblyProcessDataEntity assemblyProcessDataEntity)
        {
            foreach (var step in _assemblyProcessDataEntity.m_assemblySteps)
            {
                step.m_duration = 0;
                step.m_nbSessions = 0;
            }
        }

        public Action OnNewAssemblyProcessDataCreationEvent { get; set; }

        public void CreateNewAssemblyProcessData(string projectId, int nbOfSteps)
        {
            _assemblyProcessDataEntity = new AssemblyProcessDataEntity.Builder()
                .Create(projectId)
                .WithNumberOfSteps(nbOfSteps)
                .Build();
            
            OnStartAssemblyProcessEvent?.Invoke();
            OnShowAssemblyPanel?.Invoke();
            StopAssemblyOnDictationEnd = false;
        }

        public void StartStepMonitoring(int index,float time)
        {
            _currentStepIndex = index;
            _stepStartTimer = time;
            _assemblyProcessDataEntity.StartStep(index);
        }

        public void EndStepMonitoring(int index, float time)
        {
            var duration = time - _stepStartTimer;
            _assemblyProcessDataEntity.SetStepDuration(index,duration);
            _feedbackDataLoader.SaveData(_assemblyProcessDataEntity);
        }

        public void EndMonitoring()
        {
            
        }
        
        
        public IRemarkDto GetCurrentStepRemarkList()
        {
            var remarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex);
            var data = new string[remarks.Length];
            var types = new IRemarkDto.RemarkType[remarks.Length];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = remarks[i].m_message;
                types[i] = remarks[i].m_type == AssemblyRemark.TYPE.REMARK
                    ? IRemarkDto.RemarkType.Remark
                    : IRemarkDto.RemarkType.Issue;
            }
            return new RemarkData(data, types); 
        }

        public void StopDictation()
        {
            if (StopAssemblyOnDictationEnd)
            {
                EndMonitoring();
                OnPrematureAssemblyEnd?.Invoke();
            }
            else
            {
                OnShowAssemblyPanel.Invoke();
            }
        }

        public void InitializeDictationForRemarkReporting()
        {
            _currentReportType = AssemblyRemark.TYPE.REMARK;
            OnStartDictationProcess.Invoke();
        }

        public void InitializeDictationForIssueReporting()
        {
            _currentReportType = AssemblyRemark.TYPE.ISSUE;
            OnStartDictationProcess.Invoke();
        }

        public IStepDataDto GetAssemblyProcessData()
        {
            var durations = new float[_assemblyProcessDataEntity.m_assemblySteps.Length];
            var stepSessions = new int[_assemblyProcessDataEntity.m_assemblySteps.Length];
            var nbOfRemarks = new int[_assemblyProcessDataEntity.m_assemblySteps.Length];
            var nbOfIssues = new int[_assemblyProcessDataEntity.m_assemblySteps.Length];
            var issues = new List<string[]>
            {
                Capacity = _assemblyProcessDataEntity.m_assemblySteps.Length
            };
            var remarks = new List<string[]>
            {
                Capacity = _assemblyProcessDataEntity.m_assemblySteps.Length
            };
            
            
            for (var i = 0; i < _assemblyProcessDataEntity.m_assemblySteps.Length; i++)
            {
                durations[i] = _assemblyProcessDataEntity.m_assemblySteps[i].m_duration;
                stepSessions[i] = _assemblyProcessDataEntity.m_assemblySteps[i].m_nbSessions;
                nbOfRemarks[i] = _assemblyProcessDataEntity.m_assemblySteps[i].GetNumberOfEntries(AssemblyRemark.TYPE.REMARK);
                nbOfIssues[i] = _assemblyProcessDataEntity.m_assemblySteps[i].GetNumberOfEntries(AssemblyRemark.TYPE.ISSUE);
                remarks.Add(_assemblyProcessDataEntity.m_assemblySteps[i].GetEntries(AssemblyRemark.TYPE.REMARK));
                issues.Add(_assemblyProcessDataEntity.m_assemblySteps[i].GetEntries(AssemblyRemark.TYPE.ISSUE));
            }
            return new SimpleStepDataDto
            {
                Length = _assemblyProcessDataEntity.m_assemblySteps.Length,
                StepDuration = durations,
                StepSessions = stepSessions,
                nbOfIssues = nbOfIssues,
                nbOfRemarks = nbOfRemarks,
                Remarks = remarks,
                Issues = issues
                
            };
        }

        public bool IsInIssueMode => _currentReportType == AssemblyRemark.TYPE.ISSUE;
        public bool StopAssemblyOnDictationEnd { get; set; }
        public Action OnAssemblyEnd { get; set; }

        public void AddAssemblyRemark(string message)
        {
            var listOfPreviousRemarks = _assemblyProcessDataEntity.GetStepRemarks(_currentStepIndex).ToList();
            listOfPreviousRemarks.Add(new AssemblyRemark(_currentReportType,message));
            _assemblyProcessDataEntity.SetStepRemarks(_currentStepIndex, listOfPreviousRemarks.ToArray());
            _feedbackDataLoader.SaveData(_assemblyProcessDataEntity);
        }
    }
}