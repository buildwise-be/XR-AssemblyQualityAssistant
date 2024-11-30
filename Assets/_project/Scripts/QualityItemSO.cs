using UnityEngine;

[CreateAssetMenu(menuName="BuildWize/Quality Item", fileName = "New Quality Item")]
public class QualityItemSO : ScriptableObject
{
    public string Instructions;
    public int OrderIndex;
}