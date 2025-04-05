using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class DropDownArrowController : MonoBehaviour
    {
        public Dropdown dropdown;
        private int _currentIndex;

        private void Start()
        {
            if (dropdown == null || dropdown.options.Count == 0)
            {
                enabled = false;
            }
        }

        public void ChangeLeft()
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = dropdown.options.Count + _currentIndex;
            }

            dropdown.value = _currentIndex;
        }
        
        public void ChangeRight()
        {
            _currentIndex++;
            if (_currentIndex >= dropdown.options.Count)
            {
                _currentIndex = 0;
            }

            dropdown.value = _currentIndex;
        }
    }
}