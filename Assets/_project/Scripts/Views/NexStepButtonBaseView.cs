using UnityEngine;
using UnityEngine.Serialization;

namespace _project.Scripts.Views
{
    public abstract class NexStepButtonBaseView : MonoBehaviour, INextStepButtonView
    {
        [SerializeField] protected string _defaultText;
        public abstract void SetText(string text);

        public abstract void SetEndOfProcessText();

        public void SetDefaultText()
        {
            SetText(_defaultText);
        }
    }
}