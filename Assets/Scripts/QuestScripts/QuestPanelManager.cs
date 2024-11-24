using UnityEngine;
using System;
using TMPro;
using static Quest;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class QuestPanelManager : MonoBehaviour
{
    // ������ ��� �������� ���� ��������, ������� �������� ���������� ��� ������
    private GameObject[] questPanelPrefabParents;

    // ���������� ��� ������� � ������ ������
    private DatabaseManager buttonsList;
    private GameObject buttonsListGameObject;

    // ���� ��� ����������� �������� � �������� ������
    [SerializeField] private Task task;
    public new TMP_Text name; // ���� ��� ����� ������
    public TMP_Text description; // ���� ��� �������� ������

    // ������ ������ ������
    public GameObject prefab;
    public GameObject questButton;

    // ���������� ��� �������� ������
    public GameObject currentQuest;
    public GameObject questButtonCreate; // ������ ��� �������� ������ ������
    public GameObject questButtonEdit; // ������ ��� �������������� ������

    private void Start()
    {
        // ���� ��� ������� � ����� "questPanelPrefabParentTransform"
        questPanelPrefabParents = GameObject.FindGameObjectsWithTag("questPanelPrefabParentTransform");

        // �������� ������ � ������� SceneManager
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); // �������� ������ � ������� ���� ������ ������
    }

    // �����, ������� ���������� ��� ������� ������ "������� �����"
    public void CreateQuestButtonPressed()
    {
        // ������� ������ � ����� "questPanelPrefabParentTransform"
        GameObject questTransform = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");

        // ������� ����� ��������� ������ ������ � ��������� ��� � ������������ ������
        GameObject instantQuestPrefab = Instantiate(questButton, transform.position, transform.rotation);
        instantQuestPrefab.transform.SetParent(questTransform.transform);
        instantQuestPrefab.transform.localPosition = new Vector2(0, 0);

        // ���������, ���������� �� ��������� task
        if (task == null)
        {
            task = ScriptableObject.CreateInstance<Task>(); // ������� ����� ��������� Task
            task.materialData = new List<Task.MaterialData>(); // �������������� ������ ������
        }

        // ��������� ��������� ����� � ������ �������� �������
        buttonsList.questTasksHolder.Add(task);
        buttonsList.activeObject = task;

        // ���������, ���������� �� ��������� quest
        if (buttonsList.questHolder == null)
        {
            buttonsList.questHolder = ScriptableObject.CreateInstance<Quest>(); // ������� ����� ��������� Quest
            buttonsList.questHolder.materialData = new List<Quest.MaterialData>(); // �������������� ������ ������
        }

        // ������� ����� ������� ������ ��� ������
        Quest.MaterialData newMaterialData = new Quest.MaterialData
        {
            name = name.text, // ����������� ��� ������
            description = description.text, // ����������� �������� ������
            questButton = instantQuestPrefab, // ����������� ��������� ������ ������
            task = task // ����������� ������
        };

        // ��������� ����� ������� � ������ ������
        buttonsList.questHolder.materialData.Add(newMaterialData);
        Debug.Log(buttonsList.questHolder.materialData);

        // ������� ������ � ����� "taskPanelPrefabParentTransform"
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        if (taskPanel != null)
        {
            // ���������� ��� �������� �������� � ������� taskPanel
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("������ � ����� 'taskPanelPrefabParentTransform' �� ������.");
        }

        // ��������� ���������� Image ���� �������� �������� questTransform
        if (questTransform != null)
        {
            foreach (Transform child in questTransform.transform)
            {
                Image imageComponent = child.GetComponent<Image>();

                if (imageComponent != null)
                {
                    imageComponent.enabled = false;
                }
                else
                {
                    Debug.LogWarning("��������� Image �� ������ �� �������: " + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("questTransform ����� null.");
        }

        // �������� ��������� Image �� ������ ��� ��������� ������
        Image imageComponent_02 = instantQuestPrefab.GetComponent<Image>();
        imageComponent_02.enabled = true;

        // ������� ������
        Destroy(prefab);
    }

    // �����, ������� ���������� ��� �������������� ������
    public void QuestButtonEdit()
    {
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == currentQuest)
            {
                // ��������� ��� � �������� ������
                element.name = name.text;
                element.description = description.text;

                // �������� ��������� TMP_Text �� ��������� ������� ������ ������
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();

                if (questButtonTextComponent != null)
                {
                    // ��������� ����� �� ������
                    questButtonTextComponent.text = element.name;
                }
                else
                {
                    Debug.LogWarning("��������� TMP_Text �� ������ �� �������� ������� questButton: " + element.questButton.name);
                }
            }
        }

        // ������ ��������� ������ ��� �������������� � �������� ������
        questButtonEdit.SetActive(false);
        questButtonCreate.SetActive(true);

        // ������� ������
        Destroy(prefab);
    }
}