using UnityEngine;

namespace Project.Assets.Other
{
    public abstract class Button : MonoBehaviour
    {
        protected abstract RectTransform _rectTransform { get; set; }
        protected abstract Vector3 _defaultSize { get; set; }
        protected abstract Vector3 _squashedSize { get; set; }
        
        protected void OnAwake()
        {
            _defaultSize = _rectTransform.localScale;
            _squashedSize = _rectTransform.localScale * 0.8f;
        }

        protected void Stretch()
        {
            _rectTransform.localScale = _defaultSize;
        }

        protected void Squash()
        {
            _rectTransform.localScale = _squashedSize;
        }
    }
}
