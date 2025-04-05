using UnityEngine;

public class SceneController : MonoBehaviour
{
    public float moveSpeed = 0.1f; // Скорость перемещения камеры
    public float minX = -5f; // Минимальное значение по оси X
    public float maxX = 5f; // Максимальное значение по оси X
    public float minY = -5f; // Минимальное значение по оси Y
    public float maxY = 5f; // Максимальное значение по оси Y

    private Vector3 lastTouchPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                MoveCamera(touch.position);
            }
        }
    }

    private void MoveCamera(Vector3 currentTouchPosition)
    {
        Vector3 delta = currentTouchPosition - lastTouchPosition; // Разница между текущей и последней позицией
        lastTouchPosition = currentTouchPosition;

        // Перемещение камеры с учетом скорости, инвертируем Y-координату
        Vector3 newPosition = transform.position + new Vector3(delta.x * moveSpeed, -delta.y * moveSpeed, 0);

        // Ограничение перемещения по осям X и Y
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}
