using UnityEngine;
using System;
using TMPro;
using static Task;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

public class TaskPanelManager : MonoBehaviour
{
    // Массив для хранения всех объектов, которые являются родителями для кнопок
    private GameObject[] taskPanelPrefabParents;

    // Переменная для доступа к списку кнопок
    private DatabaseManager buttonsList;
    private GameObject buttonsListGameObject;

    // Переменные для ввода данных о задаче (название, описание и таймер)
    [SerializeField] private Task task;
    public new TMP_Text name; // Поле для ввода имени задачи
    public TMP_Text description; // Поле для ввода описания задачи

    public GameObject prefab;
    public GameObject taskButton;

    public GameObject currentTask; // Текущая задача, которая редактируется

    public GameObject taskButtonCreate; // Кнопка для создания задачи
    public GameObject taskButtonEdit; // Кнопка для редактирования задачи

    // Переменные для отслеживания времени задачи (таймер)
    public TMP_InputField hoursInput; // Поле ввода для часов
    public TMP_InputField minutesInput; // Поле ввода для минут
    public TMP_Text countdownText; // Поле для отображения времени обратного отсчёта

    private Timer timer; // Экземпляр таймера

    private void Start()
    {
        // Ищем все объекты с тегом "taskPanelPrefabParentTransform"
        taskPanelPrefabParents = GameObject.FindGameObjectsWithTag("taskPanelPrefabParentTransform");

        // Получаем объект SceneManager и доступ к базе данных кнопок
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();
    }

    // Метод для создания новой задачи
    public void TaskButtonCreate()
    {
        GameObject taskTransform;
        // Находим объект с тегом "taskPanelPrefabParentTransform" для добавления кнопки
        taskTransform = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        // Создаем новый экземпляр кнопки задачи и устанавливаем её в родительский объект
        GameObject instantTaskPrefab = Instantiate(taskButton, transform.position, transform.rotation);
        instantTaskPrefab.transform.SetParent(taskTransform.transform);
        instantTaskPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPrefab.transform.localScale = new Vector3(1, 1, 1);

        // Создаем новый объект данных для задачи
        Task.MaterialData newMaterialData = new Task.MaterialData
        {
            name = name.text, // Имя задачи
            description = description.text, // Описание задачи
            questButton = instantTaskPrefab // Кнопка для задачи
        };

        // Добавляем новую задачу в список
        buttonsList.activeObject.materialData.Add(newMaterialData);

        // Запускаем таймер для задачи
        StartTimer();

        // Удаляем префаб
        Destroy(prefab);
    }

    // Метод для редактирования существующей задачи
    public void TaskButtonEdit()
    {
        // Ищем задачу в списке и обновляем её данные
        foreach (var element in buttonsList.taskHolder.materialData)
        {
            if (element.questButton == currentTask)
            {
                element.name = name.text;
                element.description = description.text;

                // Обновляем текст на кнопке
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();
                if (questButtonTextComponent != null)
                {
                    questButtonTextComponent.text = element.name; // Обновляем текст кнопки
                }
                else
                {
                    Debug.LogWarning("Компонент TMP_Text не найден на дочернем объекте questButton: " + element.questButton.name);
                }
            }
        }

        // Меняем видимость кнопок для редактирования и создания задачи
        taskButtonEdit.SetActive(false);
        taskButtonCreate.SetActive(true);

        // Удаляем префаб
        Destroy(prefab);
    }

    // Метод для старта таймера
    private void StartTimer()
    {
        BeforeStartTimer(); // Подготовка к запуску таймера

        // Парсим введенные данные для часов и минут
        if (int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            // Устанавливаем время в таймере
            timer.SetTime(hours, minutes);
            timer.StartCountdown(timer);
            StartCoroutine(UpdateCountdownDisplay()); // Запускаем обновление отображения времени
        }
        else
        {
            Debug.LogError("Неверный ввод! Пожалуйста, введите корректные часы и минуты.");
        }
    }

    // Метод для подготовки таймера перед запуском
    public void BeforeStartTimer()
    {
        // Создаем объект Timer и добавляем его на сцену
        GameObject timerObject = new GameObject("Timer");
        timer = timerObject.AddComponent<Timer>();

        // Подписываемся на событие завершения таймера
        //timer.TimerFinished += OnTimerFinished;
    }

    // Метод для обновления отображения времени в реальном времени
    private IEnumerator UpdateCountdownDisplay()
    {
        while (timer.TimeRemaining > 0)
        {
            // Обновляем отображение времени
            UpdateTimeDisplay(timer.TimeRemaining);
            yield return new WaitForSeconds(1f); // Ждем 1 секунду
        }

        // Обновляем отображение времени, когда таймер завершен
        UpdateTimeDisplay(0);
    }

    /*
    // Метод, который вызывается по завершении таймера
    private void OnTimerFinished()
    {
        Debug.Log("Таймер завершен!"); // Сообщение в консоль
        UpdateTimeDisplay(0); // Обновляем отображение времени
    }
    */

    // Метод для обновления текстового поля с оставшимся временем
    private void UpdateTimeDisplay(float timeRemaining)
    {
        // Преобразуем оставшееся время в формат часов, минут и секунд
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Обновляем текстовое поле с отображением времени
        countdownText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}