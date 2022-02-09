using UnityEngine;

namespace Project.Assets.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
            
            Setup();
        }

        private void Setup()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
    }
}
