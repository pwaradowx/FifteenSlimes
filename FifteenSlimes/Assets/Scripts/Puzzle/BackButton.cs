using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Assets.Managers;

namespace Project.Assets.Puzzle
{
    public class BackButton : MonoBehaviour
    {
        public async void GoToMainMenu()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;
            
            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);

            SceneManager.LoadScene(0);
        }
    }
}
