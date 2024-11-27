using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskButtonManager : MonoBehaviour
{
    public Image taskButtonActiveColor;  // ������ �� �������� ���� ������ ������
    public TMP_Text taskButtonText;      // ������ �� ����� ������ ������

    private DatabaseManager buttonsList; // ���������� ��� ������� � �������, ����������� ������ � ���� �������
    private GameObject buttonsListGameObject;  // ������ �� ������ � ������ ��������

    private GameObject questPanelManagerGameObject; // ������ ���������� �������
    private TaskPanelManager currentTask;  // �������� ������� ������

    private GameObject questButtonCreate;   // ������ �������� ������
    private GameObject questButtonEdit;     // ������ �������������� ������

    public GameObject questButton;   // ������ �� ������ ������

    public RectTransform buttonRect; // ������ �� RectTransform ������
    private bool isButtonPressed = false;   // ����, ������������, ���� �� ������ ������
    private bool isButtonHeld = false;      // ����, ������������, ������������ �� ������
    private float holdTime = 0.5f;  // ����� ��������� ������
    private float currentHoldTime = 0f;  // ������� ������ ��������� ������

    private TMP_Text nameField;  // ������ �� ��������� ���� ��� �����
    private TMP_Text descriptionField;  // ������ �� ��������� ���� ��� ��������

    private Quest.MaterialData currentQuest; // ������ �������� ������


    //�������� ������ ��� �������, ������� ������� ������������ ������ � ����
    private bool taskIsCompleted = false;
    private bool questIsCompleted = false;

    void Start()
    {
        // ���� ������ � ����� "SceneManager" ��� ��������� ������� � ������ ������
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();

        // ���� �����, � �������� ����������� ��� ������, � ���������� ��� ��� �� ������
        foreach (var element in buttonsList.activeObject.materialData)
        {
            if (element.questButton == questButton)
            {
                taskButtonText.text = element.name;
            }
        }
    }

    void Update()
    {
        // �������� ������� �������
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
                currentHoldTime += Time.deltaTime;  // ����������� ������ ���������

                // ���� ����� ��������� ������, ��������� ������ ��������
                if (currentHoldTime >= holdTime && !isButtonHeld)
                {
                    isButtonHeld = true;
                    OnButtonHold();  // �������� ���������� ��������� ������
                }
            }

            // ���� ������� �����������
            if (touch.phase == TouchPhase.Ended)
            {
                if (isButtonPressed)
                {
                    if (!isButtonHeld)  // ���� ������ �� ���� ��������, �������� ���������� �������
                    {
                        OnButtonDown();
                    }
                    isButtonPressed = false;
                    isButtonHeld = false;  // ���������� ���� ���������
                    currentHoldTime = 0f;  // ���������� ������
                    OnButtonUp();  // �������� ���������� ���������� ������
                }
            }
        }
    }

    // ��������, �������� �� ������� �� ������
    private bool IsTouchingButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition);
    }

    // ����� ��� ��������� ������� ������
    private void OnButtonDown()
    {
        Debug.Log("������ ������");
        // �������������� ��� ��� ��������� ������� ������
    }

    // ����� ��� ��������� ��������� ������
    private void OnButtonHold()
    {
        Debug.Log("������ ������������");
        LoadTaskData();  // ��������� ������ ������
    }

    // ����� ��� ��������� ���������� ������
    private void OnButtonUp()
    {
        Debug.Log("������ ��������");
        // �������������� ��� ��� ��������� ���������� ������
    }

    // ����� ��� �������� ������ ������
    public void LoadTaskData()
    {
        // ���� ������ ������� � ������ �������� �������� � ��������� ������
        foreach (var element in buttonsList.activeObject.materialData)
        {
            if (element.questButton == questButton)
            {
                // ������� ������ ������ � ����������� �
                GameObject instantTaskPanelPrefab = Instantiate(buttonsList.taskPanelPrefab, transform.position, transform.rotation);
                instantTaskPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
                instantTaskPanelPrefab.transform.localScale = Vector3.one;

                // ������� ��������� ���� ��� ����� � ��������
                FindTextFields();

                if (nameField != null)
                {
                    nameField.text = element.name;  // ������������� ��� ������
                }

                if (descriptionField != null)
                {
                    descriptionField.text = element.description;  // ������������� �������� ������
                }

                // ���� ������ �������� � �������������� ������
                questButtonCreate = GameObject.FindGameObjectWithTag("QuestButtonCreate");
                questButtonEdit = GameObject.FindGameObjectWithTag("QuestButtonEdit");

                // ���� ������ � �������� � ����������� ������� ������
                questPanelManagerGameObject = GameObject.FindGameObjectWithTag("QuestPanelManager");
                currentTask = questPanelManagerGameObject.GetComponent<TaskPanelManager>();

                currentTask.currentTask = questButton;

                // �������� ��� ���������� �������������� ������
                var components = questButtonEdit.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is MonoBehaviour monoBehaviour && !monoBehaviour.enabled)
                    {
                        monoBehaviour.enabled = true;
                    }
                }

                questButtonCreate.SetActive(false);  // ��������� ������ �������� ������

                Debug.Log(element.name);
                Debug.Log(element.description);
            }
        }
    }

    // ����� ��� ������ ��������� ����� ����� � ��������
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

    // ����� ��� �������� ������
    public void DeleteTask()
    {
        // ������� ������ �� ������ �������� ��������
        for (int i = buttonsList.activeObject.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.activeObject.materialData[i];
            if (element.questButton == questButton)
            {
                Destroy(element.questButton);  // ������� ������ ������
                buttonsList.activeObject.materialData.RemoveAt(i);  // ������� ������ �� ������
            }
        }

        // ���� � ������� ��������� ������
        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];
            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            // ���� ������ ������ ���, ������� ����� � ��� ������
            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // ��������� ��������� DragObject
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;

                Destroy(buttonsList.activeObject);  // ������� ��� ������ ������
            }
        }
    }

    // ����� ��� ���������� ������
    public void CompliteTask()
    {
        // ������ ���������� ������ ����� ��, ��� � ��� ��������
        for (int i = buttonsList.activeObject.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.activeObject.materialData[i];
            if (element.questButton == questButton)
            {
                Destroy(element.questButton);
                buttonsList.activeObject.materialData.RemoveAt(i);
                taskIsCompleted = true;
                MoneyAndExpirienceAdd();
            }
        }

        // ���� � ��������� �����, ���� ������ ������ �� ����������
        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];
            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            // ���� ������ ������ ���, ��������� ����� � ������� ������
            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
                questIsCompleted = true;
                MoneyAndExpirienceAdd();

                // ������� ������ � ����������� DragObject, ����� �������� ������� � ��������� ���
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;

                // ������� ������ ������
                Destroy(buttonsList.activeObject);
            }
        }
    }

    /*
    public void MoneyAndExpirienceAdd()
    {
        var characterProgressControllHolder = GameObject.FindGameObjectWithTag("CharacterProgressControll");
        var characterProgressControllScript = characterProgressControllHolder.GetComponent<CharacterProgressControll>();

        if (taskIsCompleted)
        {
            characterProgressControllScript.money = +characterProgressControllScript.taskMoneyIncreaser;
            characterProgressControllScript.exp = +characterProgressControllScript.taskExpIncreaser;


        } else if (questIsCompleted)
        {
            characterProgressControllScript.money = +characterProgressControllScript.questMoneyIncreaser; 
            characterProgressControllScript.exp = +characterProgressControllScript.questExpIncreaser;
        }
    }
    */

    public void MoneyAndExpirienceAdd()
    {
        var characterProgressControllHolder = GameObject.FindGameObjectWithTag("CharacterProgressControll");
        var characterProgressControllScript = characterProgressControllHolder.GetComponent<CharacterProgressControll>();

        float expIncrease = 0;

        if (taskIsCompleted)
        {
            characterProgressControllScript.money += characterProgressControllScript.taskMoneyIncreaser;
            characterProgressControllScript.exp += characterProgressControllScript.taskExpIncreaser;
            //expIncrease = characterProgressControllScript.taskExpIncreaser / 100f; // ����� �� 100 ��� ����������� ����������
        }

        if (questIsCompleted)
        {
            characterProgressControllScript.money += characterProgressControllScript.questMoneyIncreaser;
            characterProgressControllScript.exp += characterProgressControllScript.questExpIncreaser;
            //expIncrease = characterProgressControllScript.questExpIncreaser / 100f; // ����� �� 100 ��� ����������� ����������
        }

        if (taskIsCompleted || questIsCompleted)
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
            characterProgressControllScript.characterLevelValue.text = characterProgressControllScript.characterLevelValue.text + 1;

            // ���������� �������
            characterProgressControllScript.characterLevelSlider.value = overflow; // ������� ����� ���������� 1
        }

        // ��������� ��������� ����
        characterProgressControllScript.characterMoneyValue.text = characterProgressControllScript.money.ToString();
    }

}