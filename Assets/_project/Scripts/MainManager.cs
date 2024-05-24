using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private QualityManager _qualityManager;
    void OnEnable()
    {
        _placementManager.OnConcreteSlabInstantiated = new UnityEvent<GameObject>();
        _placementManager?.OnConcreteSlabInstantiated.AddListener(_qualityManager.SetInspectedObject);
    }

    void OnDisable()
    {
        _placementManager?.OnConcreteSlabInstantiated.RemoveListener(_qualityManager.SetInspectedObject);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
