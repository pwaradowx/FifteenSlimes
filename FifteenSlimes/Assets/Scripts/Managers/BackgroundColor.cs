using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Assets.Managers
{
    public class BackgroundColor : MonoBehaviour
    {
        public static BackgroundColor Instance;

        public Color ColorToReceive { get; private set; }
        
        [SerializeField] private Color[] colors;

        private int _currentColorID;

        private const string UnavailableIDKey = "LastBackgroundColorID";

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
            int unavailableID = PlayerPrefs.GetInt(UnavailableIDKey);
            int[] except = {unavailableID};

            _currentColorID = RandomExcept(colors.Length, except);
            ColorToReceive = colors[_currentColorID];
        }

        private int RandomExcept(int max, int[] except)
        {
            int result = Random.Range(0, max - except.Length);

            for (int i = 0; i < except.Length; i++)
            {
                if (result < except[i])
                    return result;
                result++;
            }

            return result;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            PlayerPrefs.SetInt(UnavailableIDKey, _currentColorID);
        }
    }
}
