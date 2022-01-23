using System.Collections;
using UnityEngine;

namespace Project.Assets.FifteenPuzzle
{
    [RequireComponent(typeof(BoxCollider))]
    public class SlimeBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject body;
        /// <summary>
        /// Tells on which tile this slime must be to win.
        /// For slimes exist only numbers from 1 up 15.
        /// </summary>
        [Tooltip("Set here a number as on the slime model.")]
        [SerializeField] private int myNumber;
        public int MyNumber => myNumber;
        public Vector2 MyGridCoordinates;
        public bool IsImMoving;

        private const float MoveSpeed = 20f;

        private Coroutine _squashCoroutine;
        private const float SquashSpeed = 20f;
        private readonly Vector3 _normalScale = new Vector3(1f, 1f, 1f);
        private readonly Vector3 _minScaleX = new Vector3(0.8f, 1f, 1f);
        private readonly Vector3 _minScaleY = new Vector3(1f, 0.8f, 1f);
        
        public void Move(Vector3 goalPos, RectTransform.Axis axis)
        {
            StartCoroutine(MoveSlimeModel(goalPos, axis));
            
            if (_squashCoroutine != null) StopCoroutine(_squashCoroutine);
        }
        
        private IEnumerator MoveSlimeModel(Vector3 goalPos, RectTransform.Axis axis)
        {
            while (transform.position != goalPos)
            {
                transform.position = 
                    Vector3.Lerp(transform.position, goalPos, MoveSpeed * Time.deltaTime);
                Stretch(axis);
                
                if ((transform.position - goalPos).magnitude <= 0.05f)
                {
                    transform.position = goalPos;
                    IsImMoving = false;

                    body.transform.localScale = _normalScale;
                    _squashCoroutine = StartCoroutine(Squash(axis));
                }
                
                yield return null;
            }
        }

        private void Stretch(RectTransform.Axis axis)
        {
            if (axis == RectTransform.Axis.Horizontal)
            {
                var currentScale = body.transform.localScale;
                currentScale = new Vector3(currentScale.x * 1.01f, currentScale.y, currentScale.z);
                body.transform.localScale = currentScale;
            }
            else if (axis == RectTransform.Axis.Vertical)
            {
                var currentScale = body.transform.localScale;
                currentScale = new Vector3(currentScale.x, currentScale.y * 1.01f, currentScale.z);
                body.transform.localScale = currentScale; 
            }
        }

        private IEnumerator Squash(RectTransform.Axis axis)
        {
            if (axis == RectTransform.Axis.Horizontal)
            {
                while (body.transform.localScale != _minScaleX)
                {
                    body.transform.localScale = 
                        Vector3.Lerp(body.transform.localScale, _minScaleX, Time.deltaTime * SquashSpeed);

                    if ((body.transform.localScale - _minScaleX).magnitude <= 0.05f)
                    {
                        body.transform.localScale = _minScaleX;
                    }

                    yield return null;
                }
            }
            else if (axis == RectTransform.Axis.Vertical)
            {
                while (body.transform.localScale != _minScaleY)
                {
                    body.transform.localScale = 
                        Vector3.Lerp(body.transform.localScale, _minScaleY, Time.deltaTime * SquashSpeed);

                    if ((body.transform.localScale - _minScaleY).magnitude <= 0.05f)
                    {
                        body.transform.localScale = _minScaleY;
                    }

                    yield return null;
                }
            }
            
            while (body.transform.localScale != _normalScale)
            {
                body.transform.localScale = 
                    Vector3.Lerp(body.transform.localScale, _normalScale, Time.deltaTime * SquashSpeed);

                if ((body.transform.localScale - _normalScale).magnitude <= 0.05f)
                {
                    body.transform.localScale = _normalScale;
                }
                    
                yield return null;
            }
        }
    }
}
