using UnityEngine;

namespace Project.Assets.FifteenPuzzle
{
    public class MoveSlimes : MonoBehaviour
    {
        [SerializeField] private GridSystem gridSystem;
        
        private SlimeBehaviour _currentSlime;
        private bool _clickedOnSlimeBefore;
        
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            if (Input.touchCount != 1) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = _camera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (!hit.collider.gameObject.CompareTag("Slime")) return;
                    
                    _currentSlime = hit.collider.gameObject.GetComponent<SlimeBehaviour>();
                    _clickedOnSlimeBefore = true;
                    
                    _startTouchPosition = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!_clickedOnSlimeBefore) return;

                _endTouchPosition = touch.position;
                
                Vector2 touchDelta = _endTouchPosition - _startTouchPosition;
                int targetX;
                int targetY;

                if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
                {
                    targetY = 0;
                    if (touchDelta.x > 0)
                    {
                        targetX = 1;
                    }
                    else if (touchDelta.x < 0)
                    {
                        targetX = -1;
                    }
                    else
                    {
                        targetX = 0;
                    }
                    
                }
                else if (Mathf.Abs(touchDelta.y) > Mathf.Abs(touchDelta.x))
                {
                    targetX = 0;
                    if (touchDelta.y > 0)
                    {
                        targetY = 1;
                    }
                    else if (touchDelta.y < 0)
                    {
                        targetY = -1;
                    }
                    else
                    {
                        targetY = 0;
                    }
                }
                else
                {
                    _clickedOnSlimeBefore = false;
                    _currentSlime = null;
                    
                    return;
                }

                Vector2 targetDir = new Vector2(targetX, targetY);

                gridSystem.TryToMoveSlime(targetDir, _currentSlime);
                
                _clickedOnSlimeBefore = false;
                _currentSlime = null;
            }
        }
    }
}
