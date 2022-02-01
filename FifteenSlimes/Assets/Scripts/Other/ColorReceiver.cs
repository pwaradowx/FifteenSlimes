using UnityEngine;
using Project.Assets.Managers;

namespace Project.Assets.Other
{
    [RequireComponent(typeof(Camera))]
    public class ColorReceiver : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Camera>().backgroundColor = BackgroundColor.Instance.ColorToReceive;
        }
    }
}
