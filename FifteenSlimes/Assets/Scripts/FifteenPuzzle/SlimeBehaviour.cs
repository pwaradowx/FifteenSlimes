using System.Collections;
using UnityEngine;

namespace Project.Assets.FifteenPuzzle
{
    [RequireComponent(typeof(BoxCollider))]
    public class SlimeBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Tells on which tile this slime must be to win.
        /// For slimes exist only numbers from 1 up 15.
        /// </summary>
        [Tooltip("Set here a number as on the slime model.")]
        [SerializeField] private int myNumber;
        public int MyNumber => myNumber;
        public Vector2 MyGridCoordinates;
        public bool IsImMoving;

        private const float Speed = 20f;
        
        public void Move(Vector3 goalPos)
        {
            StartCoroutine(MoveSlimeModel(goalPos));
        }
        
        private IEnumerator MoveSlimeModel(Vector3 goalPos)
        {
            while (transform.position != goalPos)
            {
                transform.position = 
                    Vector3.Lerp(transform.position, goalPos, Speed * Time.deltaTime);
                
                if ((transform.position - goalPos).magnitude <= 0.05f)
                {
                    transform.position = goalPos;
                    IsImMoving = false;
                }
                
                yield return null;
            }
        }
    }
}
