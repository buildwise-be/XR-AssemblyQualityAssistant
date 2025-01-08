using System;
using _project.Scripts;
using MixedReality.Toolkit;
using MRTKExtensions.QRCodes;
using _project.Scripts.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class SolredoMainManager : MonoBehaviour
{
    [SerializeField] private QRTrackerController _qrTrackerController;
    [SerializeField] private SolredoPlacementManager _placementManager;
    [SerializeField] private SolredoUIManager _UIManager;
    [SerializeField] private ARPlaneManager _ARPlaneManager;
    //[SerializeField] private GameObject _moduleOne;
    //[SerializeField] private GameObject _moduleTwo;
    [SerializeField] private GameObject _moduleWKNE02;
    [SerializeField] private bool _useModuleWKNE02 = true;
    [SerializeField] [HideInInspector] private AppData _appData; // Pas touche
    public event Action<string> OnAssemblyStartProcessEvent;
    private void Awake()
    {
        
    }

    public void StartPlacementProcess(bool skipHousePlacement, AssemblyProjectScriptableObject project)
    {
        if (skipHousePlacement)
        {
            if (project)
            {
                InitializeQRDetection(project);
                return;
            }
            Debug.LogWarning("No Project found -> Beginning House Placement phase");
        }
        InitializePlacementManager();
    }

    private void InitializePlacementManager()
    {
        _placementManager.OnMiniatureInstantiated.AddListener(ParseMiniatureModules);
        _placementManager.OnModulePlaced = new UnityEvent();
        _placementManager.OnModulePlaced.AddListener(StopARPlanesDetection);

        _ARPlaneManager.planesChanged += OnARPlanesChanged;
        _placementManager.Subscribe();

        _UIManager.ShowStartHousePhaseDialog();

    }

    void Start()
    {
        _UIManager.OnIntroductionDone = new UnityEvent();
        _UIManager.OnStartFindingPlane = new UnityEvent();
        _UIManager.OnStartFindingPlane.AddListener(StartPlaneDetection);
        _UIManager.OnIntroductionDone.AddListener(() => _placementManager.AllowMiniatureCreation());
        /*_placementManager.OnModuleSelected = new UnityEvent<int>();
        _placementManager.OnModuleSelected.AddListener(ShowQRCodeDetectionDialog);
        _placementManager.OnModuleSelected.AddListener(AssignChosenModuleValue);*/

        _qrTrackerController.PositionSet += SpawnModule;
    }

    private void SpawnModule(object sender, Pose e)
    {
        _placementManager.PlaceModuleAtPosePosition(sender,pose:e);
        OnAssemblyStartProcessEvent?.Invoke(_appData.project.m_guid);
    }

    private void StopARPlanesDetection()
    {
        _ARPlaneManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        foreach (ARPlane p in _ARPlaneManager.trackables)
        {
            p.gameObject.SetActive(false);
        }
    }

    private void OnARPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane p in args.added)
        {
            //Debug.Log("ARPlane added: " + p.trackableId);
            _placementManager.AddShowModulesOnRayEndEvent(p);
        }
    }

    private void ParseMiniatureModules(GameObject miniature)
    {
        foreach (StatefulInteractable s in miniature.GetComponentsInChildren<StatefulInteractable>(true))
        {
            var moduleSelector = s.gameObject.GetComponent<IModuleSelectorController>();
            if (moduleSelector == null) continue;
            s.IsRayHovered.OnEntered.AddListener((time) => { moduleSelector.OnModuleRayHover(s.gameObject); });
            s.IsRayHovered.OnExited.AddListener((time) => { moduleSelector.OnModuleRayExit(s.gameObject); });
            s.IsRaySelected.OnEntered.AddListener((time) =>
            {
                var assemblyProcessData = moduleSelector.GetData();
                InitializeQRDetection(assemblyProcessData);
                moduleSelector.OnModuleSelection();
                
            });
        }
    }

    private void InitializeQRDetection(AssemblyProjectScriptableObject assemblyProcessData)
    {
        _appData.project = assemblyProcessData;
        
        AssignChosenModuleValue(assemblyProcessData.m_modulePrefab);
        ShowQrCodeDetectionDialog();
    }

    private void ShowQrCodeDetectionDialog()
    {
        _UIManager.ShowQRScanInfoDialog();
        _placementManager.AllowQRDetection(true);
        _qrTrackerController.StartTracking();
    }

    /*private void ShowPlaneSelectionDialog(int moduleID)
    {
        _UIManager.ShowInfoDialog("Plan de travail", "S�lectionnez le plan de travail", _UIManager.OnStartFindingPlane);
        _selectedModuleID = moduleID;
        _placementManager.ChosenModule = moduleID == 1 ? _moduleOne : _moduleTwo;
    }*/

    private void AssignChosenModuleValue(GameObject module)
    {
        _placementManager.ChosenModule = _useModuleWKNE02 ? _moduleWKNE02 : module;
    }
    
    

    private void StartPlaneDetection()
    {
        _ARPlaneManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
    }

    void OnModuleRayHover(GameObject go)
    {
        Animator anim = go.GetComponent<Animator>();
        anim.enabled = true;
    }

    void OnModuleRayExit(GameObject go)
    {
        Animator anim = go.GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);
        anim.enabled = false;
    }

    public void DebugLog(string message)
    {
        Debug.Log(message);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void StartAssemblyProcess(AssemblyProjectScriptableObject optionsAssemblyProject)
    {
        _appData.project = optionsAssemblyProject;
        OnAssemblyStartProcessEvent?.Invoke(_appData.project.m_guid);
    }

    public void EndSession()
    {
        _UIManager.ShowFinalizeDialog();
    }
}