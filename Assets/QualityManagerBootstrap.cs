using System.Collections;
using _project.Scripts.Controllers;
using _project.Scripts.Controllers.QualityProject;
using _project.Scripts.Gateways;
using _project.Scripts.UseCases;
using UnityEngine;

public class QualityManagerBootstrap : MonoBehaviour
{
    //[SerializeField] private AssemblyProcessOptions _options;
        
    //[SerializeField] private AppData _appData;
    //[SerializeField] private AssemblyProjectScriptableObject _currentProject;
    public AssemblyProcessController _assemblyProcessController;
    public DictationPanelController _dictationPanelController;
    public AssemblyProcessView _assemblyProcessView;
    [SerializeField]private MainManager _mainManager;
    private AssemblyStepFeedbackMonitor _monitor;
    [SerializeField] private QualityProjectScriptableObject _qualityProject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _monitor = new AssemblyStepFeedbackMonitor(new PersistantDataLoaderGateway());
        _assemblyProcessController.SetMonitor(_monitor);
        _dictationPanelController.SetUseCase(_monitor);
        
        _assemblyProcessView.SetController(_assemblyProcessController);
        _assemblyProcessController.OnAssemblyEnd += EndSession;

    }

    private void EndSession()
    {
            
        _mainManager.EndSession();
    }

    public void StartAssemblyProcess()
    {
        _monitor.InitMonitoring(_qualityProject.m_guid);
    }

    IEnumerator Start()
    {
        _mainManager.OnAssemblyStartProcessEvent += StartAssemblyProcess;
        yield return new WaitForSeconds(2);
        _mainManager.StartProcess(_qualityProject);
        
    }
}
