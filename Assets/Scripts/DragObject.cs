using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform rectTransform; // Ссылка на RectTransform объекта
    public float minX = -200f; // Минимальное значение по X
    public float maxX = 200f;  // Максимальное значение по X
    public float moveSpeed = 5f; // Скорость движения к целевому положению

    private Vector2 offset;
    private float targetX;

    void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>(); // Получаем RectTransform, если не задан
        }

        targetX = rectTransform.anchoredPosition.x; // Изначальная позиция по X
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Вычисляем смещение между позицией объекта и позицией касания
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Обновляем позицию объекта в соответствии с движением пальца
        float newX = eventData.position.x + offset.x;

        // Ограничиваем движение по X
        newX = Mathf.Clamp(newX, minX, maxX);

        // Применяем новую позицию
        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

        // Обновляем целевую позицию по X
        targetX = newX;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Определяем целевую позицию (до ближайшего ограничения)
        if (targetX < minX + (maxX - minX) / 2)
        {
            targetX = minX;
        }
        else
        {
            targetX = maxX;
        }

        // Запускаем корутину для плавного перемещения
        StartCoroutine(MoveToTarget());
    }

    private System.Collections.IEnumerator MoveToTarget()
    {
        while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f)
        {
            float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, Time.deltaTime * moveSpeed);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
            yield return null; // Ждем следующего кадра
        }

        // Устанавливаем окончательную позицию
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
    }
}