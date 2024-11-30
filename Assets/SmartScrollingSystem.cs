using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SmartScrollingSystem : MonoBehaviour
{
    [SerializeField] private int _itemHeight =25;
    [SerializeField] private int _spacing =5;
    [SerializeField] private int _offset =30;
     [FormerlySerializedAs("_scrollRect")] [SerializeField] private RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_rectTransform.anchoredPosition.y<0) _rectTransform.anchoredPosition = new Vector2(0,(_itemHeight+_spacing));
        if(_rectTransform.anchoredPosition.y>2*(_itemHeight+_spacing)) _rectTransform.anchoredPosition = new Vector2(0,_itemHeight+_spacing);
    }
}
