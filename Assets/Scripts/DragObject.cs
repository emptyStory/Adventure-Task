using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform rectTransform; // ������ �� RectTransform �������
    public float minX = -200f; // ����������� �������� �� X
    public float maxX = 200f;  // ������������ �������� �� X
    public float moveSpeed = 5f; // �������� �������� � �������� ���������

    private Vector2 offset;
    private float targetX;

    void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>(); // �������� RectTransform, ���� �� �����
        }

        targetX = rectTransform.anchoredPosition.x; // ����������� ������� �� X
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��������� �������� ����� �������� ������� � �������� �������
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��������� ������� ������� � ������������ � ��������� ������
        float newX = eventData.position.x + offset.x;

        // ������������ �������� �� X
        newX = Mathf.Clamp(newX, minX, maxX);

        // ��������� ����� �������
        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

        // ��������� ������� ������� �� X
        targetX = newX;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ���������� ������� ������� (�� ���������� �����������)
        if (targetX < minX + (maxX - minX) / 2)
        {
            targetX = minX;
        }
        else
        {
            targetX = maxX;
        }

        // ��������� �������� ��� �������� �����������
        StartCoroutine(MoveToTarget());
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f)
        {
            float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, Time.deltaTime * moveSpeed);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
            yield return null; // ���� ���������� �����
        }

        // ������������� ������������� �������
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
    }
}