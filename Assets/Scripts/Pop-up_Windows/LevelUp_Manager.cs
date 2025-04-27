using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp_Manager : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private RectTransform targetObject; // Ссылка на RectTransform объекта
    [SerializeField] private float animationSpeed = 1f; // Скорость анимации
    [SerializeField] private float sizeAmplitude = 0.2f; // Амплитуда изменения размера (20% от исходного)
    [SerializeField] private GameObject parentObject; // Ссылка на родительский объект для уничтожения

    private Vector2 originalSize; // Исходный размер объекта
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (targetObject != null)
        {
            originalSize = targetObject.sizeDelta;
        }
        else
        {
            Debug.LogWarning("Target object is not assigned in LevelUp_Manager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null) return;

        // Увеличиваем таймер с учетом скорости
        timer += Time.deltaTime * animationSpeed;

        // Вычисляем масштаб по синусоиде (значение от -1 до 1)
        float sinValue = Mathf.Sin(timer);

        // Преобразуем в диапазон от (1 - amplitude) до (1 + amplitude)
        float scaleFactor = 1f + sinValue * sizeAmplitude;

        // Применяем новый размер
        targetObject.sizeDelta = originalSize * scaleFactor;
    }

    // Метод для установки нового целевого объекта
    public void SetTargetObject(RectTransform newTarget)
    {
        // Сбрасываем размер предыдущего объекта, если он был
        if (targetObject != null)
        {
            targetObject.sizeDelta = originalSize;
        }

        targetObject = newTarget;
        originalSize = newTarget.sizeDelta;
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