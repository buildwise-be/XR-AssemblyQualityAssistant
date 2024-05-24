using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class HighlightedObject : MonoBehaviour
{
    [SerializeField] private float _highlightFrequency = 2.0f;
    public HighlightStates HighlightState 
    {
        get 
        {
            return _highlightState;
        }
        set
        {
            if (value == HighlightStates.Highlighted)
            {
                HighlightObject();
            }
            else
            {
                UnhighlightObject();
            }
            _highlightState = value;
        }
    }
    private HighlightStates _highlightState = HighlightStates.NotHighlighted;

    private Material _material;
    private Coroutine _highlightMaterialCoroutine;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void HighlightObject()
    {
        _highlightMaterialCoroutine = StartCoroutine(PulseMaterialCoroutine());
    }

    private void UnhighlightObject()
    {
        if (_highlightMaterialCoroutine != null)
        {
            StopCoroutine(_highlightMaterialCoroutine);
        }
        _material.SetFloat("_InnerGlowPower", 6.0f);
    }

    private IEnumerator PulseMaterialCoroutine()
    {
        if (_material.HasProperty("_InnerGlowPower"))
        {
            _material.GetFloat("_InnerGlowPower");
        }
        else
        {
            Debug.LogError("Material does not have _InnerGlowPower property");
            yield break;
        }

        while (true)
        {
            float glowPower = _material.GetFloat("_InnerGlowPower");
            glowPower = (Mathf.PingPong(Time.time, _highlightFrequency) * 4.0f) + 2.0f;
            _material.SetFloat("_InnerGlowPower", glowPower);
            yield return null;
        }
    }
}
