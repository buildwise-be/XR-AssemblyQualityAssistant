using System;
using _project.Scripts.Controllers.DTO;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class RemarkButtonView : MonoBehaviour, IRemarkButtonView, IScrollListElement
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private GameObject _issueIconGameObject;
        [SerializeField] private GameObject _remarkIconGameObject;
        [SerializeField] private PressableButton _pressableButton;

        private TMP_Text _textField;
        private string _text;
        private RectTransform _rectTransform;
        private float _yPosition;
        private float _height;

        public void SetTitle(string title)
        {
            _title.SetText(title);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _yPosition = _rectTransform.anchoredPosition.y;
            _height = _rectTransform.sizeDelta.y;
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

        public void SetInteractable(bool value)
        {
           _pressableButton.enabled = value;
        }


        public void OnClick()
        {
            _textField.SetText(_text);
        }
        
        public void OnDeleteButtonClick()
        {
            Debug.Log("OnDeleteButtonClick");
        }

        public void HandleOutOfScrollViewBounds(float rectTransformAnchoredPositionY, float sizeDeltaY)
        {
            if (rectTransformAnchoredPositionY > _yPosition + _height || _yPosition > rectTransformAnchoredPositionY+sizeDeltaY)
            {
                SetInteractable(false);
            }
            else
            {
                SetInteractable(true);
            }
        }

        public float GetYPosition()
        {
            return GetComponent<RectTransform>().anchoredPosition.y;
        }

        public void Activate(bool activate)
        {
            _pressableButton.enabled = activate;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
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