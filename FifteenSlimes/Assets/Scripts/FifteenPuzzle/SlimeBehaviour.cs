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
    }
}
