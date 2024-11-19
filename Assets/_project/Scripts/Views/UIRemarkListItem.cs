using TMPro;
using UnityEngine;

namespace _project.Scripts.Views
{
    public class UIRemarkListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public void SetText(string variable)
        {
            _text.SetText(variable);
        }
    }
}