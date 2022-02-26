using Project.Assets.Other;
using UnityEngine;
using UnityEngine.EventSystems;
using Project.Assets.Managers;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Project.Assets.MainMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class SettingsButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        protected override RectTransform _rectTransform { get; set; }
        protected override Vector3 _defaultSize { get; set; }
        protected override Vector3 _squashedSize { get; set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Squash();
            
            AudioManager.Instance.PlayClickSound();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Stretch();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GoToSettingsMenu();
        }

        private async void GoToSettingsMenu()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;

            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);

            SceneManager.LoadScene(1);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;
            
            OnAwake();
        }
    }
}