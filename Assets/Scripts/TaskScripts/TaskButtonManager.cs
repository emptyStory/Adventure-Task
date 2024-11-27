using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskButtonManager : MonoBehaviour
{
    public Image taskButtonActiveColor;  // Ссылка на активный цвет кнопки задачи
    public TMP_Text taskButtonText;      // Ссылка на текст кнопки задачи

    private DatabaseManager buttonsList; // Переменная для доступа к скрипту, содержащему данные о всех кнопках
    private GameObject buttonsListGameObject;  // Ссылка на объект с данным скриптом

    private GameObject questPanelManagerGameObject; // Панель управления квестом
    private TaskPanelManager currentTask;  // Менеджер текущей задачи

    private GameObject questButtonCreate;   // Кнопка создания квеста
    private GameObject questButtonEdit;     // Кнопка редактирования квеста

    public GameObject questButton;   // Ссылка на кнопку квеста

    public RectTransform buttonRect; // Ссылка на RectTransform кнопки
    private bool isButtonPressed = false;   // Флаг, показывающий, была ли нажата кнопка
    private bool isButtonHeld = false;      // Флаг, показывающий, удерживается ли кнопка
    private float holdTime = 0.5f;  // Время удержания кнопки
    private float currentHoldTime = 0f;  // Текущий таймер удержания кнопки

    private TMP_Text nameField;  // Ссылка на текстовое поле для имени
    private TMP_Text descriptionField;  // Ссылка на текстовое поле для описания

    private Quest.MaterialData currentQuest; // Данные текущего квеста


    //является флагом для функции, которая передаёт пользователю валюту и опыт
    private bool taskIsCompleted = false;
    private bool questIsCompleted = false;

    void Start()
    {
        // Ищем объект с тегом "SceneManager" для получения доступа к данным кнопок
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();

        // Ищем квест, к которому принадлежит эта кнопка, и отображаем его имя на кнопке
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
        // Проверка наличия касания
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
                currentHoldTime += Time.deltaTime;  // Увеличиваем таймер удержания

                // Если время удержания прошло, выполняем нужное действие
                if (currentHoldTime >= holdTime && !isButtonHeld)
                {
                    isButtonHeld = true;
                    OnButtonHold();  // Вызываем обработчик удержания кнопки
                }
            }

            // Если касание закончилось
            if (touch.phase == TouchPhase.Ended)
            {
                if (isButtonPressed)
                {
                    if (!isButtonHeld)  // Если кнопка не была удержана, вызываем обработчик нажатия
                    {
                        OnButtonDown();
                    }
                    isButtonPressed = false;
                    isButtonHeld = false;  // Сбрасываем флаг удержания
                    currentHoldTime = 0f;  // Сбрасываем таймер
                    OnButtonUp();  // Вызываем обработчик отпускания кнопки
                }
            }
        }
    }

    // Проверка, попадает ли касание на кнопку
    private bool IsTouchingButton(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition);
    }

    // Метод для обработки нажатия кнопки
    private void OnButtonDown()
    {
        Debug.Log("Кнопка нажата");
        // Дополнительный код для обработки нажатия кнопки
    }

    // Метод для обработки удержания кнопки
    private void OnButtonHold()
    {
        Debug.Log("Кнопка удерживается");
        LoadTaskData();  // Загружаем данные задачи
    }

    // Метод для обработки отпускания кнопки
    private void OnButtonUp()
    {
        Debug.Log("Кнопка отпущена");
        // Дополнительный код для обработки отпускания кнопки
    }

    // Метод для загрузки данных задачи
    public void LoadTaskData()
    {
        // Ищем нужный элемент в списке активных объектов и загружаем данные
        foreach (var element in buttonsList.activeObject.materialData)
        {
            if (element.questButton == questButton)
            {
                // Создаем панель задачи и настраиваем её
                GameObject instantTaskPanelPrefab = Instantiate(buttonsList.taskPanelPrefab, transform.position, transform.rotation);
                instantTaskPanelPrefab.transform.SetParent(buttonsList.parentTransform);
                instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
                instantTaskPanelPrefab.transform.localScale = Vector3.one;

                // Находим текстовые поля для имени и описания
                FindTextFields();

                if (nameField != null)
                {
                    nameField.text = element.name;  // Устанавливаем имя задачи
                }

                if (descriptionField != null)
                {
                    descriptionField.text = element.description;  // Устанавливаем описание задачи
                }

                // Ищем кнопки создания и редактирования квеста
                questButtonCreate = GameObject.FindGameObjectWithTag("QuestButtonCreate");
                questButtonEdit = GameObject.FindGameObjectWithTag("QuestButtonEdit");

                // Ищем панель с квестами и настраиваем текущую задачу
                questPanelManagerGameObject = GameObject.FindGameObjectWithTag("QuestPanelManager");
                currentTask = questPanelManagerGameObject.GetComponent<TaskPanelManager>();

                currentTask.currentTask = questButton;

                // Включаем все компоненты редактирования кнопки
                var components = questButtonEdit.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is MonoBehaviour monoBehaviour && !monoBehaviour.enabled)
                    {
                        monoBehaviour.enabled = true;
                    }
                }

                questButtonCreate.SetActive(false);  // Отключаем кнопку создания квеста

                Debug.Log(element.name);
                Debug.Log(element.description);
            }
        }
    }

    // Метод для поиска текстовых полей имени и описания
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

    // Метод для удаления задачи
    public void DeleteTask()
    {
        // Удаляем задачу из списка активных объектов
        for (int i = buttonsList.activeObject.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.activeObject.materialData[i];
            if (element.questButton == questButton)
            {
                Destroy(element.questButton);  // Удаляем кнопку задачи
                buttonsList.activeObject.materialData.RemoveAt(i);  // Удаляем задачу из списка
            }
        }

        // Ищем и удаляем связанные квесты
        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];
            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            // Если задачи больше нет, удаляем квест и его кнопку
            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);

                // Отключаем компонент DragObject
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;

                Destroy(buttonsList.activeObject);  // Удаляем сам объект задачи
            }
        }
    }

    // Метод для завершения задачи
    public void CompliteTask()
    {
        // Логика завершения задачи такая же, как и для удаления
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

        // Ищем и завершаем квест, если задача больше не существует
        for (int i = buttonsList.questHolder.materialData.Count - 1; i >= 0; i--)
        {
            var element = buttonsList.questHolder.materialData[i];
            if (element.task == buttonsList.activeObject)
            {
                currentQuest = element;
            }

            // Если задачи больше нет, завершает квест и удаляет кнопку
            if (buttonsList.activeObject.materialData.Count == 0 && currentQuest != null)
            {
                Destroy(currentQuest.questButton);
                buttonsList.questHolder.materialData.RemoveAt(i);
                questIsCompleted = true;
                MoneyAndExpirienceAdd();

                // Находим объект с компонентом DragObject, чтобы сбросить позицию и отключить его
                DragObject dragObject = FindObjectOfType<DragObject>();
                dragObject.rectTransform.anchoredPosition = new Vector2(1080, dragObject.rectTransform.anchoredPosition.y);
                dragObject.enabled = false;

                // Удаляем объект задачи
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
            //expIncrease = characterProgressControllScript.taskExpIncreaser / 100f; // Делим на 100 для корректного увеличения
        }

        if (questIsCompleted)
        {
            characterProgressControllScript.money += characterProgressControllScript.questMoneyIncreaser;
            characterProgressControllScript.exp += characterProgressControllScript.questExpIncreaser;
            //expIncrease = characterProgressControllScript.questExpIncreaser / 100f; // Делим на 100 для корректного увеличения
        }

        if (taskIsCompleted || questIsCompleted)
        {
            expIncrease = characterProgressControllScript.exp / 100f; //Делим на 100 для корректного увеличения
        }

        // Увеличиваем значение слайдера
        characterProgressControllScript.characterLevelSlider.value += expIncrease;

        // Проверяем, не превышает ли слайдер максимальное значение
        if (characterProgressControllScript.characterLevelSlider.value >= 1f)
        {
            // Сохраняем остаток
            float overflow = characterProgressControllScript.characterLevelSlider.value - 1f;

            // Увеличиваем уровень персонажа
            //characterProgressControllScript.exp += 1; // Увеличиваем опыт на 1, так как уровень увеличивается
            characterProgressControllScript.characterLevelValue.text = characterProgressControllScript.characterLevelValue.text + 1;

            // Сбрасываем слайдер
            characterProgressControllScript.characterLevelSlider.value = overflow; // Остаток после достижения 1
        }

        // Обновляем текстовые поля
        characterProgressControllScript.characterMoneyValue.text = characterProgressControllScript.money.ToString();
    }

}