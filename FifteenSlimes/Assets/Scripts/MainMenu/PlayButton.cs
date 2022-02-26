using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Assets.Managers;
using Project.Assets.Other;
using UnityEngine.EventSystems;

namespace Project.Assets.MainMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private SelectMode selectMode;

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
            StartPuzzle();
        }

        private async void StartPuzzle()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;

            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);
            
            SceneManager.LoadScene((int) selectMode.CurrentMode);
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;
            
            OnAwake();
        }
    }
}
