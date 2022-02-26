using System.IO;
using Project.Assets.Managers;
using Project.Assets.Other;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Assets.Settings
{
    public class ResetTimeButton : Button, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
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
            string pathToTimeData = Application.persistentDataPath + "/StopwatchData.txt";

            File.Delete(pathToTimeData);

            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;
            
            OnAwake();

            string pathToTimeData = Application.persistentDataPath + "/StopwatchData.txt";

            if (!File.Exists(pathToTimeData)) gameObject.SetActive(false);
        }
    }
}
