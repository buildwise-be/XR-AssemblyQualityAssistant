using System;
using _project.Scripts.Controllers.DTO;
using TMPro;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class RemarkButtonView : MonoBehaviour, IRemarkButtonView
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private GameObject _issueIconGameObject;
        [SerializeField] private GameObject _remarkIconGameObject;

        private TMP_Text _textField;
        private string _text;

        public void SetTitle(string title)
        {
            _title.SetText(title);
        }

        public void SetText(string s, TMP_Text dictationTextField)
        {
            _textField = dictationTextField;
            _text = s;
        }

        public void SetIcon(IRemarksCollection.RemarksType type)
        {
            switch (type)
            {
                case IRemarksCollection.RemarksType.Remark:
                    _issueIconGameObject.SetActive(false);
                    _remarkIconGameObject.SetActive(true);
                    break;
                case IRemarksCollection.RemarksType.Issue:
                    _issueIconGameObject.SetActive(true);
                    _remarkIconGameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public void OnClick()
        {
            _textField.SetText(_text);
        }
        
        public void OnDeleteButtonClick()
        {
            Debug.Log("OnDeleteButtonClick");
        }
    }
}

public class RemarkButtonView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}