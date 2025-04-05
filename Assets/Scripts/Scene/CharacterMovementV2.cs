using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using static TimelineItems;

public class CharacterMovementV2 : MonoBehaviour
{
    public Transform[] points; // Массив точек, к которым будет двигаться персонаж
    public TimelineItems timelinesData; // Данные таймлайнов
    public PlayableDirector playableDirector; // Компонент PlayableDirector
    public Animator animator; // Аниматор персонажа

    // Список стартовых текстов
    public List<string> startTexts = new List<string>();
    public int startTextOutputDuration;

    // Поля для канваса и текстового элемента
    public Canvas canvas; // Ссылка на ваш канвас
    public TMP_Text outputText; // Ссылка на текстовый элемент, куда будет выводиться текст

    public Transform character;
    public Vector3 position;

    private List<TimelineItemsData> timelineItemsData = new List<TimelineItemsData>();
    private NavMeshAgent agent;
    private int currentPointIndex = -1; // Начальное значение -1 для случайного выбора первой точки

    private void Start()
    {
        position = character.position;

        timelineItemsData = timelinesData.timelineItemsData;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(StartCharacterBehavior());
        position = agent.transform.position;
    }

    private IEnumerator StartCharacterBehavior()
    {
        StartTextOutput();
        // Ждем некоторое время перед началом движения
        yield return new WaitForSeconds(5);

        while (true) // Бесконечный цикл для постоянного движения
        {
            animator.SetTrigger("isWalking");
            MoveToRandomPoint();

            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

            animator.SetBool("isWalking", false);
            animator.SetTrigger("isIdle");

            yield return ExecutePointProcess(); // Выполняем процесс на текущей точке
        }
    }

    private void MoveToRandomPoint()
    {
        // Проверяем, что массив точек не пустой
        if (points == null || points.Length == 0)
        {
            Debug.LogError("Массив точек пуст или не назначен!");
            return; // Прекращаем выполнение, если массив пуст
        }

        int newPointIndex;

        // Генерируем новый индекс, пока он не будет отличаться от текущего
        do
        {
            newPointIndex = Random.Range(0, points.Length);
        } while (newPointIndex == currentPointIndex);

        currentPointIndex = newPointIndex; // Обновляем текущий индекс
        // animator.SetTrigger("isWalking");
        agent.SetDestination(points[currentPointIndex].position); // Двигаемся к новой точке
    }

    private IEnumerator ExecutePointProcess()
    {
        if (currentPointIndex >= timelineItemsData.Count) // Исправлено на >=
        {
            yield break; // Завершаем выполнение, если индекс вне диапазона
        }

        TimelineItemsData currentTimeline = timelineItemsData[currentPointIndex];
        PlayTimeline(currentTimeline);
        TextOutput(); // Добавляем вывод текста

        // Ожидание завершения таймлайна
        while (playableDirector.state == PlayState.Playing) // Исправлено на Playing
        {
            yield return null; // Ждем, пока таймлайн не завершится
        }
    }

    private void PlayTimeline(TimelineItemsData timeline)
    {
        playableDirector.playableAsset = timeline.playableAsset; // Устанавливаем соответствующий таймлайн
        playableDirector.Play(); // Запускаем воспроизведение таймлайна

        // Подписываемся на событие остановки таймлайна
        playableDirector.stopped += OnTimelineStopped;
    }

    private void ResetCharacterPosition()
    {
        // Получаем текущую позицию персонажа
        Vector3 position = agent.transform.position;

        // Сбрасываем Y-координату, чтобы персонаж был на уровне земли
        position.y = 0; // Или используйте высоту, которая соответствует уровню земли в вашей сцене

        // Применяем новую позицию
        agent.transform.position = position;

        // Сбрасываем вращение персонажа
        agent.transform.rotation = Quaternion.identity; // Или используйте начальное вращение
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        Vector3 newPosition = agent.transform.position;

        newPosition.y = position.y;

        // Присваиваем новую позицию
        character.transform.position = newPosition;

        playableDirector.stopped -= OnTimelineStopped;
    }

    private IEnumerator WaitForTextOutput()
    {
        yield return new WaitForSeconds(timelineItemsData[currentPointIndex].textDuration);
        canvas.gameObject.SetActive(false); // Скрываем canvas после ожидания
    }

    private IEnumerator WaitForStartTextOutput()
    {
        yield return new WaitForSeconds(startTextOutputDuration);
        canvas.gameObject.SetActive(false); // Скрываем canvas после ожидания
    }

    private void StartTextOutput()
    {
        
       
            canvas.gameObject.SetActive(true);

            // Выбираем случайный индекс для текста
            int randomTextIndex = Random.Range(0, startTexts.Count);

            // Выводим случайный текст
            outputText.text = startTexts[randomTextIndex];

            StartCoroutine(WaitForStartTextOutput()); // Запускаем корутину
    }

    private void TextOutput()
    {
        // Проверяем, что у нас есть текст для вывода
        if (timelineItemsData[currentPointIndex].texts.Count > 0)
        {
            canvas.gameObject.SetActive(true);

            // Выбираем случайный индекс для текста
            int randomTextIndex = Random.Range(0, timelineItemsData[currentPointIndex].texts.Count);

            // Выводим случайный текст
            outputText.text = timelineItemsData[currentPointIndex].texts[randomTextIndex];

            StartCoroutine(WaitForTextOutput()); // Запускаем корутину
        }
        else
        {
            Debug.LogWarning("Нет текста для вывода!");
        }
    }
}
