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
            var height = _rectTransform.rect.height;
            if (_list == null) return;
            if(_list.Length == 0) return;
            for (var i = 0; i < _list.Length; i++)
            {
                var localPosition = transform.InverseTransformPoint(_list[i].GetPosition());
                var activate = !(localPosition.y > 0 || localPosition.y <- height);
                _list[i].Activate(activate);
            }
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
        float GetYPosition();
        void Activate(bool activate);
        Vector3 GetPosition();
    }
}