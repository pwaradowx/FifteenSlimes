using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Assets.Puzzle
{
    public class BackgroundColor : MonoBehaviour
    {
        [SerializeField] private Color[] colors;

        private int _currentColorID;

        private const string UnavailableIDKey = "LastBackgroundColorID";

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;

            int unavailableID = PlayerPrefs.GetInt(UnavailableIDKey);
            int[] except = {unavailableID};

            _currentColorID = RandomExcept(colors.Length, except);
            _camera.backgroundColor = colors[_currentColorID];
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
