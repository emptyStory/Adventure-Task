using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class ToggleArrowController : MonoBehaviour
    {
        public Toggle[] toggles;
        private int _currentIndex;

        private void Start()
        {
            if (toggles == null || toggles.Length == 0)
            {
                enabled = false;
            }
        }

        public void ToggleLeft()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = toggles.Length + _currentIndex;
            }

            toggles[_currentIndex].isOn = true;
        }
        
        public void ToggleRight()
        {
            _currentIndex++;
            if (_currentIndex >= toggles.Length)
            {
                _currentIndex = 0;
            }

            toggles[_currentIndex].isOn = true;
        }
    }
}