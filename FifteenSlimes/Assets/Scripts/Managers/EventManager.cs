using System;
using UnityEngine;

namespace Project.Assets.Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public event Action PlayerSolvedPuzzleEvent;
        public void OnPlayerSolvedPuzzle()
        {
            if (PlayerSolvedPuzzleEvent != null)
            {
                PlayerSolvedPuzzleEvent();
            }
        }
    }
}
