using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Text Settings")]
    public TMP_Text textMeshPro; // Ссылка на компонент TextMeshPro
    public float typingSpeed = 0.05f; // Скорость печати (в секундах между символами)

    [TextArea(3, 10)]
    public string fullText; // Полный текст, который будет появляться постепенно

    [Header("Objects To Activate")]
    public List<GameObject> objectsToEnableAfterTyping; // Список объектов для активации после печати

    private string currentText = "";
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Функция, которую можно повесить на кнопку
    public void StartTyping()
    {
        // Если текст уже печатается, не начинаем заново
        if (isTyping) return;

        // Очищаем предыдущий текст
        currentText = "";
        textMeshPro.text = currentText;

        // Запускаем корутину с эффектом печати
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;

        // Постепенно добавляем символы
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textMeshPro.text = currentText;

            // Ждем указанное время перед добавлением следующего символа
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // Активируем все объекты из списка после завершения печати
        ActivateObjects();
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToEnableAfterTyping)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Объект в списке objectsToEnableAfterTyping не назначен!", this);
            }
        }
    }

    // Опционально: функция для прерывания эффекта
    public void StopTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textMeshPro.text = fullText;
            isTyping = false;

            // Активируем объекты даже если печать прервана
            ActivateObjects();
        }
    }
}