using MixedReality.Toolkit;
using MRTKExtensions.QRCodes;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class SolredoMainManager : MonoBehaviour
{
    [SerializeField] private QRTrackerController _qrTrackerController;
    [SerializeField] private SolredoPlacementManager _placementManager;
    [SerializeField] private SolredoUIManager _UIManager;
    [SerializeField] private ARPlaneManager _ARPlaneManager;
    [SerializeField] private GameObject _moduleOne;
    [SerializeField] private GameObject _moduleTwo;
    [SerializeField] private GameObject _moduleWKNE02;
    [SerializeField] private bool _useModuleWKNE02 = true;

    private int _selectedModuleID = -1;

    void OnEnable()
    {
        _UIManager.OnIntroductionDone = new UnityEvent();
        _UIManager.OnStartFindingPlane = new UnityEvent();
        _UIManager.OnStartFindingPlane.AddListener(StartPlaneDetection);
        _UIManager.OnIntroductionDone.AddListener(() => _placementManager.AllowMiniatureCreation());
        _placementManager.OnModuleSelected = new UnityEvent<int>();
        _placementManager.OnModuleSelected.AddListener(ShowQRCodeDetectionDialog);
        _placementManager.OnModuleSelected.AddListener(AssignChosenModuleValue);
        _placementManager.OnMiniatureInstantiated.AddListener(ParseMiniatureModules);
        _placementManager.OnModulePlaced = new UnityEvent();
        _placementManager.OnModulePlaced.AddListener(StopARPlanesDetection);

        _ARPlaneManager.planesChanged += OnARPlanesChanged;
    }

    private void StopARPlanesDetection()
    {
        _ARPlaneManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        //_ARPlaneManager.enabled = false;
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
            if (s.gameObject.name == "ModuleBox1")
            {
                s.IsRayHovered.OnEntered.AddListener((time) => { OnModuleRayHover(s.gameObject); });
                s.IsRayHovered.OnExited.AddListener((time) => { OnModuleRayExit(s.gameObject); });
                s.IsRaySelected.OnEntered.AddListener((time) => { _placementManager.OnModuleSelected?.Invoke(1); });
            }
            else if (s.gameObject.name == "ModuleBox2")
            {
                s.IsRayHovered.OnEntered.AddListener((time) => { OnModuleRayHover(s.gameObject); });
                s.IsRayHovered.OnExited.AddListener((time) => { OnModuleRayExit(s.gameObject); });
                s.IsRaySelected.OnEntered.AddListener((time) => { _placementManager.OnModuleSelected?.Invoke(2); });
            }
        }
    }

    private void ShowQRCodeDetectionDialog(int moduleID)
    {
        _UIManager.ShowInfoDialog("QR Code", "Scannez le QR Code pour placer le module choisi", null);
        _placementManager.AllowQRDetection(true);
        _qrTrackerController.StartTracking();
    }

    private void ShowPlaneSelectionDialog(int moduleID)
    {
        _UIManager.ShowInfoDialog("Plan de travail", "Sélectionnez le plan de travail", _UIManager.OnStartFindingPlane);
        _selectedModuleID = moduleID;
        _placementManager.ChosenModule = moduleID == 1 ? _moduleOne : _moduleTwo;
    }

    private void AssignChosenModuleValue(int moduleID)
    {
        if (_useModuleWKNE02)
        {
            _placementManager.ChosenModule = _moduleWKNE02;
        }
        else
        {
            _selectedModuleID = moduleID;
            _placementManager.ChosenModule = moduleID == 1 ? _moduleOne : _moduleTwo;
        }
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
}
