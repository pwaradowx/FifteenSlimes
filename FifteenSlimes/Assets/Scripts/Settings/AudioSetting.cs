using UnityEngine;
using TMPro;
using Project.Assets.Other;
using UnityEngine.EventSystems;
using Project.Assets.Managers;

namespace Project.Assets.Settings
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AudioSetting : Button, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI stateText;

        private bool _isAudioOn = true;
        private const string AudioStateKey = "AudioState";
        
        protected override RectTransform _rectTransform { get; set; }
        protected override Vector3 _defaultSize { get; set; }
        protected override Vector3 _squashedSize { get; set; }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            Stretch();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Squash();
            
            AudioManager.Instance.PlayClickSound();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _isAudioOn = !_isAudioOn;
            AudioManager.Instance.IsAudioOn = _isAudioOn;

            stateText.text = _isAudioOn ? "On" : "Off";
            
            PlayerPrefs.SetInt(AudioStateKey, _isAudioOn ? 1 : -1);
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            if (_rectTransform == null) return;
            
            OnAwake();

            if (PlayerPrefs.GetInt(AudioStateKey) == 0)
            {
                _isAudioOn = true;
            }
            else
            {
                _isAudioOn = PlayerPrefs.GetInt(AudioStateKey) == 1;
            }

            AudioManager.Instance.IsAudioOn = _isAudioOn;

            stateText.text = _isAudioOn ? "On" : "Off";
        }
    }
}
