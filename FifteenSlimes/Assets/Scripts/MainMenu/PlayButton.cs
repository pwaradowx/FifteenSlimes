using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Assets.Managers;

namespace Project.Assets.MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private SelectMode selectMode;
        
        public async void StartPuzzle()
        {
            if (!TransitionManager.Instance.IsTransitionDone) return;
            
            TransitionManager.Instance.TransitionIn();

            await Task.Delay(TransitionManager.Instance.TransitionTime);
            
            SceneManager.LoadScene((int) selectMode.CurrentMode);
        }
    }
}
