using TMPro;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class RemarkButtonView : MonoBehaviour, IRemarkButtonView
    {
        [SerializeField] private TMP_Text _title;
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