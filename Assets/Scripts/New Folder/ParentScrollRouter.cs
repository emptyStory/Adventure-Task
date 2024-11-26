namespace Utilities.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    // ������� ������������� ������� �������.
    // �������� ������� ����������� ScrollRect ������������ ��������� UI.
    // ���������� ������ �� �������� ScrollRect.
    [AddComponentMenu("Utilities/UI/Parent Scroll Router")]
    [RequireComponent(typeof(ScrollRect))]
    public class ParentScrollRouter : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // �������� ��������� �������, ������� ������������ ����� ��������� ������� �����������.
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

        // �������� ��������� ������� ������ �����������.
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

        // �������� ��������� ������� �����������.
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

        // �������� ��������� ������� ���������� �����������.
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