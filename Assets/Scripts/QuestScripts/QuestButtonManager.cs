using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButtonManager : MonoBehaviour
{
    // ��������� ������ �� �������� ����������
    public Image questButtonActiveColor; // ���� �������� ������
    public TMP_Text questButtonText; // ����� �� ������

    // ����� ��������� ���������� ��� ������������ ��������
    public float destroyDelayDuration = 1.0f;

    // ������ �� �������, ����������� �������
    private DatabaseManager buttonsList; // ��������, ������� ��������� ����� ��������
    private GameObject buttonsListGameObject;

    private GameObject questPanelManagerGameObject; // ������ ���������� ������� �������
    private QuestPanelManager currentQuest; // ������� �����

    private GameObject questButtonCreate; // ������ ��� �������� ������
    private GameObject questButtonEdit; // ������ ��� �������������� ������

    public GameObject questButton; // ������ ������
    public RectTransform buttonRect; // ������ �� RectTransform ������

    // ����� � ������� ��� ������������ �������������� � �������
    private bool isButtonPressed = false; // ���� ��� ������������ �������
    private bool isButtonHeld = false; // ���� ��� ������������ ��������� ������
    private float holdTime = 1.0f; // ����� ��������� ������ ��� ��������� ��������
    private float currentHoldTime = 0f; // ������� ������ ���������

    // ��������� ���� ��� ����������� ������
    private TMP_Text nameField; // ���� ��� �����
    private TMP_Text descriptionField; // ���� ��� ��������

    // ������ ��� �������� ������ �����
    public GameObject taskButtonPrefab;

    void Start()
    {
        // �������� ������ �� ������, ����������� ��������
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();

        // �������������� ����� ������ � ��������� ������
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                questButtonText.text = element.name; // ������������� ����� �� ������
            }
        }

        // �������� ������ ������ � ��������� � � ������
        AdjustButtonHeightToText();
    }

    // ����� ����� ��� �������� ������ ������ ��� �����
    private void AdjustButtonHeightToText()
    {
        if (questButtonText != null && buttonRect != null)
        {
            // �������� ���������������� ������ ������ (�������� ������� �����)
            float textHeight = questButtonText.preferredHeight;

            // ������������� ����� ������ ��� RectTransform ������
            buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
        }
        else
        {
            Debug.LogWarning("QuestButtonText ��� ButtonRect �� ��������� � ����������!");
        }
    }

    void Update()
    {
        // �������� �� ������� ������
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ���� ������� ��������
            if (touch.phase == TouchPhase.Began)
            {
                // ���������, �������� �� ������� �� ������
                if (IsTouchingButton(touch.position))
                {
                    isButtonPressed = true;
                    currentHoldTime = 0f; // ���������� ������ ���������
                }
            }

            // ���� ������� ������������
            if (touch.phase == TouchPhase.Stationary && isButtonPressed)
            {
                currentHoldTime += Time.deltaTime; // ����������� ������ ���������

                if (currentHoldTime >= holdTime && !isButtonHeld)
                {
                    isButtonHeld = true; // ������������� ���� ���������
                    OnButtonHold(); // �������� ���������� ���������
                }
            }

            // ���� ������� �����������
            if (touch.phase == TouchPhase.Ended)
            {
                if (isButtonPressed)
                {
                    if (!isButtonHeld) // ���� �� ���� ���������
                    {
                        OnButtonDown(); // �������� ���������� �������
                    }
                    isButtonPressed = false;
                    isButtonHeld = false; // ���������� ���� ���������
                    currentHoldTime = 0f; // ���������� ������
                    OnButtonUp(); // �������� ���������� ����������
                }
            }
        }
    }

    // �������� ��������� ������� �� ������
    private bool IsTouchingButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition);
    }

    // ��������� ������� ������
    private void OnButtonDown()
    {
        Debug.Log("������ ������");
        TasksLoad(); // ��������� ������
    }

    // ��������� ��������� ������
    private void OnButtonHold()
    {
        Debug.Log("������ ������������");
        LoadData(); // ��������� ������ ��� ������
    }

    // ��������� ���������� ������
    private void OnButtonUp()
    {
        Debug.Log("������ ��������");
    }

    // ����� ��� �������� ������ ������
    public void LoadData()
    {
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                // ������� ������ � ������� ������
                GameObject instantQuestPanelPrefab = Instantiate(buttonsList.questPanelPrefab, transform.position, transform.rotation);
                instantQuestPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantQuestPanelPrefab.transform.localPosition = Vector2.zero;
                instantQuestPanelPrefab.transform.localScale = Vector3.one;

                FindTextFields(); // ���� ���� ��� ����� � ��������

                // ��������� ��������� ���� ������� �� �������� ������
                if (nameField != null)
                {
                    nameField.text = element.name;
                }

                if (descriptionField != null)
                {
                    descriptionField.text = element.description;
                }

                // ������������� ������ �������������� � �������� ������
                questButtonCreate = GameObject.FindGameObjectWithTag("QuestButtonCreate");
                questButtonEdit = GameObject.FindGameObjectWithTag("QuestButtonEdit");

                questPanelManagerGameObject = GameObject.FindGameObjectWithTag("QuestPanelManager");
                currentQuest = questPanelManagerGameObject.GetComponent<QuestPanelManager>();

                currentQuest.currentQuest = questButton; // ������������� ������� �����

                // �������� ��� ����������� ���������� �� ������ ��������������
                var components = questButtonEdit.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is MonoBehaviour monoBehaviour && !monoBehaviour.enabled)
                    {
                        monoBehaviour.enabled = true;
                    }
                }

                questButtonCreate.SetActive(false); // ��������� ������ �������� ������

                Debug.Log(element.name);
                Debug.Log(element.description);
            }
        }
    }

    // ����� ��� �������� ����� � ������ �����
    public void TasksLoad()
    {
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");
        GameObject questPanel = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");
        DragObject dragObject = FindObjectOfType<DragObject>();

        if (taskPanel != null)
        {
            // ������� ��� �������� �������� ������ �����
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // ��������� ������ ��� ���������������� ������
            foreach (var element in buttonsList.questHolder.materialData)
            {
                if (element.questButton == questButton)
                {
                    Task task = element.task;
                    if (task != null)
                    {
                        foreach (var taskElement in task.materialData)
                        {
                            if (taskElement != null)
                            {
                                GameObject instantiatedButton = Instantiate(taskButtonPrefab, taskPanel.transform);
                                taskElement.questButton = instantiatedButton; // ����������� ������ ������
                            }
                        }
                    }
                    buttonsList.activeObject = element.task;
                }
            }
        }
        else
        {
            Debug.LogWarning("������ � ����� 'taskPanelPrefabParentTransform' �� ������.");
        }

        // ��������� ���������� ���������� ������, ���� ��� �� �����
        if (questPanel != null)
        {
            foreach (Transform child in questPanel.transform)
            {
                GameObject childGameObject = child.gameObject;
                if (childGameObject != null)
                {
                    Image imageComponent = childGameObject.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        imageComponent.enabled = false;
                    }
                }
            }
        }
    }

    // ����� ��������� ����� ��� ����� � ��������
    private void FindTextFields()
    {
        GameObject[] nameObjects = GameObject.FindGameObjectsWithTag("Name");
        GameObject[] descriptionObjects = GameObject.FindGameObjectsWithTag("Description");

        if (nameObjects.Length > 0)
        {
            nameField = nameObjects[0].GetComponent<TMP_Text>();
        }

        if (descriptionObjects.Length > 0)
        {
            descriptionField = descriptionObjects[0].GetComponent<TMP_Text>();
        }
    }

    // �������� ������
    public void DeleteQuest()
    {
        StartCoroutine(DeleteQuestWithDelay());
    }

    private IEnumerator DeleteQuestWithDelay()
    {
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.questButton == questButton)
            {
                // ������� ��������� ������ � ��� �����
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // ������� ��� �������� �������� ������ �����
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                // ��������� �������� ����� ������������ ��������
                yield return new WaitForSeconds(destroyDelayDuration);

                // ������� ������ ������ � ��� ����� �� ������
                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
            }
        }

        // ���� ������ ��� �������, ��������� DragObject
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }

    // ���������� ������
    public void CompliteQuest()
    {
        StartCoroutine(CompleteQuestWithDelay());
    }

    private IEnumerator CompleteQuestWithDelay()
    {
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.questButton == questButton)
            {
                // ������� ��������� ������
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // ������� ��� �������� �������� ������ �����
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                // ��������� �������� ����� ������������ ��������
                yield return new WaitForSeconds(destroyDelayDuration);

                // ������� ������ ������ � ��� ����� �� ������
                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // �������� ������ ��������� � ��������� �������
                var progressController = GameObject.FindGameObjectWithTag("CharacterProgressControll").GetComponent<CharacterProgressControll>();
                progressController.AddMoneyAndExp(progressController.questMoneyIncreaser, progressController.questExpIncreaser);
            }
        }

        // ���� ������ ��� �������, ��������� DragObject
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }
}