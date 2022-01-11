using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundColor : MonoBehaviour
{
    [SerializeField] private Color[] colors;

    private readonly List<int> _availableColorIDs = new List<int>();
    private int _currentColorID;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;

        for (int i = 0; i < colors.Length; i++)
        {
            _availableColorIDs.Add(i);
        }

        int unavailableID = PlayerPrefs.GetInt("LastBackgroundColorID");
        _availableColorIDs.Remove(unavailableID);

        _currentColorID = RandomExcept(0, colors.Length, unavailableID);
        _camera.backgroundColor = colors[_currentColorID];
    }
    
    private int RandomExcept(int min, int max, int except)
    {
        int random = Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LastBackgroundColorID", _currentColorID);
    }
}
