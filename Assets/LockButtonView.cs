using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class LockButtonView : MonoBehaviour
{
    [SerializeField] private LayoutElement _tuneContainer;

    private bool _isLocked;
    private PlacementManager _placementManager;
    [SerializeField] private float _deployedHeight = 65;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _placementManager = FindObjectOfType<PlacementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTuneButtonDisplay()
    {
        _placementManager.LockSlabPositionOnOff();
        if (_placementManager.IsSlabLocked)
        {
            Tween.UIPreferredHeight(_tuneContainer,0,0.8f);
        }
        else
        {
            Tween.UIPreferredHeight(_tuneContainer,_deployedHeight,0.8f);
        }
    }
}
