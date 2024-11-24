using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskButtonManager : MonoBehaviour
{
    public Image taskButtonActiveColor;
    public TMP_Text taskButtonText;

    private DatabaseManager buttonsList; //A script access variable that stores information about all buttons
    private GameObject buttonsListGameObject;

    private GameObject questPanelManagerGameObject;
    private TaskPanelManager currentTask;

    private GameObject questButtonCreate;
    private GameObject questButtonEdit;

    public GameObject questButton;

    public RectTransform buttonRect; // Ссылка на RectTransform кнопки
    private bool isButtonPressed = false;
    private bool isButtonHeld = false; // Флаг для отслеживания удержания
    private float holdTime = 0.5f; // Время, после которого считается удержание
    private float currentHoldTime = 0f; // Текущий таймер удержания

    private TMP_Text nameField; // Ссылка на текстовое поле для имени
    private TMP_Text descriptionField; // Ссылка на текстовое поле для описания

    private Quest.MaterialData currentQuest;

    void Start()
    {
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); //We get access to the script containing information about all the buttons

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
        // Проверяем, есть ли касание
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Если касание началось
            if (touch.phase == TouchPhase.Began)
            {
                // Проверяем, попадает ли касание на кнопку
                if (IsTouchingButton(touch.position))
                {
                    isButtonPressed = true;
                    currentHoldTime = 0f; // Сбрасываем таймер удержания
                }
            }

            // Если касание продолжается
            if (touch.phase == TouchPhase.Stationary && isButtonPressed)
            {
                currentHoldTime += Time.deltaTime; // Увеличиваем таймер удержания

                if (currentHoldTime >= holdTime && !isButtonHeld)
                {
                    isButtonHeld = true; // Устанавливаем флаг удержания
                    OnButtonHold(); // Вызываем метод удержания только один раз
                }
            }

            // Если касание закончилось
            if (touch.phase == TouchPhase.Ended)
            {
                if (isButtonPressed)
                {
                    if (!isButtonHeld) // Проверяем, было ли удержание
                    {
                        OnButtonDown(); // Вызываем метод нажатия только если не было удержания
                    }
                    isButtonPressed = false;
                    isButtonHeld = false; // Сбрасываем флаг удержания
                    currentHoldTime = 0f; // Сбрасываем таймер
                    OnButtonUp();
                }
            }
        }
    }

    private bool IsTouchingButton(Vector2 touchPosition)
    {
        // Проверяем, попадает ли позиция касания на кнопку
        return RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition);
    }

    private void OnButtonDown()
    {
        Debug.Log("Кнопка нажата");
        // Ваш код для обработки нажатия кнопки
    }

    private void OnButtonHold()
    {
        Debug.Log("Кнопка удерживается");
        LoadTaskData();
        // Ваш код для обработки удерживания кнопки
    }

    private void OnButtonUp()
    {
        Debug.Log("Кнопка отпущена");
        // Ваш код для обработки отпускания кнопки
    }

    public void LoadTaskData()
    {
        foreach (var element in buttonsList.activeObject.materialData)
        {
            if (element.questButton == questButton)
            {
                GameObject instantTaskPanelPrefab = Instantiate(buttonsList.taskPanelPrefab, transform.position, transform.rotation);
                instantTaskPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantTaskPanelPrefab.transform.localPosition = new Vector2(0, 0);
                instantTaskPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
                FindTextFields();

                if (nameField != null)
                {
                    nameField.text = element.name;
                }

                if (descriptionField != null)
                {
                    descriptionField.text = element.description;
                }

                questButtonCreate = GameObject.FindGameObjectWithTag("QuestButtonCreate");
                questButtonEdit = GameObject.FindGameObjectWithTag("QuestButtonEdit");

                questPanelManagerGameObject = GameObject.FindGameObjectWithTag("QuestPanelManager");
                currentTask = questPanelManagerGameObject.GetComponent<TaskPanelManager>();

                currentTask.currentTask = questButton;

                var components = questButtonEdit.GetComponents<Component>();

                // Включаем все выключенные компоненты
                foreach (var component in components)
                {
                    // Проверяем, является ли компонент MonoBehaviour и выключен ли он
                    if (component is MonoBehaviour monoBehaviour && !monoBehaviour.enabled)
                    {
                        monoBehaviour.enabled = true;
                    }
                }

                questButtonCreate.SetActive(false);
                //questButtonEdit.SetActive(true);



                Debug.Log(element.name);
                Debug.Log(element.description);

                Debug.Log("Это пиздец товарищи, что-то где-то когда-то сломается и я ебнусь это чинить!");
            }
        }
    }

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

    public void DeleteTask()
    {

        for (int i = buttonsList.activeObject.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.activeObject.materialData[i];
            if (element.questButton == questButton)
            {
                Destroy(element.questButton);
                buttonsList.activeObject.materialData.RemoveAt(i);
            }
        }

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // Находим объект с компонентом DragObject
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;
                Destroy(buttonsList.activeObject);
            }
        }
    }

    public void CompliteTask()
    {

        for (int i = buttonsList.activeObject.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.activeObject.materialData[i];
            if (element.questButton == questButton)
            {
                Destroy(element.questButton);
                buttonsList.activeObject.materialData.RemoveAt(i);
            }
        }

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // Находим объект с компонентом DragObject
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;
                Destroy(buttonsList.activeObject);
            }
        }
    }
}

