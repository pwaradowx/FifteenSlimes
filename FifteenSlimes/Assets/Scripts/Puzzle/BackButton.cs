using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Assets.Puzzle
{
    public class BackButton : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
