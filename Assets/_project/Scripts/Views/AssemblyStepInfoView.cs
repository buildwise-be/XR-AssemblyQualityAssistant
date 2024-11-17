using _project.Scripts.Controllers;
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
    }
}