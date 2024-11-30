using System.Drawing;
using System.Text;
using TMPro;
using UnityEngine;

public class AssemblyStepIndication : MonoBehaviour, IAssemblyInsctructionView
{
    [SerializeField]private TMP_Text _text;

    public void SetText(int index, string indicationText)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"<size=8 > Etape {index+1} </size> \n<size=6>{indicationText}</size>");
        _text.SetText(stringBuilder.ToString());
    }
}
