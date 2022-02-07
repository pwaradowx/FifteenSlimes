using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Assets.Managers
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance;

        /// <summary>
        /// Time in milliseconds that takes to complete transition effect.
        /// </summary>
        public int TransitionTime => 600;
        public bool IsTransitionDone { get; private set; }

        private RawImage _transitionPanel;

        private const float Speed = 10f;
        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private const int Transparent = 1;
        private const int Opaque = 0;

        /// <summary>
        /// Makes screen dark from borders to center until it fills whole screen.
        /// </summary>
        public void TransitionIn()
        {
            StartCoroutine(SetRadius(Opaque));
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += (arg0, mode) => Setup();
        }

        private async void Setup()
        {
            _transitionPanel = GameObject.FindGameObjectWithTag("TransitionPanel").GetComponent<RawImage>();

            if (_transitionPanel == null)
            {
                Debug.LogError("Can not find the Transition Panel in the scene!");
                return;
            }

            _transitionPanel.material.SetFloat(Radius, Opaque);

            await Task.Delay(TransitionTime);

            TransitionOut();
        }
        
        private void TransitionOut()
        {
            StartCoroutine(SetRadius(Transparent));
        }

        private IEnumerator SetRadius(int targetRadius)
        {
            IsTransitionDone = false;
            
            float currentRad = _transitionPanel.material.GetFloat(Radius);
            
            while (currentRad != targetRadius)
            {
                currentRad = Mathf.Lerp(currentRad, targetRadius, Time.deltaTime * Speed);

                if (Mathf.Abs(currentRad - targetRadius) <= 0.01f)
                {
                    currentRad = targetRadius;
                    _transitionPanel.material.SetFloat(Radius, currentRad);
                    IsTransitionDone = true;
                    yield break;
                }
                
                _transitionPanel.material.SetFloat(Radius, currentRad);

                yield return null;
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus) return;
            
            if (_transitionPanel == null)
            {
                Debug.LogError("Can not find the Transition Panel in the scene!");
                return;
            }
            
            _transitionPanel.material.SetFloat(Radius, Transparent);
        }
    }
}

