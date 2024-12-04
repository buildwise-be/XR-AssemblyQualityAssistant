using System;
using _project.Scripts.Controllers.QualityProject;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private QualityManager _qualityManager;
    void OnEnable()
    {
        _placementManager.OnConcreteSlabInstantiated = new UnityEvent<GameObject>();
        _placementManager?.OnConcreteSlabInstantiated.AddListener(OnSlabInstantiated);
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
    
    public void StartProcess(QualityProjectScriptableObject qualityProjectScriptableObject)
    {
        
        _placementManager.Prefab = qualityProjectScriptableObject.Prefab;
    }

    public void BeginQualityMonitoring()
    {
        OnAssemblyStartProcessEvent?.Invoke();
    }
}
