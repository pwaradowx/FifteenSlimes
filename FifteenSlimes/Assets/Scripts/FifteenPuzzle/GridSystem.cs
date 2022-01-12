using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Assets.FifteenPuzzle
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private bool createCheatGrid;
        
        [SerializeField] private int side;
        [SerializeField] private float tileOffset;

        [SerializeField] private GameObject[] slimesPrefabs;

        [Tooltip("Add here a gameobject called Dynamic under the World gameobject to keep scene clean.")]
        [SerializeField] private GameObject parent;

        private readonly List<int> _slimesIndexes = new List<int>();
        private SlimeBehaviour[,] _slimes;
        private GridTile[,] _gridTiles;
        private bool[,] _isIFullArray;
        
        public void TryToMoveSlime(Vector2 targetDirection, SlimeBehaviour slime)
        {
            int currentX = (int) slime.MyGridCoordinates.x;
            int currentY = (int) slime.MyGridCoordinates.y;

            int targetX = (int) (currentX + targetDirection.x);
            int targetY = (int) (currentY + targetDirection.y);

            if (targetX < 0) targetX = 0;
            if (targetY < 0) targetY = 0;
            if (targetX > side - 1) targetX = side - 1;
            if (targetY > side - 1) targetY = side - 1;
            
            if (!_isIFullArray[targetX, targetY])
            {
                Vector3 targetPosition = new Vector3(targetX, targetY) * tileOffset;
                StartCoroutine(LerpSlimeMovement(_slimes[currentX, currentY].gameObject.transform,
                    _slimes[currentX, currentY].gameObject.transform.position, targetPosition,
                    0.5f * Time.deltaTime));

                _slimes[targetX, targetY] = _slimes[currentX, currentY];
                _slimes[currentX, currentY] = null;

                _isIFullArray[targetX, targetY] = true;
                _isIFullArray[currentX, currentY] = false;

                slime.MyGridCoordinates = new Vector2(targetX, targetY);

                if (CheckWinCondition())
                {
                    print("You win!");
                }
            }
            else
            {
                print("I can't move slime to this position!");
            }
        }

        private IEnumerator LerpSlimeMovement(Transform toMove, Vector3 start, Vector3 end, float step)
        {
            float startTime = Time.time;
            while (Time.time < startTime + step)
            {
                toMove.position = Vector3.Lerp(start, end, (Time.time - startTime)/step);
                yield return null;
            }

            toMove.position = end;
        }

        private bool CheckWinCondition()
        {
            int successfulConditions = 0;
            
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    if ((x == side - 1) && (y == 0))
                    {
                        if (_slimes[x, y] == null)
                        {
                            successfulConditions++;
                        }
                    }
                    else
                    {
                        if (_slimes[x, y] == null) continue;
                        
                        if (_gridTiles[x, y].MyNumber == _slimes[x, y].MyNumber)
                        {
                            successfulConditions++;
                        }
                    }
                }
            }
            
            return successfulConditions == 16;
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
            _isIFullArray = new bool[side, side];

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

                        _gridTiles[x, y] = new GridTile((GridTile.TileNumber) actualTileNumber);
                        actualTileNumber++;

                        if (x != emptyX || y != emptyY)
                        {
                            int slimeID = GetRandomSlimeIndex();
                            if (slimeID >= 0)
                            {
                                SpawnSlime(slimesPrefabs[slimeID].gameObject, position);
                            }

                            _isIFullArray[x, y] = true;
                        }
                        else
                        {
                            _isIFullArray[x, y] = false;
                        }
                    }
                }
            }
            else
            {
                CreateCheatedGrid(actualTileNumber);
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
            slime.transform.parent = parent.transform;
            
            int x = (int) (position.x / tileOffset);
            int y = (int) (position.y / tileOffset);
            _slimes[x, y] = slime.GetComponent<SlimeBehaviour>();
            _slimes[x, y].MyGridCoordinates = new Vector2(x, y);
        }

        private void CreateCheatedGrid(int actualTileNumber)
        {
            // Spawn tiles.
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    _gridTiles[x, y] = new GridTile((GridTile.TileNumber) actualTileNumber);
                    actualTileNumber++;
                }
            }

            // Spawn first three lines.
            int id = 0;
            for (int y = side - 1; y >= 1; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    Vector3 position = new Vector3(x * tileOffset, y * tileOffset, 0f);
                        
                    SpawnSlime(slimesPrefabs[id].gameObject, position);
                    id++;
                    _isIFullArray[x, y] = true;
                }
            }
                
            // Spawn last and lowest line.
            int lastY = 0;
            for (int x = 1; x < side; x++)
            {
                Vector3 position = new Vector3(x * tileOffset, lastY * tileOffset, 0f);
                SpawnSlime(slimesPrefabs[id].gameObject, position);
                id++;
                _isIFullArray[x, lastY] = true;
            }

            _isIFullArray[0, 0] = false;
        }
    }
}
