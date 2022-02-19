using System.IO;
using System.Threading.Tasks;
using Project.Assets.MainMenu;
using Project.Assets.Managers;
using UnityEngine;
using TMPro;

namespace Project.Assets.Puzzle
{
    public class Stopwatch : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentTimeHolder;

        [SerializeField] private SelectMode.Mode mode;
        
        private bool _stopwatchIsActive;

        private float _currentMillisec;
        private int _currentSec;
        private int _currentMin;
        private int _currentHour;
        
        private int _eightPuzzleBestSec;
        private int _eightPuzzleBestMin;
        private int _eightPuzzleBestHour;
        
        private int _fifteenPuzzleBestSec;
        private int _fifteenPuzzleBestMin;
        private int _fifteenPuzzleBestHour;

        public void StartStopwatch()
        {
            _stopwatchIsActive = true;
        }

        public (int, int, int) GetCurrentTime()
        {
            return (_currentHour, _currentMin, _currentSec);
        }

        public (int?, int?, int?) GetBestTime()
        {
            if (mode == SelectMode.Mode.EightPuzzle)
            {
                return (_eightPuzzleBestHour, _eightPuzzleBestMin, _eightPuzzleBestSec);
            }
            else if (mode == SelectMode.Mode.FifteenPuzzle)
            {
                return (_fifteenPuzzleBestHour, _fifteenPuzzleBestMin, _fifteenPuzzleBestSec);
            }
            else
            {
                return (null, null, null);
            }
        }

        private void Start()
        {
            StopwatchData data = LoadStopwatchData();
            
            if (data == null) return;
            
            _eightPuzzleBestSec = data.EightPuzzleBestSec;
            _eightPuzzleBestMin = data.EightPuzzleBestMin;
            _eightPuzzleBestHour = data.EightPuzzleBestHour;
            
            _fifteenPuzzleBestSec = data.FifteenPuzzleBestSec;
            _fifteenPuzzleBestMin = data.FifteenPuzzleBestMin;
            _fifteenPuzzleBestHour = data.FifteenPuzzleBestHour;

            EventManager.Instance.PlayerSolvedPuzzleEvent += OnPlayerSolvedPuzzle;
        }

        private StopwatchData LoadStopwatchData()
        {
            string pathToData = Application.persistentDataPath + "/StopwatchData.txt";

            if (File.Exists(pathToData))
            {
                string stringJson = File.ReadAllText(pathToData);

                StopwatchData data = JsonUtility.FromJson<StopwatchData>(stringJson);
                return data;
            }
            
            Debug.Log("There is no file located in" + pathToData);
            return null;
        }

        private void FixedUpdate()
        {
            if (!_stopwatchIsActive) return;
            
            CurrentTimeLogic();
        }

        private void CurrentTimeLogic()
        {
            _currentMillisec += 0.02f;

            if (_currentMillisec >= 1f)
            {
                _currentMillisec = 0f;
                _currentSec++;
            }

            if (_currentSec == 60)
            {
                _currentSec = 0;
                _currentMin++;
            }

            if (_currentMin == 60)
            {
                _currentMin = 0;
                _currentHour++;
            }

            currentTimeHolder.text = $"{_currentHour}h:{_currentMin}m:{_currentSec}s";
        }

        private async void OnPlayerSolvedPuzzle()
        {
            await StopStopwatch();
            
            SaveStopwatchData(_eightPuzzleBestSec, _eightPuzzleBestMin, _eightPuzzleBestHour,
                _fifteenPuzzleBestSec, _fifteenPuzzleBestMin, _fifteenPuzzleBestHour);
        }
        
        private async Task StopStopwatch()
        {
            _stopwatchIsActive = false;
            
            BestTimeLogic();

            await Task.CompletedTask;
        }

        private void BestTimeLogic()
        {
            if (mode == SelectMode.Mode.EightPuzzle)
            {
                // If best time was changed already because as minimum one value is not equals to 0,
                // So change best time only if it less or equals to current time by further logic. 
                if (_eightPuzzleBestSec != 0 || _eightPuzzleBestMin != 0 || _eightPuzzleBestHour != 0)
                {
                    if (_currentHour <= _eightPuzzleBestHour)
                    {
                        _eightPuzzleBestHour = _currentHour;

                        if (_currentMin <= _eightPuzzleBestMin)
                        {
                            _eightPuzzleBestMin = _currentMin;

                            if (_currentSec <= _eightPuzzleBestSec)
                            {
                                _eightPuzzleBestSec = _currentSec;
                            }
                        }
                    }
                }
                // Else means all values are equal to 0,
                // So it means it's player's first win
                // And we need just to save the current time values to best time values.
                else
                {
                    _eightPuzzleBestHour = _currentHour;
                    _eightPuzzleBestMin = _currentMin;
                    _eightPuzzleBestSec = _currentSec;
                }
            }
            else if (mode == SelectMode.Mode.FifteenPuzzle)
            {
                if (_fifteenPuzzleBestSec != 0 || _fifteenPuzzleBestMin != 0 || _fifteenPuzzleBestHour != 0)
                {
                    if (_currentHour <= _fifteenPuzzleBestHour)
                    {
                        _fifteenPuzzleBestHour = _currentHour;

                        if (_currentMin <= _fifteenPuzzleBestMin)
                        {
                            _fifteenPuzzleBestMin = _currentMin;

                            if (_currentSec <= _fifteenPuzzleBestSec)
                            {
                                _fifteenPuzzleBestSec = _currentSec;
                            }
                        }
                    }
                }
                // Else means all values are equal to 0,
                // So it means it's player's first win
                // And we need just to save the current time values to best time values.
                else
                {
                    _fifteenPuzzleBestHour = _currentHour;
                    _fifteenPuzzleBestMin = _currentMin;
                    _fifteenPuzzleBestSec = _currentSec;
                }
            }
        }

        private void SaveStopwatchData(int eightSec, int eightMin, int eightHour, int fifteenSec, int fifteenMin,
            int fifteenHour)
        {
            StopwatchData data = new StopwatchData(eightSec, eightMin, eightHour,
                fifteenSec, fifteenMin, fifteenHour);

            string stringJson = JsonUtility.ToJson(data);
            string pathToData = Application.persistentDataPath + "/StopwatchData.txt";
            File.WriteAllText(pathToData, stringJson);
        }
    }
}
