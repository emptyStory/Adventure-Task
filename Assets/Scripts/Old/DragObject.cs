using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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

    // ����� Start ���������� ��� ������ ����
    void Start()
    {
        // ���� RectTransform �� �����, �������� ��� �������������
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // ����������� ������� ������� �� ��� X
        targetX = rectTransform.anchoredPosition.x;
    }

    // ����� ����������, ����� ���������� �������������� �������
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ��������� �������� ����� ������� �������� ������� � �������� ������ ��������������
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    // ����� ���������� ��� ������ �������� ������� �� ����� ��������������
    public void OnDrag(PointerEventData eventData)
    {
        // ��������� ����� ������� ������� �� ��� X, ����������� �� ������� ��������
        float newX = eventData.position.x + offset.x;

        // ������������ �������� ������� � �������� minX � maxX
        newX = Mathf.Clamp(newX, minX, maxX);

        // ��������� ����� ������� �� ��� X
        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

        // ��������� ������� ������� �� ��� X
        targetX = newX;
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