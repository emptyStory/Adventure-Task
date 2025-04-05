using UnityEngine;
using UnityEngine.Events;

namespace Controllers.UI
{
    public class OneKeyAction : MonoBehaviour
    {
        public KeyCode keyCode;
        public UnityEvent unityEvent;

        private void Start()
        {
            enabled = unityEvent != null && !KeyCode.None.Equals(keyCode);
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                unityEvent.Invoke();
            }
        }
    }
}