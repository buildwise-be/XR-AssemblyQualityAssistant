using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Views
{
    internal class ScrollViewContentActivator : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private IScrollListElement[]_list;
        private float _lastYPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_list == null) return;
            if(_list.Length == 0) return;
            if (_rectTransform.anchoredPosition.y - _lastYPosition < 0.01f) return;
            {
                _lastYPosition = _rectTransform.anchoredPosition.y;
                for (var i = 0; i < _list.Length; i++)
                {
                    _list[i].HandleOutOfScrollViewBounds(_rectTransform.anchoredPosition.y, _rectTransform.sizeDelta.y);
                }
            };
        }

        public void SetContentList(GameObject[] contentList)
        {
            _list = new IScrollListElement[contentList.Length];
            for (var i = 0; i < contentList.Length; i++) _list[i] = contentList[i].GetComponent<IScrollListElement>();
           
        }
    }

    public interface IScrollListElement
    {
        void HandleOutOfScrollViewBounds(float rectTransformAnchoredPositionY, float sizeDeltaY);
    }
}