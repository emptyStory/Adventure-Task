using UnityEngine;
using System;
using TMPro;
using static Quest;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class QuestPanelManager : MonoBehaviour
{
    private GameObject[] questPanelPrefabParents; //All objects that are the parents of the button's prefab are stored here

    private DatabaseManager buttonsList; //A script access variable that stores information about all buttons
    private GameObject buttonsListGameObject;

    [SerializeField] private Task task;

    public new TMP_Text name; //The information that the user enters in the field implying the name of the quest
    public TMP_Text description; //The information that the user enters in the field implying a description of the quest

    public GameObject prefab;
    public GameObject questButton;

    public GameObject currentQuest;

    public GameObject questButtonCreate;
    public GameObject questButtonEdit;

    private void Start()
    {
        questPanelPrefabParents = GameObject.FindGameObjectsWithTag("questPanelPrefabParentTransform"); //We will find all objects on the scene with the tag questPanelPrefabParentTransform
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); //We get access to the script containing information about all the buttons
    }

    public void CreateQuestButtonPressed() //This function describes the actions that occur after clicking the create quest button
    {
        GameObject questTransform;
        questTransform = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");
        GameObject instantQuestPrefab = Instantiate(questButton, transform.position, transform.rotation);
        instantQuestPrefab.transform.SetParent(questTransform.transform);
        instantQuestPrefab.transform.localPosition = new Vector2(0, 0);

        // ���������, ���������� �� ������� task
        if (task == null)
        {
            task = ScriptableObject.CreateInstance<Task>(); // ������� ����� �������
            task.materialData = new List<Task.MaterialData>(); // �������������� ������
        } 

        buttonsList.questTasksHolder.Add(task);
        buttonsList.activeObject = task;

        // ���������, ���������� �� ������� quest
        if (buttonsList.questHolder == null)
        {
            buttonsList.questHolder = ScriptableObject.CreateInstance<Quest>(); // ������� ����� �������
            buttonsList.questHolder.materialData = new List<Quest.MaterialData>(); // �������������� ������
        }

        // ������� ����� ������� MaterialData
        Quest.MaterialData newMaterialData = new Quest.MaterialData();
        newMaterialData.name = name.text;
        newMaterialData.description = description.text;
        newMaterialData.questButton = instantQuestPrefab;
        newMaterialData.task = task;

        // ��������� ������� � ������ materialData
        buttonsList.questHolder.materialData.Add(newMaterialData);
        Debug.Log(buttonsList.questHolder.materialData);

        // ������� ������ � ����� taskPanelPrefabParentTransform
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        if (taskPanel != null)
        {
            // ���������� ��� �������� ��������
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("������ � ����� 'taskPanelPrefabParentTransform' �� ������.");
        }

        if (questTransform != null)
        {
            // ����������� �� ���� �������� �������� questTransform
            foreach (Transform child in questTransform.transform)
            {
                // �������� ��������� Image �� ��������� �������
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

        // �������� ��������� Image �� ��������� �������
        Image imageComponent_02 = instantQuestPrefab.GetComponent<Image>();
        imageComponent_02.enabled = true;

        Destroy(prefab);
    }

    public void QuestButtonEdit() //This function describes the actions that occur after clicking the create quest button
    {
        

        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == currentQuest)
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

        questButtonEdit.SetActive(false);
        questButtonCreate.SetActive(true);

        Destroy(prefab);
    }
}
