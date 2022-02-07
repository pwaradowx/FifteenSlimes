using System.IO;
using Project.Assets.MainMenu;
using UnityEngine;
using TMPro;

namespace Project.Assets.Puzzle
{
    public class Stopwatch : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentTimeHandler;

        [SerializeField] private SelectMode.Mode mode;
        
        private bool _stopwatchIsActive;

        private float _currentMillisec;
        private int _currentSec;
        private int _currentMin;
        private int _currentHour;
        
        private int _bestSec;
        private int _bestMin;
        private int _bestHour;

        public void StartStopwatch()
        {
            _stopwatchIsActive = true;
        }

        public void StopStopwatch()
        {
            _stopwatchIsActive = false;
            
            BestTimeLogic();
        }

        private void Start()
        {
            StopwatchData data = LoadStopwatchData();
            
            if (data == null) return;
            
            if (mode == SelectMode.Mode.EightPuzzle)
            {
                _bestSec = data.EightPuzzleBestSec;
                _bestMin = data.EightPuzzleBestMin;
                _bestHour = data.EightPuzzleBestHour;
            }
            else if (mode == SelectMode.Mode.FifteenPuzzle)
            {
                _bestSec = data.FifteenPuzzleBestSec;
                _bestMin = data.FifteenPuzzleBestMin;
                _bestHour = data.FifteenPuzzleBestHour;
            }
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

            currentTimeHandler.text = $"{_currentHour}h:{_currentMin}m:{_currentSec}s";
        }

        private void BestTimeLogic()
        {
            // If best time was changed already because as minimum one value is not equals to 0,
            // So change best time only if it less or equals to current time by further logic. 
            if (_bestSec != 0 || _bestMin != 0 || _bestHour != 0)
            {
                if (_currentHour <= _bestHour)
                {
                    _bestHour = _currentHour;

                    if (_currentMin <= _bestMin)
                    {
                        _bestMin = _currentMin;

                        if (_currentSec <= _bestSec)
                        {
                            _bestSec = _currentSec;
                        }
                    }
                }
            }
            // Else means all values are equal to 0,
            // So it means it's player's first win
            // And we need just to save the current time values to best time values.
            else
            {
                _bestHour = _currentHour;
                _bestMin = _currentMin;
                _bestSec = _currentSec;
            }
        }

        private void SaveStopwatchData(int bestSec, int bestMin, int bestHour)
        {
            StopwatchData data = new StopwatchData(bestSec, bestMin, bestHour, mode);

            string stringJson = JsonUtility.ToJson(data);
            string pathToData = Application.persistentDataPath + "/StopwatchData.txt";
            File.WriteAllText(pathToData, stringJson);
        }

        private void OnDestroy()
        {
            SaveStopwatchData(_bestSec, _bestMin, _bestHour);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnDestroy();
        }
    }
}
