using UnityEngine;

public class RectTransformModifier : MonoBehaviour
{
    // Ссылаемся на RectTransform текущего объекта
    private RectTransform rectTransform;

    // Ссылаемся на RectTransform объекта панели квеста (если он найден)
    private RectTransform questPanelTransform;

    void Start()
    {
        // Получаем RectTransform текущего объекта
        rectTransform = GetComponent<RectTransform>();

        // Ищем объект с тегом "questPanelprefabParentTransform"
        GameObject questPanelObject = GameObject.FindGameObjectWithTag("questPanelprefabParentTransform");

        // Проверяем, найден ли объект с нужным тегом
        if (questPanelObject != null)
        {
            // Получаем RectTransform найденного объекта
            questPanelTransform = questPanelObject.GetComponent<RectTransform>();
        }
        else
        {
            // Если объект не найден, выводим предупреждение
            Debug.LogWarning("Объект с тегом 'questPanelprefabParentTransform' не найден!");
        }

        // Устанавливаем начальное значение bottom (отступ снизу)
        SetBottom(-200);
    }

    // Метод для изменения RectTransform
    public void ModifyRectTransforms()
    {
        // Устанавливаем значение bottom (отступ снизу) для текущего объекта
        SetBottom(-200);

        // Проверяем, был ли найден объект с RectTransform панели квеста
        if (questPanelTransform != null)
        {
            // Получаем текущую позицию панели квеста
            Vector2 newPos = questPanelTransform.anchoredPosition;

            // Ограничиваем значение y позиции панели, чтобы она не выходила за пределы -200
            newPos.y = Mathf.Max(newPos.y + 100, -200); // Прибавляем 100 и ограничиваем снизу значением -200

            // Применяем новое значение позиции панели квеста
            questPanelTransform.anchoredPosition = newPos;
        }
    }

    // Метод для установки значения bottom (отступ снизу)
    private void SetBottom(float value)
    {
        // Получаем текущие отступы RectTransform
        Vector2 offset = rectTransform.offsetMin;

        // Устанавливаем новое значение для отступа снизу (bottom)
        offset.y = value;

        // Применяем изменённый отступ
        rectTransform.offsetMin = offset;
    }
}