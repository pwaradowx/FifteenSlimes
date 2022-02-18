using Project.Assets.Managers;
using UnityEngine;

namespace Project.Assets.Puzzle
{
    public class TouchSlime : MonoBehaviour
    {
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private Stopwatch stopwatch;

        private Camera _camera;

        private bool _firstTouchWasYet;
        private bool _allowToTouch = true;

        private void Start()
        {
            _camera = Camera.main;

            _firstTouchWasYet = false;

            EventManager.Instance.PlayerSolvedPuzzleEvent += () => _allowToTouch = false;
        }
        
        private void Update()
        {
            if (Input.touchCount != 1) return;

            if (!_allowToTouch) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = _camera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.collider.gameObject.TryGetComponent(out SlimeBehaviour slimeBehaviour);
                    
                    if (slimeBehaviour == null) return;

                    if (!_firstTouchWasYet)
                    {
                        stopwatch.StartStopwatch();
                        _firstTouchWasYet = true;
                    }

                    if (slimeBehaviour.IsImMoving) return;
                    
                    gridSystem.TryToMoveSlime(slimeBehaviour);
                }
            }
        }
    }
}
