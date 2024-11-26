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
    public float maxAngleDeviation = 35f; // Максимальное отклонение в градусах

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
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
        }
    }
}