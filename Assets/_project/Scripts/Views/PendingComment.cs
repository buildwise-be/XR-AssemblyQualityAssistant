using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

internal class PendingComment : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private int _index;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private RectTransform _textField;
    private RectTransform _inputFieldRectTransform;
    [SerializeField] private float _offset=10;
    public Action<int> OnDeleteButtonEventClick { get; set; }


    private void Awake()
    {
        _inputFieldRectTransform = _inputField.GetComponent<RectTransform>();
    }

    public void SetText(string arg0)
    {
        _inputField.text = arg0;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void OnDeleteButtonClick()
    {
        OnDeleteButtonEventClick?.Invoke(_index);
    }

    private void Update()
    {
        var sizeDelta = _inputFieldRectTransform.sizeDelta;
        sizeDelta.y = _textField.sizeDelta.y+_offset;
        _inputFieldRectTransform.sizeDelta = sizeDelta;
    }
}