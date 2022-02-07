using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = Project.Assets.Other.Button;

namespace Project.Assets.MainMenu
{
    [RequireComponent(typeof(RawImage))]
    public class ModeButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private SelectMode selectMode;
        [SerializeField] private SelectMode.Mode modeToSwitch;

        protected override RectTransform _rectTransform { get; set; }
        protected override Vector3 _defaultSize { get; set; }
        protected override Vector3 _squashedSize { get; set; }

        private RawImage _buttonImage;

        private readonly Color ChekedColor = new Color(255f, 255f, 255f, 1f);
        private readonly Color UnchekedColor = new Color(255f, 255f, 255f, 0.3f);

        public void OnPointerClick(PointerEventData eventData)
        {
            selectMode.CurrentMode = modeToSwitch;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            foreach (var button in selectMode.ModesButtons)
            {
                button._buttonImage.color = UnchekedColor;
            }
            _buttonImage.color = ChekedColor;
            
            Squash();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Stretch();
        }

        private void Awake()
        {
            _buttonImage = GetComponent<RawImage>();
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null || _buttonImage == null) return;

            if (modeToSwitch != selectMode.CurrentMode) _buttonImage.color = UnchekedColor;

            OnAwake();
        }
    }
}
