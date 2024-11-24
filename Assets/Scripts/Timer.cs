using System;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    // Время, оставшееся до завершения таймера
    private float timeRemaining;

    // Свойство для получения оставшегося времени
    public float TimeRemaining => timeRemaining;

    // Событие, которое будет вызвано при завершении таймера
    public event Action TimerFinished;

    // Префаб, который будет создан по завершению таймера
    public GameObject timerFinishedPrefab;

    // Родительский объект, в который будет вставлен префаб по завершению
    public Transform parentTransform;

    // Метод для установки времени таймера
    // Принимает количество часов и минут и конвертирует их в секунды
    public void SetTime(int hours, int minutes)
    {
        timeRemaining = (hours * 3600) + (minutes * 60); // Преобразуем время в секунды
    }

    // Метод для старта отсчета таймера
    // Запускает корутину для отсчета
    public void StartCountdown(MonoBehaviour caller)
    {
        // Запускаем корутину отсчета времени
        StartCoroutine(CountdownCoroutine());
    }

    // Корутин для отсчета времени
    private IEnumerator CountdownCoroutine()
    {
        // Пока оставшееся время больше 0
        while (timeRemaining > 0)
        {
            // Ожидаем 1 секунду
            yield return new WaitForSeconds(1f);

            // Уменьшаем оставшееся время
            timeRemaining--;

            // Для отладки выводим оставшееся время в консоль
            Debug.Log(timeRemaining);
        }

        // Когда время закончилось, вызываем событие завершения таймера
        TimerFinished?.Invoke();

        // Выполняем действия после завершения таймера
        OnTimerFinished();
    }

    // Метод, который выполняется по завершении таймера
    private void OnTimerFinished()
    {
        // Выводим сообщение в консоль
        Debug.Log("Timer finished!");

        // Создаем префаб, который будет отображаться по завершении таймера
        GameObject instantTaskPanelPrefab = Instantiate(timerFinishedPrefab, transform.position, transform.rotation);

        // Устанавливаем родительский объект для префаба
        instantTaskPanelPrefab.transform.SetParent(parentTransform);

        // Устанавливаем позицию и масштаб префаба
        instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
        instantTaskPanelPrefab.transform.localScale = Vector3.one;

        // Уничтожаем объект таймера, так как он больше не нужен
        Destroy(gameObject);
    }

    // Метод для обновления состояния таймера (если требуется вручную)
    public void UpdateTimer()
    {
        // Этот метод можно использовать для обновления состояния таймера вручную, если нужно
    }
}