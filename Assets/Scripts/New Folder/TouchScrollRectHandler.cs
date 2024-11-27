using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchScrollRectHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
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

    private ScrollRect scrollRect;
    private Vector2 initialTouchPosition;
    private bool isDragging = false;

    [Header("Settings")]
    [Tooltip("Maximum angle deviation from vertical to consider the touch as vertical.")]
    public float maxAngleDeviation = 35f; // Максимальное отклонение в градусах

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        // Если RectTransform не задан, получаем его автоматически
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Изначальная позиция объекта по оси X
        targetX = rectTransform.anchoredPosition.x;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        initialTouchPosition = eventData.position; // Запоминаем начальную позицию касания
        isDragging = false; // Сбрасываем флаг перетаскивания
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            scrollRect.enabled = true; // Включаем ScrollRect обратно
            isDragging = false; // Сбрасываем флаг перетаскивания
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = eventData.position.y - initialTouchPosition.y;
        float deltaX = eventData.position.x - initialTouchPosition.x;

        // Вычисляем угол между вертикальным и горизонтальным движением
        float angle = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;

        // Проверяем, если угол меньше или равен максимальному отклонению
        if (Mathf.Abs(angle) <= maxAngleDeviation || Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
        {
            scrollRect.enabled = true; // Убедитесь, что ScrollRect включен
            isDragging = false; // Устанавливаем флаг перетаскивания в false
        }
        else
        {
            scrollRect.enabled = false; // Отключаем ScrollRect
            isDragging = true; // Устанавливаем флаг перетаскивания

            // Вычисляем новую позицию объекта по оси X, основываясь на текущем движении
            float newX = eventData.position.x + offset.x;

            // Ограничиваем движение объекта в пределах minX и maxX
            newX = Mathf.Clamp(newX, minX, maxX);

            // Применяем новую позицию по оси X
            rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);

            // Обновляем целевую позицию по оси X
            targetX = newX;
        }

        
    }


    // Метод вызывается, когда начинается перетаскивание объекта
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Вычисляем смещение между текущей позицией объекта и позицией начала перетаскивания
        offset = rectTransform.anchoredPosition - eventData.position;
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