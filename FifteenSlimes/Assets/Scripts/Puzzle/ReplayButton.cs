using System.Threading.Tasks;
using Project.Assets.Other;
using UnityEngine;
using UnityEngine.EventSystems;
using Project.Assets.Managers;
using UnityEngine.SceneManagement;

namespace Project.Assets.Puzzle
{
    public class ReplayButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
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
            RestartThisPuzzle();
        }

        private async void RestartThisPuzzle()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;
            
            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if (_rectTransform == null) return;
            
            OnAwake();
        }
    }
}