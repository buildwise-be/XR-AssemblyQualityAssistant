using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToogleXRManipulator : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    public Action<bool> OnValueChanged;
    [SerializeField] private TMP_Text _toggleText;

    public void UpdateToggleValue(bool arg0)
    {
        _toggleText.color = arg0 ? Color.red : Color.grey;
        OnValueChanged.Invoke(arg0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleValue()
    {
        _toggle.isOn = !_toggle.isOn;
    }

    public void SetToggleValue(bool _value)
    {
        _toggle.isOn = _value;
        
    }
}
