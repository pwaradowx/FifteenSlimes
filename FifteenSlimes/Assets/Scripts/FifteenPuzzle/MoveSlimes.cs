using UnityEngine;

namespace Project.Assets.FifteenPuzzle
{
    public class MoveSlimes : MonoBehaviour
    {
        [SerializeField] private GridSystem gridSystem;

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
                    hit.collider.gameObject.TryGetComponent(out SlimeBehaviour slimeBehaviour);
                    
                    if (slimeBehaviour == null) return;
                    if (slimeBehaviour.IsImMoving) return;
                    
                    gridSystem.TryToMoveSlime(slimeBehaviour);
                }
            }
        }
    }
}
