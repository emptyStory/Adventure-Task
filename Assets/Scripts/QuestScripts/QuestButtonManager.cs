using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButtonManager : MonoBehaviour
{
    // Публичные ссылки на элементы интерфейса
    public Image questButtonActiveColor; // Цвет активной кнопки
    public TMP_Text questButtonText; // Текст на кнопке

    // Новая публичная переменная для длительности задержки
    public float destroyDelayDuration = 1.0f;

    // Ссылки на объекты, управляющие данными
    private DatabaseManager buttonsList; // Сценарий, который управляет всеми кнопками
    private GameObject buttonsListGameObject;

    private GameObject questPanelManagerGameObject; // Объект управления панелью квестов
    private QuestPanelManager currentQuest; // Текущий квест

    private GameObject questButtonCreate; // Кнопка для создания квеста
    private GameObject questButtonEdit; // Кнопка для редактирования квеста

    public GameObject questButton; // Кнопка квеста
    public RectTransform buttonRect; // Ссылка на RectTransform кнопки

    // Флаги и таймеры для отслеживания взаимодействия с кнопкой
    private bool isButtonPressed = false; // Флаг для отслеживания нажатия
    private bool isButtonHeld = false; // Флаг для отслеживания удержания кнопки
    private float holdTime = 1.0f; // Время удержания кнопки для активации действия
    private float currentHoldTime = 0f; // Текущий таймер удержания

    // Текстовые поля для отображения данных
    private TMP_Text nameField; // Поле для имени
    private TMP_Text descriptionField; // Поле для описания

    // Префаб для создания кнопок задач
    public GameObject taskButtonPrefab;

    void Start()
    {
        // Получаем ссылку на объект, управляющий кнопками
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();

        // Инициализируем текст кнопки с названием квеста
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                questButtonText.text = element.name; // Устанавливаем текст на кнопку
            }
        }

        // Получаем высоту текста и применяем её к кнопке
        AdjustButtonHeightToText();
    }

    // Новый метод для подгонки высоты кнопки под текст
    private void AdjustButtonHeightToText()
    {
        if (questButtonText != null && buttonRect != null)
        {
            // Получаем предпочтительную высоту текста (учитывая перенос строк)
            float textHeight = questButtonText.preferredHeight;

            // Устанавливаем новую высоту для RectTransform кнопки
            buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textHeight);
        }
        else
        {
            Debug.LogWarning("QuestButtonText или ButtonRect не назначены в инспекторе!");
        }
    }

    void Update()
    {
        // Проверка на касание экрана
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
                    OnButtonHold(); // Вызываем обработчик удержания
                }
            }

            // Если касание завершилось
            if (touch.phase == TouchPhase.Ended)
            {
                if (isButtonPressed)
                {
                    if (!isButtonHeld) // Если не было удержания
                    {
                        OnButtonDown(); // Вызываем обработчик нажатия
                    }
                    isButtonPressed = false;
                    isButtonHeld = false; // Сбрасываем флаг удержания
                    currentHoldTime = 0f; // Сбрасываем таймер
                    OnButtonUp(); // Вызываем обработчик отпускания
                }
            }
        }
    }

    // Проверка попадания касания на кнопку
    private bool IsTouchingButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition);
    }

    // Обработка нажатия кнопки
    private void OnButtonDown()
    {
        Debug.Log("Кнопка нажата");
        TasksLoad(); // Загружаем задачи
    }

    // Обработка удержания кнопки
    private void OnButtonHold()
    {
        Debug.Log("Кнопка удерживается");
        LoadData(); // Загружаем данные для квеста
    }

    // Обработка отпускания кнопки
    private void OnButtonUp()
    {
        Debug.Log("Кнопка отпущена");
    }

    // Метод для загрузки данных квеста
    public void LoadData()
    {
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == questButton)
            {
                // Создаем панель с данными квеста
                GameObject instantQuestPanelPrefab = Instantiate(buttonsList.questPanelPrefab, transform.position, transform.rotation);
                instantQuestPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantQuestPanelPrefab.transform.localPosition = Vector2.zero;
                instantQuestPanelPrefab.transform.localScale = Vector3.one;

                FindTextFields(); // Ищем поля для имени и описания

                // Заполняем текстовые поля данными из элемента квеста
                if (nameField != null)
                {
                    nameField.text = element.name;
                }

                if (descriptionField != null)
                {
                    descriptionField.text = element.description;
                }

                // Инициализация кнопок редактирования и создания квеста
                questButtonCreate = GameObject.FindGameObjectWithTag("QuestButtonCreate");
                questButtonEdit = GameObject.FindGameObjectWithTag("QuestButtonEdit");

                questPanelManagerGameObject = GameObject.FindGameObjectWithTag("QuestPanelManager");
                currentQuest = questPanelManagerGameObject.GetComponent<QuestPanelManager>();

                currentQuest.currentQuest = questButton; // Устанавливаем текущий квест

                // Включаем все отключенные компоненты на кнопке редактирования
                var components = questButtonEdit.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is MonoBehaviour monoBehaviour && !monoBehaviour.enabled)
                    {
                        monoBehaviour.enabled = true;
                    }
                }

                questButtonCreate.SetActive(false); // Отключаем кнопку создания квеста

                Debug.Log(element.name);
                Debug.Log(element.description);
            }
        }
    }

    // Метод для загрузки задач в панель задач
    public void TasksLoad()
    {
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");
        GameObject questPanel = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");
        DragObject dragObject = FindObjectOfType<DragObject>();

        if (taskPanel != null)
        {
            // Удаляем все дочерние элементы панели задач
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Загружаем задачи для соответствующего квеста
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
                                taskElement.questButton = instantiatedButton; // Присваиваем кнопку задачи
                            }
                        }
                    }
                    buttonsList.activeObject = element.task;
                }
            }
        }
        else
        {
            Debug.LogWarning("Объект с тегом 'taskPanelPrefabParentTransform' не найден.");
        }

        // Отключаем компоненты интерфейса квеста, если они не нужны
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

    // Поиск текстовых полей для имени и описания
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

    // Удаление квеста
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
                // Удаляем связанные задачи и сам квест
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // Удаляем все дочерние элементы панели задач
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                // Добавляем задержку перед уничтожением элемента
                yield return new WaitForSeconds(destroyDelayDuration);

                // Удаляем кнопку квеста и сам квест из списка
                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
            }
        }

        // Если больше нет квестов, отключаем DragObject
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }

    // Завершение квеста
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
                // Удаляем связанные задачи
                Destroy(element.task);

                if (taskPanel != null)
                {
                    // Удаляем все дочерние элементы панели задач
                    foreach (Transform child in taskPanel.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                // Добавляем задержку перед уничтожением элемента
                yield return new WaitForSeconds(destroyDelayDuration);

                // Удаляем кнопку квеста и сам квест из списка
                Destroy(element.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // Получаем скрипт прогресса и добавляем награду
                var progressController = GameObject.FindGameObjectWithTag("CharacterProgressControll").GetComponent<CharacterProgressControll>();
                progressController.AddMoneyAndExp(progressController.questMoneyIncreaser, progressController.questExpIncreaser);
            }
        }

        // Если больше нет квестов, отключаем DragObject
        if (buttonsList.questHolder.materialData.Count == 0)
        {
            DragObject dragObject = FindObjectOfType<DragObject>();
            dragObject.enabled = false;
        }
    }
}