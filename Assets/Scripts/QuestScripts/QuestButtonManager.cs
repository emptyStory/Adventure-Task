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

    //�������� ������ ��� �������, ������� ������� ������������ ������ � ����
    private bool questIsCompleted = false;

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

            // ���� ����� �������� �������, ����� ������������:
            // float padding = 10f;
            // buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight + padding);
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
        // �������������� ��������� ������� ������ (���� ���������)
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

        dragObject.enabled = true; // �������� ��������� DragObject
        dragObject.rectTransform.anchoredPosition = new Vector2(0, dragObject.rectTransform.anchoredPosition.y);
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

                // ������� ������ ������ � ��� ����� �� ������
                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
                questIsCompleted = true;
                MoneyAndExpirienceAdd();
            }
        }

        // ���� ������ ��� �������, ��������� DragObject
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }

    public void MoneyAndExpirienceAdd()
    {
        var characterProgressControllHolder = GameObject.FindGameObjectWithTag("CharacterProgressControll");
        var characterProgressControllScript = characterProgressControllHolder.GetComponent<CharacterProgressControll>();

        float expIncrease = 0;

        

        if (questIsCompleted)
        {
            characterProgressControllScript.money += characterProgressControllScript.questMoneyIncreaser;
            characterProgressControllScript.exp += characterProgressControllScript.questExpIncreaser;
            //expIncrease = characterProgressControllScript.questExpIncreaser / 100f; // ����� �� 100 ��� ����������� ����������
        }

        if (questIsCompleted)
        {
            expIncrease = characterProgressControllScript.exp / 100f; //����� �� 100 ��� ����������� ����������
        }

        // ����������� �������� ��������
        characterProgressControllScript.characterLevelSlider.value += expIncrease;

        // ���������, �� ��������� �� ������� ������������ ��������
        if (characterProgressControllScript.characterLevelSlider.value >= 1f)
        {
            // ��������� �������
            float overflow = characterProgressControllScript.characterLevelSlider.value - 1f;

            // ����������� ������� ���������
            //characterProgressControllScript.exp += 1; // ����������� ���� �� 1, ��� ��� ������� �������������
            characterProgressControllScript.characterLevelValue.text = characterProgressControllScript.characterLevelValue.text.ToString() + 1.ToString();

            // ���������� �������
            characterProgressControllScript.characterLevelSlider.value = overflow; // ������� ����� ���������� 1
        }

        // ��������� ��������� ����
        characterProgressControllScript.characterMoneyValue.text = characterProgressControllScript.money.ToString();
    }

}