using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Controllers.UI
{
    public class DoubleClickHandler : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent onDoubleClick;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.clickCount == 2) {
                onDoubleClick.Invoke();
            }
        }
    }
}