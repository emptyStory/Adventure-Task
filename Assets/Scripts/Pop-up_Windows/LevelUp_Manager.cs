using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp_Manager : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private List<RectTransform> targetObjects = new List<RectTransform>(); // Список объектов для анимации
    [SerializeField] private float animationSpeed = 1f; // Скорость анимации
    [SerializeField] private float sizeAmplitude = 0.2f; // Амплитуда изменения размера (20% от исходного)
    [SerializeField] private GameObject parentObject; // Ссылка на родительский объект для уничтожения

    private Dictionary<RectTransform, Vector2> originalSizes = new Dictionary<RectTransform, Vector2>(); // Словарь для хранения исходных размеров
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Инициализируем оригинальные размеры для всех объектов
        foreach (var target in targetObjects)
        {
            if (target != null)
            {
                originalSizes[target] = target.sizeDelta;
            }
            else
            {
                Debug.LogWarning("One of target objects is null in LevelUp_Manager!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObjects.Count == 0) return;

        // Увеличиваем таймер с учетом скорости
        timer += Time.deltaTime * animationSpeed;

        // Вычисляем масштаб по синусоиде (значение от -1 до 1)
        float sinValue = Mathf.Sin(timer);

        // Преобразуем в диапазон от (1 - amplitude) до (1 + amplitude)
        float scaleFactor = 1f + sinValue * sizeAmplitude;

        // Применяем новый размер ко всем объектам
        foreach (var target in targetObjects)
        {
            if (target != null && originalSizes.ContainsKey(target))
            {
                target.sizeDelta = originalSizes[target] * scaleFactor;
            }
        }
    }

    // Метод для добавления нового целевого объекта
    public void AddTargetObject(RectTransform newTarget)
    {
        if (newTarget == null) return;

        if (!targetObjects.Contains(newTarget))
        {
            targetObjects.Add(newTarget);
            originalSizes[newTarget] = newTarget.sizeDelta;
        }
    }

    // Метод для удаления целевого объекта
    public void RemoveTargetObject(RectTransform targetToRemove)
    {
        if (targetToRemove == null) return;

        if (targetObjects.Contains(targetToRemove))
        {
            // Восстанавливаем оригинальный размер перед удалением
            if (originalSizes.ContainsKey(targetToRemove))
            {
                targetToRemove.sizeDelta = originalSizes[targetToRemove];
                originalSizes.Remove(targetToRemove);
            }
            targetObjects.Remove(targetToRemove);
        }
    }

    // Метод для очистки всех целевых объектов
    public void ClearAllTargets()
    {
        // Восстанавливаем оригинальные размеры перед очисткой
        foreach (var target in targetObjects)
        {
            if (target != null && originalSizes.ContainsKey(target))
            {
                target.sizeDelta = originalSizes[target];
            }
        }

        targetObjects.Clear();
        originalSizes.Clear();
        timer = 0f;
    }

    // Метод для закрытия и уничтожения родительского объекта
    public void Close()
    {
        if (parentObject != null && parentObject.activeInHierarchy)
        {
            Destroy(parentObject);
            // Или если хотите просто скрыть, а не уничтожать:
            // parentObject.SetActive(false);
        }
    }
}