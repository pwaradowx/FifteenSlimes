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
        
        private SlimeBehaviour[,] _slimes;
        private GridTile[,] _gridTiles;
        private int[,] _slimesNumbers;
        
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
                
                AudioManager.Instance.PlaySlimeSound();
                
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
                
                AudioManager.Instance.PlaySlimeSound();
                
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
        }

        private void Awake()
        {
            CreateGrid(side); 
        }

        private void CreateGrid(int side)
        {
            _slimes = new SlimeBehaviour[side, side];
            _gridTiles = new GridTile[side, side];
            _slimesNumbers = new int[side, side];

            int emptyX = side - 1;
            int emptyY = 0;

            int actualTileNumber = 1;
            int slimeNumber = 1;

            if (!createCheatGrid)
            {
                // Just create slimes numbers in order from up to down and from left to right.
                // This action creates solved puzzle.
                for (int y = side - 1; y >= 0; y--)
                {
                    for (int x = 0; x < side; x++)
                    {
                        _gridTiles[x, y] = new GridTile(actualTileNumber);
                        actualTileNumber++;

                        if (x != emptyX || y != emptyY)
                        {
                            _slimesNumbers[x, y] = slimeNumber;
                            slimeNumber++;
                        }
                        else
                        {
                            _slimesNumbers[x, y] = 0;
                            _freeX = x;
                            _freeY = y;
                        }
                    }
                }
                
                // Shuffle board from solved position.
                ShuffleBoard(300);
            }
            else
            {
                CreateCheatedGrid(actualTileNumber);
            }
        }
        
        private void CreateCheatedGrid(int actualTileNumber)
        {
            // Cheated grid looks like solved puzzle,
            // But empty cell locates in lowest cell on horizontal axis.
            
            // 1  2  3  4
            // 5  6  7  8
            // 9  10 11 12
            //    13 14 15
            
            int emptyX = 0;
            int emptyY = 0;
            int slimeID = 0;
            
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    _gridTiles[x, y] = new GridTile(actualTileNumber);
                    actualTileNumber++;

                    if (x != emptyX || y != emptyY)
                    {
                        Vector3 position = new Vector3(x * tileOffset, y * tileOffset, 0f);
                        SpawnSlime(slimesPrefabs[slimeID].gameObject, position);
                        slimeID++;
                    }
                }
            }
        }

        private void ShuffleBoard(int numberOfShuffles)
        {
            // Shuffle slimes numbers inside borders of int massive.
            for (int i = 0; i < numberOfShuffles; i++)
            {
                int currentX = Random.Range(0, side);
                int currentY = Random.Range(0, side);

                if (currentX == _freeX && currentY == _freeY) continue;

                int deltaX = Mathf.Abs(currentX - _freeX);
                int deltaY = Mathf.Abs(currentY - _freeY);

                if (deltaY == 0 && deltaX > 0)
                {
                    int dir = currentX > _freeX ? -1 : 1;

                    if (dir == 1)
                    {
                        for (int j = _freeX - 1; j >= currentX; j--)
                        {
                            _slimesNumbers[j + dir, currentY] = _slimesNumbers[j, currentY];
                        }
                    }
                    else
                    {
                        for (int j = _freeX + 1; j <= currentX; j++)    
                        {
                            _slimesNumbers[j + dir, currentY] = _slimesNumbers[j, currentY];
                        }
                    }
                    
                    _slimesNumbers[currentX, currentY] = 0;
                    _freeX = currentX;
                }
                else if (deltaX == 0 && deltaY > 0)
                {
                    int dir = currentY > _freeY ? -1 : 1;

                    if (dir == 1)
                    {
                        for (int j = _freeY - 1; j >= currentY; j--)
                        {
                            _slimesNumbers[currentX, j + dir] = _slimesNumbers[currentX, j];
                        }
                    }
                    else
                    {
                        for (int j = _freeY + 1; j <= currentY; j++)
                        {
                            _slimesNumbers[currentX, j + dir] = _slimesNumbers[currentX, j];
                        }
                    }
                    
                    _slimesNumbers[currentX, currentY] = 0;
                    _freeY = currentY;
                }
            }
            
            // Then spawn slimes by their numbers that were shuffled earlier.
            for (int y = side - 1; y >= 0; y--)
            {
                for (int x = 0; x < side; x++)
                {
                    if (_slimesNumbers[x, y] == 0)
                    {
                        _slimes[x, y] = null;
                        continue;
                    }
                    
                    Vector3 position = new Vector3(x * tileOffset, y * tileOffset, 0f);

                    int slimeID = _slimesNumbers[x, y] - 1;
                    
                    SpawnSlime(slimesPrefabs[slimeID], position);
                }
            }
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