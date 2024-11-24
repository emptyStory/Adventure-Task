using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButtonManager : MonoBehaviour
{
    public Image questButtonActiveColor;
    public TMP_Text questButtonText;

    private DatabaseManager buttonsList; //A script access variable that stores information about all buttons
    private GameObject buttonsListGameObject;

    private GameObject questPanelManagerGameObject;
    private QuestPanelManager currentQuest;

    private GameObject questButtonCreate;
    private GameObject questButtonEdit;

    public GameObject questButton;

    public RectTransform buttonRect; // Ссылка на RectTransform кнопки
    private bool isButtonPressed = false;
    private bool isButtonHeld = false; // Флаг для отслеживания удержания
    private float holdTime = 1.0f; // Время, после которого считается удержание
    private float currentHoldTime = 0f; // Текущий таймер удержания

    private TMP_Text nameField; // Ссылка на текстовое поле для имени
    private TMP_Text descriptionField; // Ссылка на текстовое поле для описания

    //Task field
    public GameObject taskButtonPrefab;

    void Start()
    {
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); //We get access to the script containing information about all the buttons

        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                questButtonText.text = element.name;
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
        TasksLoad();
    }

    private void OnButtonHold()
    {
        Debug.Log("Кнопка удерживается");
        LoadData();
        // Ваш код для обработки удерживания кнопки
    }

    private void OnButtonUp()
    {
        Debug.Log("Кнопка отпущена");
        // Ваш код для обработки отпускания кнопки
    }

    public void LoadData()
    {
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                GameObject instantQuestPanelPrefab = Instantiate(buttonsList.questPanelPrefab, transform.position, transform.rotation);
                instantQuestPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantQuestPanelPrefab.transform.localPosition = new Vector2(0, 0);
                instantQuestPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
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
                currentQuest = questPanelManagerGameObject.GetComponent<QuestPanelManager>();

                currentQuest.currentQuest = questButton;

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

                Debug.Log(element.name);
                Debug.Log(element.description);

                Debug.Log("Это пиздец товарищи, что-то где-то когда-то сломается и я ебнусь это чинить!");
            }
        }
    }

    public void TasksLoad()
    {

        // Находим объект с тегом taskPanelPrefabParentTransform
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");
        // Находим объект с тегом questPanelPrefabParentTransform
        GameObject questPanel = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");
        DragObject dragObject = FindObjectOfType<DragObject>();

        if (taskPanel != null)
        {
            // Уничтожаем все дочерние элементы
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Инстанциируем новые префабы на основе данных из scriptable object task
            foreach (var element in buttonsList.questHolder.materialData)
            {
                if (element.questButton == questButton) // Проверяем соответствие префаба
                {
                    // Получаем доступ к scriptable object
                    Task task = element.task; // Предполагаем, что у вас есть поле task в element

                    // Проверяем, что task не равен null
                    if (task != null)
                    {
                        foreach (var taskElement in task.materialData)
                        {
                            if (taskElement != null) // Проверяем, что taskElement не равен null
                            {
                                GameObject instantiatedButton = Instantiate(taskButtonPrefab, taskPanel.transform);
                                taskElement.questButton = instantiatedButton; // Присваиваем созданный объект
                            }
                        }
                    }

                    // Здесь можно настроить созданный экземпляр кнопки, например, установить текст или обработчик событий
                    // questButtonInstance.GetComponentInChildren<Text>().text = element.task.name; // Пример установки текста

                    buttonsList.activeObject = element.task;
                } 
            }
        }
        else
        {
            Debug.LogWarning("Объект с тегом 'taskPanelPrefabParentTransform' не найден.");
        }

        if (questPanel != null)
        {
            // Итерируемся по всем дочерним объектам questPanel
            foreach (Transform child in questPanel.transform)
            {
                // Получаем объект GameObject из Transform
                GameObject childGameObject = child.gameObject;

                if (childGameObject != null)
                {
                    Image imageComponent = childGameObject.GetComponent<Image>();
                    if (imageComponent != null)
                    {
                        imageComponent.enabled = false;
                    }
                    else
                    {
                        Debug.LogWarning("Компонент Image не найден на объекте child.");
                    }
                }
                else
                {
                    Debug.LogError("child равен null.");
                }
            }
        }

        dragObject.enabled = true;
        dragObject.rectTransform.anchoredPosition = new Vector2(0, dragObject.rectTransform.anchoredPosition.y);
        questButtonActiveColor.enabled = true;
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

    public void DeleteQuest()
    {
        // Находим объект с тегом taskPanelPrefabParentTransform
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.questButton == questButton)
            {
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // Уничтожаем все дочерние элементы
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
            }
        }
        if(buttonsList.questHolder.materialData.Count == 0)
        {
            // Находим объект с компонентом DragObject
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }

    public void CompliteQuest()
    {
        // Находим объект с тегом taskPanelPrefabParentTransform
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];

            if (element.questButton == questButton)
            {
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // Уничтожаем все дочерние элементы
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
            }
        }
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            // Находим объект с компонентом DragObject
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }
}

