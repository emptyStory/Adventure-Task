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
    private GameObject[] taskPanelPrefabParents; //All objects that are the parents of the button's prefab are stored here

    private DatabaseManager buttonsList; //A script access variable that stores information about all buttons
    private GameObject buttonsListGameObject;

    [SerializeField] private Task task; //Variable of access to the creation of an instance of a script object that stores information entered by the user in the quest panel

    public new TMP_Text name; //The information that the user enters in the field implying the name of the quest
    public TMP_Text description; //The information that the user enters in the field implying a description of the quest

    public GameObject prefab;
    public GameObject taskButton;

    public GameObject currentTask;

    public GameObject taskButtonCreate;
    public GameObject taskButtonEdit;

    //Здесь идёт описание переменных для таймера, отслеживающего время за которое необходимо выполнить задачу
    public TMP_InputField hoursInput; // Поле ввода для часов
    public TMP_InputField minutesInput; // Поле ввода для минут
    public TMP_Text countdownText; // Текстовое поле для отображения обратного отсчёта

    private Timer timer;


    private void Start()
    {
        taskPanelPrefabParents = GameObject.FindGameObjectsWithTag("taskPanelPrefabParentTransform"); //We will find all objects on the scene with the tag questPanelPrefabParentTransform
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); //We get access to the script containing information about all the buttons
    }

    public void TaskButtonCreate() //This function describes the actions that occur after clicking the create quest button
    {
        GameObject taskTransform;
        taskTransform = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");
        GameObject instantTaskPrefab = Instantiate(taskButton, transform.position, transform.rotation);
        instantTaskPrefab.transform.SetParent(taskTransform.transform);
        instantTaskPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPrefab.transform.localScale = new Vector3(1, 1, 1);


        Task.MaterialData newMaterialData = new Task.MaterialData();
        newMaterialData.name = name.text;
        newMaterialData.description = description.text;
        newMaterialData.questButton = instantTaskPrefab;

        buttonsList.activeObject.materialData.Add(newMaterialData); // Добавляем элемент в список

        StartTimer();

        Destroy(prefab);
    }
    
    

    public void TaskButtonEdit() //This function describes the actions that occur after clicking the create quest button
    {

        foreach (var element in buttonsList.taskHolder.materialData)
        {
            if (element.questButton == currentTask)
            {
                element.name = name.text;
                element.description = description.text;

                // Получаем компонент TMP_Text из дочернего объекта questButton
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();

                if (questButtonTextComponent != null)
                {
                    questButtonTextComponent.text = element.name; // Устанавливаем текст кнопки
                }
                else
                {
                    Debug.LogWarning("Компонент TMP_Text не найден на дочернем объекте questButton: " + element.questButton.name);
                }
            }
        }

        taskButtonEdit.SetActive(false);
        taskButtonCreate.SetActive(true);

        Destroy(prefab);
    }

    private void StartTimer()
    {
        BeforeStartTimer();

        // Парсим введенные данные
        if (int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            // Устанавливаем время в таймере
            timer.SetTime(hours, minutes);
            timer.StartCountdown(timer);
            StartCoroutine(UpdateCountdownDisplay());
        }
        else
        {
            Debug.LogError("Invalid input! Please enter valid hours and minutes.");
        }
    }

    public void BeforeStartTimer()
    {
        // Создаем объект Timer и добавляем его в сцену
        GameObject timerObject = new GameObject("Timer");
        timer = timerObject.AddComponent<Timer>();

        // Подписываемся на событие завершения таймера
        timer.TimerFinished += OnTimerFinished;
    }

    private IEnumerator UpdateCountdownDisplay()
    {
        while (timer.TimeRemaining > 0)
        {
            UpdateTimeDisplay(timer.TimeRemaining);
            yield return new WaitForSeconds(1f); // Ждем 1 секунду
        }

        UpdateTimeDisplay(0); // Обновляем текстовое поле по окончании таймера
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer finished!"); // Сообщение в консоль о завершении таймера
        UpdateTimeDisplay(0); // Обновляем текстовое поле по окончании таймера
    }

    private void UpdateTimeDisplay(float timeRemaining)
    {
        // Преобразуем время в формат часов, минут и секунд
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Обновляем текстовое поле
        countdownText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

}
