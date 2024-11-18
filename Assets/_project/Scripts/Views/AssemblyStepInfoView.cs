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

        public void SetStepData(AssemblyProcessDataDto stepData)
        {
            
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

        public void UnFoldContent()
        {
            var sizeDelta = _content.sizeDelta;
            sizeDelta.y = 250;
            Tween.UISizeDelta(_content, sizeDelta, 1, Ease.Default);
        }
        public void FoldContent()
        {
            var sizeDelta = _content.sizeDelta;
            sizeDelta.y = 50;
            Tween.UISizeDelta(_content, sizeDelta, 1, Ease.Default);
        }
    }
}