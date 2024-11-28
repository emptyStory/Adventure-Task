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
    // ������ ��� �������� ���� ��������, ������� �������� ���������� ��� ������
    private GameObject[] taskPanelPrefabParents;

    // ���������� ��� ������� � ������ ������
    private DatabaseManager buttonsList;
    private GameObject buttonsListGameObject;

    // ���������� ��� ����� ������ � ������ (��������, �������� � ������)
    [SerializeField] private Task task;
    public new TMP_Text name; // ���� ��� ����� ����� ������
    public TMP_Text description; // ���� ��� ����� �������� ������

    public GameObject prefab;
    public GameObject taskButton;

    public GameObject currentTask; // ������� ������, ������� �������������

    public GameObject taskButtonCreate; // ������ ��� �������� ������
    public GameObject taskButtonEdit; // ������ ��� �������������� ������

    // ���������� ��� ������������ ������� ������ (������)
    public TMP_InputField hoursInput; // ���� ����� ��� �����
    public TMP_InputField minutesInput; // ���� ����� ��� �����
    public TMP_Text countdownText; // ���� ��� ����������� ������� ��������� �������

    private Timer timer; // ��������� �������

    private void Start()
    {
        // ���� ��� ������� � ����� "taskPanelPrefabParentTransform"
        taskPanelPrefabParents = GameObject.FindGameObjectsWithTag("taskPanelPrefabParentTransform");

        // �������� ������ SceneManager � ������ � ���� ������ ������
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();
    }

    // ����� ��� �������� ����� ������
    public void TaskButtonCreate()
    {
        GameObject taskTransform;
        // ������� ������ � ����� "taskPanelPrefabParentTransform" ��� ���������� ������
        taskTransform = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        // ������� ����� ��������� ������ ������ � ������������� � � ������������ ������
        GameObject instantTaskPrefab = Instantiate(taskButton, transform.position, transform.rotation);
        instantTaskPrefab.transform.SetParent(taskTransform.transform);
        instantTaskPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPrefab.transform.localScale = new Vector3(1, 1, 1);

        // ������� ����� ������ ������ ��� ������
        Task.MaterialData newMaterialData = new Task.MaterialData
        {
            name = name.text, // ��� ������
            description = description.text, // �������� ������
            questButton = instantTaskPrefab // ������ ��� ������
        };

        // ��������� ����� ������ � ������
        buttonsList.activeObject.materialData.Add(newMaterialData);

        // ��������� ������ ��� ������
        StartTimer();

        // ������� ������
        Destroy(prefab);
    }

    // ����� ��� �������������� ������������ ������
    public void TaskButtonEdit()
    {
        // ���� ������ � ������ � ��������� � ������
        foreach (var element in buttonsList.taskHolder.materialData)
        {
            if (element.questButton == currentTask)
            {
                element.name = name.text;
                element.description = description.text;

                // ��������� ����� �� ������
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();
                if (questButtonTextComponent != null)
                {
                    questButtonTextComponent.text = element.name; // ��������� ����� ������
                }
                else
                {
                    Debug.LogWarning("��������� TMP_Text �� ������ �� �������� ������� questButton: " + element.questButton.name);
                }
            }
        }

        // ������ ��������� ������ ��� �������������� � �������� ������
        taskButtonEdit.SetActive(false);
        taskButtonCreate.SetActive(true);

        // ������� ������
        Destroy(prefab);
    }

    // ����� ��� ������ �������
    private void StartTimer()
    {
        BeforeStartTimer(); // ���������� � ������� �������

        // ������ ��������� ������ ��� ����� � �����
        if (int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            // ������������� ����� � �������
            timer.SetTime(hours, minutes);
            timer.StartCountdown(timer);
            StartCoroutine(UpdateCountdownDisplay()); // ��������� ���������� ����������� �������
        }
        else
        {
            Debug.LogError("�������� ����! ����������, ������� ���������� ���� � ������.");
        }
    }

    // ����� ��� ���������� ������� ����� ��������
    public void BeforeStartTimer()
    {
        // ������� ������ Timer � ��������� ��� �� �����
        GameObject timerObject = new GameObject("Timer");
        timer = timerObject.AddComponent<Timer>();

        // ������������� �� ������� ���������� �������
        //timer.TimerFinished += OnTimerFinished;
    }

    // ����� ��� ���������� ����������� ������� � �������� �������
    private IEnumerator UpdateCountdownDisplay()
    {
        while (timer.TimeRemaining > 0)
        {
            // ��������� ����������� �������
            UpdateTimeDisplay(timer.TimeRemaining);
            yield return new WaitForSeconds(1f); // ���� 1 �������
        }

        // ��������� ����������� �������, ����� ������ ��������
        UpdateTimeDisplay(0);
    }

    /*
    // �����, ������� ���������� �� ���������� �������
    private void OnTimerFinished()
    {
        Debug.Log("������ ��������!"); // ��������� � �������
        UpdateTimeDisplay(0); // ��������� ����������� �������
    }
    */

    // ����� ��� ���������� ���������� ���� � ���������� ��������
    private void UpdateTimeDisplay(float timeRemaining)
    {
        // ����������� ���������� ����� � ������ �����, ����� � ������
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // ��������� ��������� ���� � ������������ �������
        countdownText.text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}