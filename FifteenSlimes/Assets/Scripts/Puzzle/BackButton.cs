using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Assets.Managers;
using Project.Assets.Other;
using UnityEngine.EventSystems;

namespace Project.Assets.Puzzle
{
    [RequireComponent(typeof(RectTransform))]
    public class BackButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
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
            GoToMainMenu();
        }
        
        private async void GoToMainMenu()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;
            
            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);

            SceneManager.LoadScene(0);
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;

            OnAwake();
        }
    }
}
