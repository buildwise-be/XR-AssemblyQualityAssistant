using TMPro;
using UnityEngine;

public class StepInfoDetail : MonoBehaviour
{
    [SerializeField] TMP_Text _detailText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string toString)
    {
        _detailText.text = toString;
    }
}
