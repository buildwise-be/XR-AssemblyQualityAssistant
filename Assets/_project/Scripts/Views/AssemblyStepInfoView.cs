using _project.Scripts.Controllers;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Views
{
    public class AssemblyStepInfoView : MonoBehaviour, IStepInfoView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _stepIndexText;
        
        [SerializeField] private StepInfoDetail _stepDuration;
        [SerializeField] private StepInfoDetail _stepRemarks;
        [SerializeField] private StepInfoDetail _stepIssues;
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _foldingContent;
        [SerializeField] private float _offset=15;
        private bool _isUnFolded;
        [SerializeField] private float _startHeight=60;

        public void SetStepData(AssemblyProcessDataDto stepData)
        {
            SetNumberOfIssues(stepData.m_dataNbOfIssues);
            SetNumberOfRemarks(stepData.DataNbOfRemarks);
        }

        public void SetIllustation(int i, Sprite illustrationSprite)
        {
            _image.sprite = illustrationSprite;
            _stepIndexText.text = i.ToString();
        }

        public void SetTitle(int assemblyStepTitle, string getAssemblyStepTitle)
        {
            _title.text = getAssemblyStepTitle;
        }

        public void SetDuration(string durationInMinutes)
        {
            _stepDuration.SetText(durationInMinutes);
        }

        public void SetNumberOfIssues(int numberOfIssues)
        {
            _stepIssues.SetText(numberOfIssues.ToString());
        }

        public void SetNumberOfRemarks(int numberOfRemarks)
        {
            _stepRemarks.SetText(numberOfRemarks.ToString());
        }

        public void ToggleFolding()
        {
            if (_isUnFolded)
            {
                FoldContent();
                return;
            }
            UnFoldContent();
        }

        public void UnFoldContent()
        {
            _isUnFolded = true;
            var sizeDelta = _content.sizeDelta;
            sizeDelta.y = _startHeight+_foldingContent.sizeDelta.y+_offset;
            Tween.UISizeDelta(_content, sizeDelta, 1, Ease.Default);
        }
        public void FoldContent()
        {
            _isUnFolded = false;
            var sizeDelta = _content.sizeDelta;
            sizeDelta.y = _startHeight;
            Tween.UISizeDelta(_content, sizeDelta, .5f, Ease.Default);
        }
    }
}