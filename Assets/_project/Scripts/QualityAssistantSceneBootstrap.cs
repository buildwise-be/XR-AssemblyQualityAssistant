using _project.Scripts.Controllers;
using _project.Scripts.Gateways;
using _project.Scripts.UseCases;
using UnityEngine;

namespace _project.Scripts
{
    public class QualityAssistantSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private AppData _appData;
        //[SerializeField] private AssemblyProjectScriptableObject _currentProject;
        private HandMenuActions _handMenuActions;
        public AssemblyProcessController _assemblyProcessController;
        public DictationPanelController _dictationPanelController;
        public AssemblyProcessView _assemblyProcessView;

        private AssemblyStepFeedbackMonitor _monitor;
        [SerializeField] private int _fakeLoaderStepCount = 10;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            //_monitor = new AssemblyStepFeedbackMonitor(new FakeDataLoader(_fakeLoaderStepCount));
            _monitor = new AssemblyStepFeedbackMonitor(new PersistantDataLoaderGateway());
            _handMenuActions = FindFirstObjectByType<HandMenuActions>();
            _assemblyProcessController.SetMonitor(_monitor);
            _dictationPanelController.SetUseCase(_monitor);
        
            _assemblyProcessView.SetController(_assemblyProcessController);
        
        }

        public void StartAssemblyProcess()
        {
            _monitor.InitMonitoring(_appData.project.m_guid);
        }

        private void Start()
        {
            _monitor.InitMonitoring(_appData.project.m_guid);
        }
    }
}