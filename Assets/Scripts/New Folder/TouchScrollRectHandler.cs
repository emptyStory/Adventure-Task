using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchScrollRectHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // ������ �� RectTransform �������, ������� ����� ���������������
    public RectTransform rectTransform;

    // ����������� � ������������ �������� �� ��� X, � �������� ������� ������ ����� ������������
    public float minX = -200f;
    public float maxX = 200f;

    // �������� ����������� ������� � ������� �������
    public float moveSpeed = 5f;

    // �������� ������� ������������ ������ ��������������
    private Vector2 offset;

    // ������� ������� ������� �� ��� X
    private float targetX;

    private ScrollRect scrollRect;
    private Vector2 initialTouchPosition;
    private bool isDragging = false;

    [Header("Settings")]
    [Tooltip("Maximum angle deviation from vertical to consider the touch as vertical.")]
    public float maxAngleDeviation = 35f; // ������������ ���������� � ��������

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        // ���� RectTransform �� �����, �������� ��� �������������
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // ����������� ������� ������� �� ��� X
        targetX = rectTransform.anchoredPosition.x;
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

            // ��������� ����� ������� ������� �� ��� X, ����������� �� ������� ��������
            float newX = eventData.position.x + offset.x;

            // ������������ �������� ������� � �������� minX � maxX
            newX = Mathf.Clamp(newX, minX, maxX);

            // ��������� ����� ������� �� ��� X
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            // ��������� ������� ������� �� ��� X
            targetX = newX;
        }

        
    }


    // ����� ����������, ����� ���������� �������������� �������
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��������� �������� ����� ������� �������� ������� � �������� ������ ��������������
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    // ����� ����������, ����� �������������� ������� ���������
    public void OnEndDrag(PointerEventData eventData)
    {
        // ����������, � ����� ������� ������ ������ �������������
        // ���� ������� ������� ����� � minX, ���������� ������ � minX, ����� - � maxX
        if (targetX < minX + (maxX - minX) / 2)
        {
            targetX = minX;
        }
        else
        {
            targetX = maxX;
        }

        // ��������� �������� ��� �������� ����������� ������� � ������� �������
        StartCoroutine(MoveToTarget());
    }

    // ��������� ��� �������� ����������� ������� � ������� �������
    private System.Collections.IEnumerator MoveToTarget()
    {
        // ���� ������ �� ��������� ������� �������, ���������� ��� ������ ����������
        while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f)
        {
            // ������ ������ ������� ������� ����� ������� �������� � �������
            float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, Time.deltaTime * moveSpeed);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            // ���� ���������� ����� ����� ��������� ����������� �������
            yield return null;
        }

        // ������������� ������������� ������� ������� ����� � ������� �����
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
    }
}