using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchScrollRectHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private ScrollRect scrollRect;
    private Vector2 initialTouchPosition;
    private bool isDragging = false;

    [Header("Settings")]
    [Tooltip("Maximum angle deviation from vertical to consider the touch as vertical.")]
    public float maxAngleDeviation = 35f; // ������������ ���������� � ��������

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPosition = eventData.position; // ���������� ��������� ������� �������
        isDragging = false; // ���������� ���� ��������������
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            scrollRect.enabled = true; // �������� ScrollRect �������
            isDragging = false; // ���������� ���� ��������������
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = eventData.position.y - initialTouchPosition.y;
        float deltaX = eventData.position.x - initialTouchPosition.x;

        // ��������� ���� ����� ������������ � �������������� ���������
        float angle = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;

        // ���������, ���� ���� ������ ��� ����� ������������� ����������
        if (Mathf.Abs(angle) <= maxAngleDeviation || Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        {
            scrollRect.enabled = true; // ���������, ��� ScrollRect �������
            isDragging = false; // ������������� ���� �������������� � false
        }
        else
        {
            scrollRect.enabled = false; // ��������� ScrollRect
            isDragging = true; // ������������� ���� ��������������
        }
    }
}