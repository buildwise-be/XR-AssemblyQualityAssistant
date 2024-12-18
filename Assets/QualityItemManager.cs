using UnityEngine;

public class QualityItemManager : MonoBehaviour
{
    public QualityItem[] qualityItems;

    private int _i;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextQualityItem()
    {
        _i++;
        if (_i >= qualityItems.Length) return;
        QualityItem qualityItem = qualityItems[_i];
        qualityItem.gameObject.SetActive(true);
    }
}
