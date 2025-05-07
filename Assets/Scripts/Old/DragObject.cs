using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Ссылка на RectTransform объекта, который будет перетаскиваться
    public RectTransform rectTransform;

    // Минимальное и максимальное значение по оси X, в пределах которых объект может перемещаться
    public float minX = -200f;
    public float maxX = 200f;

    // Скорость перемещения объекта к целевой позиции
    public float moveSpeed = 5f;

    // Смещение объекта относительно начала перетаскивания
    private Vector2 offset;

    // Целевая позиция объекта по оси X
    private float targetX;

    // Метод Start вызывается при старте игры
    void Start()
    {
        // Если RectTransform не задан, получаем его автоматически
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Изначальная позиция объекта по оси X
        targetX = rectTransform.anchoredPosition.x;
    }

    // Метод вызывается, когда начинается перетаскивание объекта
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Вычисляем смещение между текущей позицией объекта и позицией начала перетаскивания
        offset = rectTransform.anchoredPosition - eventData.position;
    }

    // Метод вызывается при каждом движении объекта во время перетаскивания
    public void OnDrag(PointerEventData eventData)
    {
        // Вычисляем новую позицию объекта по оси X, основываясь на текущем движении
        float newX = eventData.position.x + offset.x;

        // Ограничиваем движение объекта в пределах minX и maxX
        newX = Mathf.Clamp(newX, minX, maxX);

        // Применяем новую позицию по оси X
        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

        // Обновляем целевую позицию по оси X
        targetX = newX;
    }

    // Метод вызывается, когда перетаскивание объекта завершено
    public void OnEndDrag(PointerEventData eventData)
    {
        // Определяем, в какую сторону объект должен переместиться
        // Если текущая позиция ближе к minX, перемещаем объект в minX, иначе - в maxX
        if (targetX < minX + (maxX - minX) / 2)
        {
            targetX = minX;
        }
        else
        {
            targetX = maxX;
        }

        // Запускаем корутину для плавного перемещения объекта к целевой позиции
        StartCoroutine(MoveToTarget());
    }

    // Коррутина для плавного перемещения объекта к целевой позиции
    private System.Collections.IEnumerator MoveToTarget()
    {
        // Пока объект не достигнет целевой позиции, продолжаем его плавно перемещать
        while (Mathf.Abs(rectTransform.anchoredPosition.x - targetX) > 0.1f)
        {
            // Плавно меняем позицию объекта между текущей позицией и целевой
            float newX = Mathf.Lerp(rectTransform.anchoredPosition.x, targetX, Time.deltaTime * moveSpeed);
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            // Ждем следующего кадра перед повторным обновлением позиции
            yield return null;
        }

        // Устанавливаем окончательную позицию объекта точно в целевую точку
        rectTransform.anchoredPosition = new Vector2(targetX, rectTransform.anchoredPosition.y);
    }
}