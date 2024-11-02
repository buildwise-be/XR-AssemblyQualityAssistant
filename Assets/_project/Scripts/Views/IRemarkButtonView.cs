using TMPro;

namespace _project.Scripts.Views
{
    internal interface IRemarkButtonView
    {
        void SetTitle(string title);
        void SetText(string s, TMP_Text dictationTextField);
    }
}