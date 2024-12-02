using System.Collections;
using _project.Scripts.Controllers;
using _project.Scripts.Gateways;
using _project.Scripts.UseCases;
using UnityEngine;

namespace _project.Scripts
{
    public class QualityAssistantSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private AssemblyProcessOptions _options;
        
        //[SerializeField] private AppData _appData;
        //[SerializeField] private AssemblyProjectScriptableObject _currentProject;
        public AssemblyProcessController _assemblyProcessController;
        public DictationPanelController _dictationPanelController;
        public AssemblyProcessView _assemblyProcessView;
        [SerializeField] private SolredoMainManager _solredoMainManager;
        private AssemblyStepFeedbackMonitor _monitor;
        [SerializeField] private int _fakeLoaderStepCount = 10;
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            //_monitor = new AssemblyStepFeedbackMonitor(new FakeDataLoader(_fakeLoaderStepCount));
            _monitor = new AssemblyStepFeedbackMonitor(new PersistantDataLoaderGateway());
            _assemblyProcessController.SetMonitor(_monitor);
            _dictationPanelController.SetUseCase(_monitor);
        
            _assemblyProcessView.SetController(_assemblyProcessController);
            _assemblyProcessController.OnAssemblyEnd += EndSession;

        }

        private void EndSession()
        {
            
            _solredoMainManager.EndSession();
        }

        public void StartAssemblyProcess(string assemblyName)
        {
            _monitor.InitMonitoring(assemblyName);
        }

        IEnumerator Start()
        {
            _solredoMainManager.OnAssemblyStartProcessEvent += StartAssemblyProcess;
            yield return new WaitForSeconds(2);
            if (_options._skipAll) _solredoMainManager.StartAssemblyProcess(_options._assemblyProject);
            else _solredoMainManager.StartPlacementProcess(_options._skipHousePlacementPhase, _options._assemblyProject);
                

        }
    }
}