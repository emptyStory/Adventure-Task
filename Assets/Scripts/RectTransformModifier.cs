using UnityEngine;

public class RectTransformModifier : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform questPanelTransform;

    void Start()
    {
        // Получаем RectTransform текущего объекта
        rectTransform = GetComponent<RectTransform>();

        // Находим объект с тегом "questPanelprefabParentTransform"
        GameObject questPanelObject = GameObject.FindGameObjectWithTag("questPanelprefabParentTransform");
        if (questPanelObject != null)
        {
            questPanelTransform = questPanelObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("Объект с тегом 'questPanelprefabParentTransform' не найден!");
        }

        // Устанавливаем начальное значение bottom
        SetBottom(-200);
    }

    public void ModifyRectTransforms()
    {
        // Изменяем параметр bottom текущего RectTransform
        SetBottom(-200);

        // Проверяем, что questPanelTransform установлен
        if (questPanelTransform != null)
        {
            // Устанавливаем ограничение по Y для questPanelTransform
            Vector2 newPos = questPanelTransform.anchoredPosition;
            newPos.y = Mathf.Max(newPos.y + 100, -200); // Ограничиваем y на -200 и прибавляем 100
            questPanelTransform.anchoredPosition = newPos;
        }
    }

    private void SetBottom(float value)
    {
        // Изменяем значение bottom RectTransform
        Vector2 offset = rectTransform.offsetMin; // Получаем текущие отступы
        offset.y = value; // Устанавливаем новое значение bottom
        rectTransform.offsetMin = offset; // Применяем отступы
    }
}