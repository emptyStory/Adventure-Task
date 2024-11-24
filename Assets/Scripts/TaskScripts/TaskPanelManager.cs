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

    //����� ��� �������� ���������� ��� �������, �������������� ����� �� ������� ���������� ��������� ������
    public TMP_InputField hoursInput; // ���� ����� ��� �����
    public TMP_InputField minutesInput; // ���� ����� ��� �����
    public TMP_Text countdownText; // ��������� ���� ��� ����������� ��������� �������

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

        buttonsList.activeObject.materialData.Add(newMaterialData); // ��������� ������� � ������

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

                // �������� ��������� TMP_Text �� ��������� ������� questButton
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();

                if (questButtonTextComponent != null)
                {
                    questButtonTextComponent.text = element.name; // ������������� ����� ������
                }
                else
                {
                    Debug.LogWarning("��������� TMP_Text �� ������ �� �������� ������� questButton: " + element.questButton.name);
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

        // ������ ��������� ������
        if (int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            // ������������� ����� � �������
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
        // ������� ������ Timer � ��������� ��� � �����
        GameObject timerObject = new GameObject("Timer");
        timer = timerObject.AddComponent<Timer>();

        // ������������� �� ������� ���������� �������
        timer.TimerFinished += OnTimerFinished;
    }

    private IEnumerator UpdateCountdownDisplay()
    {
        while (timer.TimeRemaining > 0)
        {
            UpdateTimeDisplay(timer.TimeRemaining);
            yield return new WaitForSeconds(1f); // ���� 1 �������
        }

        UpdateTimeDisplay(0); // ��������� ��������� ���� �� ��������� �������
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer finished!"); // ��������� � ������� � ���������� �������
        UpdateTimeDisplay(0); // ��������� ��������� ���� �� ��������� �������
    }

    private void UpdateTimeDisplay(float timeRemaining)
    {
        // ����������� ����� � ������ �����, ����� � ������
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // ��������� ��������� ����
        countdownText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

}
