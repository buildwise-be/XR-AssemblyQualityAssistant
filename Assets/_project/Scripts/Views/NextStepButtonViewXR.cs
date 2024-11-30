using TMPro;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class NextStepButtonViewXR : NexStepButtonBaseView
    {
        [SerializeField] private TMP_Text _textMeshProUGUI;
        public override void SetText(string text)
        {
            _textMeshProUGUI.SetText(text);
        }

        public override void SetEndOfProcessText()
        {
            SetText("Validate Process");
        }
    }
}