using System;
using _project.Scripts.Controllers.QualityProject;
using MRTKExtensions.QRCodes;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private QualityManager _qualityManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private QRTrackerController _QrDetector;

    void OnEnable()
    {
        _placementManager.OnConcreteSlabInstantiated = new UnityEvent<GameObject>();
        _placementManager?.OnConcreteSlabInstantiated.AddListener(OnSlabInstantiated);
        
    }

    private void OnPositionFound(object sender, Pose e)
    {
        _placementManager.PlaceConcreteSlab(e.position);
    }

    private void OnSlabInstantiated(GameObject arg0)
    {
        
        _qualityManager.SetInspectedObject(arg0);
    }

    void OnDisable()
    {
        _placementManager?.OnConcreteSlabInstantiated.RemoveListener(_qualityManager.SetInspectedObject);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void EndSession()
    {

    }

    public event Action OnAssemblyStartProcessEvent;
    
    public async void StartProcess(QualityProjectScriptableObject qualityProjectScriptableObject)
    {
        try
        {
            _placementManager.Prefab = qualityProjectScriptableObject.Prefab;
            var result = await _uiManager.DisplayStartProcessMessage();

            switch (result)
            {
                case 1: // QRCode
                    _uiManager.ShowQRIntroDialog();
                    _QrDetector.StartTracking();
                    _QrDetector.PositionSet += OnPositionFound;
                    
                    break;
                case 2: // Manual
                    _uiManager.ShowManualIntroDialog();
                    _placementManager.Initialize();
                    _placementManager.OnConcreteSlabInstantiated.AddListener(OnSlabInstantiated);
                    break;
            }
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    public void BeginQualityMonitoring()
    {
        OnAssemblyStartProcessEvent?.Invoke();
    }
}
