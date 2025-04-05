using UnityEngine;
using UnityEngine.UI;

namespace Events
{
    public class PlaceHolderDisable : MonoBehaviour
    {
        public InputField source;
        public Behaviour[] affectedComponents;

        private void Awake()
        {
            if (source == null)
            {
                enabled = false;
                return;
            }
            source.onValueChanged.AddListener(OnChange);
        }

        private void OnChange(string text)
        {
            foreach (var component in affectedComponents)
            {
                component.enabled = string.IsNullOrEmpty(text);
            }
        }
    }
}