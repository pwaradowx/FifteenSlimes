using System.Collections.Generic;
using Project.Assets.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Assets.Puzzle
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private int side;
        [SerializeField] private float tileOffset;

        [SerializeField] private GameObject[] slimesPrefabs;

        [Tooltip("Add here a gameobject called Dynamic under the World gameobject to keep scene clean.")]
        [SerializeField] private GameObject slimesParent;

        [SerializeField] private bool createCheatGrid;

        private readonly List<int> _slimesIndexes = new List<int>();
        private SlimeBehaviour[,] _slimes;
        private GridTile[,] _gridTiles;
        
        private Vector3 _slimeTargetPosition;
        private int _freeX;
        private int _freeY;

        public void TryToMoveSlime(SlimeBehaviour slime)
        {
            int currentX = (int) slime.MyGridCoordinates.x;
            int currentY = (int) slime.MyGridCoordinates.y;

            int deltaX = Mathf.Abs(currentX - _freeX);
            int deltaY = Mathf.Abs(currentY - _freeY);

            if (deltaY == 0 && deltaX > 0)
            {
                int dir = currentX > _freeX ? -1 : 1;

                if (dir == 1)
                {
                    for (int i = _freeX - 1; i >= currentX; i--)
                    {
                        _slimes[i + dir, currentY] = _slimes[i, currentY];
                        _slimes[i + dir, currentY].MyGridCoordinates = new Vector2(i + dir, currentY);
                        
                        _slimeTargetPosition = new Vector3((i + dir) * tileOffset, currentY * tileOffset, 0f);
                        _slimes[i + dir, currentY].Move(_slimeTargetPosition, RectTransform.Axis.Horizontal);
                    }
                }
                else
                {
                    for (int i = _freeX + 1; i <= currentX; i++)
                    {
                        _slimes[i + dir, currentY] = _slimes[i, currentY];
                        _slimes[i + dir, currentY].MyGridCoordinates = new Vector2(i + dir, currentY);
                        
                        _slimeTargetPosition = new Vector3((i + dir) * tileOffset, currentY * tileOffset, 0f);
                        _slimes[i + dir, currentY].Move(_slimeTargetPosition, RectTransform.Axis.Horizontal);
                    }
                }

                _slimes[currentX, currentY] = null;
                _freeX = currentX;
                
                CheckVictory();
            }
            else if (deltaX == 0 && deltaY > 0)
            {
                int dir = currentY > _freeY ? -1 : 1;

                if (dir == 1)
                {
                    for (int i = _freeY - 1; i >= currentY; i--)
                    {
                        _slimes[currentX, i + dir] = _slimes[currentX, i];
                        _slimes[currentX, i + dir].MyGridCoordinates = new Vector2(currentX, i + dir);
                        
                        _slimeTargetPosition = new Vector3(currentX * tileOffset, (i + dir) * tileOffset, 0f);
                        _slimes[currentX, i + dir].Move(_slimeTargetPosition, RectTransform.Axis.Vertical);
                    }
                }
                else
                {
                    for (int i = _freeY + 1; i <= currentY; i++)
                    {
                        _slimes[currentX, i + dir] = _slimes[currentX, i];
                        _slimes[currentX, i + dir].MyGridCoordinates = new Vector2(currentX, i + dir);
                        
                        _slimeTargetPosition = new Vector3(currentX * tileOffset, (i + dir) * tileOffset, 0f);
                        _slimes[currentX, i + dir].Move(_slimeTargetPosition, RectTransform.Axis.Vertical);
                    }
                }
                
                _slimes[currentX, currentY] = null;
                _freeY = currentY;
                
                CheckVictory();
            }
            else
            {
                print("Can not move slimes!");
            }
        }

        private void CheckVictory()
        {
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    if (_slimes[x, y] != null)
                    {
                        if (_gridTiles[x, y].MyNumber != _slimes[x, y].MyNumber)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (x != side - 1 || y != 0)
                        {
                            return;
                        }
                    }
                }
            }
            
            EventManager.Instance.OnPlayerSolvedPuzzle();
            print("You win!");
        }

        private void Awake()
        {
            for (int i = 0; i < (side * side) - 1; i++) 
            {
                _slimesIndexes.Add(i);
            }

            CreateGrid(side); 
        }

        private void CreateGrid(int side)
        {
            _slimes = new SlimeBehaviour[side, side];
            _gridTiles = new GridTile[side, side];

            int emptyX = Random.Range(0, side - 1);
            int emptyY = Random.Range(0, side - 1);

            int actualTileNumber = 1;

            if (!createCheatGrid)
            {
                for (int y = side - 1; y >= 0; y--)
                {
                    for (int x = 0; x < side; x++)
                    {
                        Vector3 position = new Vector3(x * tileOffset, y * tileOffset, 0f);

                        _gridTiles[x, y] = new GridTile(actualTileNumber);
                        actualTileNumber++;

                        if (x != emptyX || y != emptyY)
                        {
                            int slimeID = GetRandomSlimeIndex();
                            if (slimeID >= 0)
                            {
                                SpawnSlime(slimesPrefabs[slimeID], position);
                            }
                        }
                        else
                        {
                            _freeX = x;
                            _freeY = y;
                        }
                    }
                }
            }
            else
            {
                CreateCheatedGrid(actualTileNumber);
            }
        }
        
        private void CreateCheatedGrid(int actualTileNumber)
        {
            // Spawn tiles.
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    _gridTiles[x, y] = new GridTile(actualTileNumber);
                    actualTileNumber++;
                }
            }

            // Spawn first three lines.
            int slimeID = 0;
            for (int y = side - 1; y >= 1; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    Vector3 position = new Vector3(x * tileOffset, y * tileOffset, 0f);
                    SpawnSlime(slimesPrefabs[slimeID].gameObject, position);
                    slimeID++;
                }
            }
                
            // Spawn last and lowest line.
            int lastY = 0;
            for (int x = 1; x < side; x++)
            {
                Vector3 position = new Vector3(x * tileOffset, lastY * tileOffset, 0f);
                SpawnSlime(slimesPrefabs[slimeID].gameObject, position);
                slimeID++;
            }
        }

        private int GetRandomSlimeIndex()
        {
            if (_slimesIndexes.Count <= 0) return -1;
         
            int newIndex = Random.Range(0, _slimesIndexes.Count - 1);
            int indexToReturn = _slimesIndexes[newIndex];
            _slimesIndexes.RemoveAt(newIndex);
            return indexToReturn;
        }

        private void SpawnSlime(GameObject slimePrefab, Vector3 position)
        {
            GameObject slime = Instantiate(slimePrefab, position, Quaternion.identity);
            slime.transform.parent = slimesParent.transform;
            
            int x = (int) (position.x / tileOffset);
            int y = (int) (position.y / tileOffset);
            _slimes[x, y] = slime.GetComponent<SlimeBehaviour>();
            _slimes[x, y].MyGridCoordinates = new Vector2(x, y);
        }
    }
}
