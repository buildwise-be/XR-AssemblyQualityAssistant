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
        stringBuilder.AppendLine($"<size=8 > Etape {index} </size> \n<size=6><alpha=#88>{indicationText}</alpha></size>)");
        _text.SetText(stringBuilder.ToString());
    }
}
