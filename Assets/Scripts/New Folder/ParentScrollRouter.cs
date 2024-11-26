namespace Utilities.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    // Простой маршрутизатор событий скролла.
    // Передает события перемещения ScrollRect родительским элементам UI.
    // Необходимо вешать на дочерний ScrollRect.
    [AddComponentMenu("Utilities/UI/Parent Scroll Router")]
    [RequireComponent(typeof(ScrollRect))]
    public class ParentScrollRouter : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // Передаем родителям событие, которое отправляется перед возможным началом перемещения.
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                foreach (var handler in parent.GetComponents<IInitializePotentialDragHandler>())
                    handler.OnInitializePotentialDrag(eventData);

                parent = parent.parent;
            }
        }

        // Передаем родителям событие начала перемещения.
        public void OnBeginDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                foreach (var handler in parent.GetComponents<IBeginDragHandler>())
                    handler.OnBeginDrag(eventData);

                parent = parent.parent;
            }
        }

        // Передаем родителям событие перемещения.
        public void OnDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                foreach (var handler in parent.GetComponents<IDragHandler>())
                    handler.OnDrag(eventData);

                parent = parent.parent;
            }
        }

        // Передаем родителям событие завершения перемещения.
        public void OnEndDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            while (parent != null)
            {
                foreach (var handler in parent.GetComponents<IEndDragHandler>())
                    handler.OnEndDrag(eventData);

                parent = parent.parent;
            }
        }
    }
}