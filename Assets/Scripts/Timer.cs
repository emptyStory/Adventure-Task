using System;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    // Время, оставшееся до завершения таймера
    private float timeRemaining;

    // Время, когда был запущен таймер
    private DateTime timerStartTime;

    // Свойство для получения оставшегося времени
    public float TimeRemaining => timeRemaining;

    // Событие, которое будет вызвано при завершении таймера
    public event Action TimerFinished;

    // Префаб, который будет создан по завершению таймера
    public GameObject timerFinishedPrefab;

    // Родительский объект, в который будет вставлен префаб по завершению
    public Transform parentTransform;

    // Метод для установки времени таймера
    public void SetTime(int hours, int minutes)
    {
        timeRemaining = (hours * 3600) + (minutes * 60); // Преобразуем время в секунды
        timerStartTime = DateTime.Now; // Запоминаем время начала
    }

    // Метод для старта отсчета таймера
    public void StartCountdown(MonoBehaviour caller)
    {
        // Запускаем корутину отсчета времени
        StartCoroutine(CountdownCoroutine());
    }

    // Корутин для отсчета времени
    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            Debug.Log(timeRemaining);
        }

        TimerFinished?.Invoke();
        OnTimerFinished();
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer finished!");
        characterDamage();

        GameObject instantTaskPanelPrefab = Instantiate(timerFinishedPrefab, transform.position, transform.rotation);
        instantTaskPanelPrefab.transform.SetParent(parentTransform);
        instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
        instantTaskPanelPrefab.transform.localScale = Vector3.one;

        Destroy(gameObject);
    }

    public void characterDamage()
    {
        var characterProgressControllHolder = GameObject.FindGameObjectWithTag("CharacterProgressControll");
        var characterProgressControllScript = characterProgressControllHolder.GetComponent<CharacterProgressControll>();

        float currentSliderValue = characterProgressControllScript.characterLifeSlider.value;
        float newSliderValue = currentSliderValue - 10;

        if (newSliderValue < 0)
        {
            newSliderValue = 0;
        }

        characterProgressControllScript.characterLifeSlider.value = newSliderValue;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            CheckTimerStatus();
        }
    }

    private void CheckTimerStatus()
    {
        // Получаем текущее время
        DateTime currentTime = DateTime.Now;
        Debug.Log(currentTime);

        // Вычисляем прошедшее время с момента запуска таймера
        TimeSpan elapsedTime = currentTime - timerStartTime;

        // Уменьшаем оставшееся время на прошедшее время в секундах
        timeRemaining -= (float)elapsedTime.TotalSeconds;

        // Если таймер закончился, вызываем событие завершения
        if (timeRemaining <= 0)
        {
            timeRemaining = 0; // Устанавливаем оставшееся время в 0
            TimerFinished?.Invoke();
            OnTimerFinished();
        }
        else
        {
            // Обновляем время старта на текущее (для следующего вызова)
            timerStartTime = DateTime.Now;
        }
    }
}