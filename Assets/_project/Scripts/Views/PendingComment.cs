using System;
using TMPro;
using UnityEngine;

internal class PendingComment : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private int _index;
    
    public Action<int> OnDeleteButtonEventClick { get; set; }

    public void SetText(string arg0)
    {
        _text.SetText(arg0);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void OnDeleteButtonClick()
    {
        OnDeleteButtonEventClick?.Invoke(_index);
    }
}