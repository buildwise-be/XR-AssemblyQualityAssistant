using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    private GameObject _inspectedObject;
    private int _inspectionItemIndex = -1;
    private QualityItem _currentInspectionItem;
    private List<QualityItem> _qualityItems;

    [SerializeField] private GameObject _chevronIndicator;

    public void SetInspectedObject(GameObject go)
    {
        _inspectedObject = go;
        _qualityItems = new List<QualityItem>(_inspectedObject.GetComponentsInChildren<QualityItem>());
    }

    /*
    public void StartStopInspection()
    {
        if (_inspectedObject == null)
        {
            return;
        }
        foreach (HighlightedObject obj in _inspectedObject.GetComponentsInChildren<HighlightedObject>())
        {
            if (obj.HighlightState == HighlightedObject.HighlightStates.Highlighted)
            {
                obj.HighlightState = HighlightedObject.HighlightStates.NotHighlighted;
            }
            else
            {
                obj.HighlightState = HighlightedObject.HighlightStates.Highlighted;
            }
        }
    }
    */

    public void HighlightCurrentInspectedItem()
    {
        _currentInspectionItem.GetComponent<HighlightedObject>().HighlightState = HighlightedObject.HighlightStates.Highlighted;
        _chevronIndicator.SetActive(true);
        _chevronIndicator.GetComponent<DirectionalIndicator>().DirectionalTarget = _currentInspectionItem.transform;
    }

    public void UnHighlightCurrentInspectedItem()
    {
        _chevronIndicator.SetActive(false);
        if (_currentInspectionItem != null)
        {
            _currentInspectionItem.GetComponent<HighlightedObject>().HighlightState = HighlightedObject.HighlightStates.NotHighlighted;
        }
    }

    public QualityItem GetNextInspectionElement()
    {
        _inspectionItemIndex += 1;
        QualityItem item = null;
        foreach (QualityItem i in _qualityItems)
        {
            if (i.OrderIndex == _inspectionItemIndex)
            {
                _currentInspectionItem = i;
                return i;
            }
        }

        _currentInspectionItem = null;
        _inspectionItemIndex = -1;
        return item;
    }

    public void ResetQualityControl()
    {
        _currentInspectionItem = null;
        _inspectionItemIndex = -1;
        foreach (HighlightedObject obj in _inspectedObject.GetComponentsInChildren<HighlightedObject>())
        {
            obj.HighlightState = HighlightedObject.HighlightStates.NotHighlighted;
        }
    }
}
