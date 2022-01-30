using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Assets.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private SelectMode selectMode;
        
        public void StartPuzzle()
        {
            SceneManager.LoadScene((int) selectMode.CurrentMode);
        }
    }
}
