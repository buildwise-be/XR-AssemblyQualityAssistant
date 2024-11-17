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
    }
}