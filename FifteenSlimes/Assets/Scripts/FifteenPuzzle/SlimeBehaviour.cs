using UnityEngine;

namespace Project.Assets.FifteenPuzzle
{
    [RequireComponent(typeof(BoxCollider))]
    public class SlimeBehaviour : MonoBehaviour
    {
        [SerializeField] private GridTile.TileNumber myNumber;
        public GridTile.TileNumber MyNumber => myNumber;
        
        public Vector2 MyGridCoordinates { get; set; }
    }
}
