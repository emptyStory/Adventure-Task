using UnityEngine;
using System;
using TMPro;
using static Quest;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class QuestPanelManager : MonoBehaviour
{
    // Массив для хранения всех объектов, которые являются родителями для кнопок
    private GameObject[] questPanelPrefabParents;

    // Переменная для доступа к списку кнопок
    private DatabaseManager buttonsList;
    private GameObject buttonsListGameObject;

    // Поля для отображения названия и описания квеста
    [SerializeField] private Task task;
    public new TMP_Text name; // Поле для имени квеста
    public TMP_Text description; // Поле для описания квеста

    // Префаб кнопки квеста
    public GameObject prefab;
    public GameObject questButton;

    // Переменные для текущего квеста
    public GameObject currentQuest;
    public GameObject questButtonCreate; // Кнопка для создания нового квеста
    public GameObject questButtonEdit; // Кнопка для редактирования квеста

    private void Start()
    {
        // Ищем все объекты с тегом "questPanelPrefabParentTransform"
        questPanelPrefabParents = GameObject.FindGameObjectsWithTag("questPanelPrefabParentTransform");

        // Получаем доступ к объекту SceneManager
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>(); // Получаем доступ к скрипту базы данных кнопок
    }

    // Метод, который вызывается при нажатии кнопки "Создать квест"
    public void CreateQuestButtonPressed()
    {
        // Находим объект с тегом "questPanelPrefabParentTransform"
        GameObject questTransform = GameObject.FindGameObjectWithTag("questPanelPrefabParentTransform");

        // Создаем новый экземпляр кнопки квеста и добавляем его в родительский объект
        GameObject instantQuestPrefab = Instantiate(questButton, transform.position, transform.rotation);
        instantQuestPrefab.transform.SetParent(questTransform.transform);
        instantQuestPrefab.transform.localPosition = new Vector2(0, 0);

        // Проверяем, существует ли экземпляр task
        if (task == null)
        {
            task = ScriptableObject.CreateInstance<Task>(); // Создаем новый экземпляр Task
            task.materialData = new List<Task.MaterialData>(); // Инициализируем список данных
        }

        // Добавляем созданный квест в список активных квестов
        buttonsList.questTasksHolder.Add(task);
        buttonsList.activeObject = task;

        // Проверяем, существует ли экземпляр quest
        if (buttonsList.questHolder == null)
        {
            buttonsList.questHolder = ScriptableObject.CreateInstance<Quest>(); // Создаем новый экземпляр Quest
            buttonsList.questHolder.materialData = new List<Quest.MaterialData>(); // Инициализируем список данных
        }

        // Создаем новый элемент данных для квеста
        Quest.MaterialData newMaterialData = new Quest.MaterialData
        {
            name = name.text, // Присваиваем имя квеста
            description = description.text, // Присваиваем описание квеста
            questButton = instantQuestPrefab, // Привязываем созданную кнопку квеста
            task = task // Привязываем задачу
        };

        // Добавляем новый элемент в список данных
        buttonsList.questHolder.materialData.Add(newMaterialData);
        Debug.Log(buttonsList.questHolder.materialData);

        // Находим объект с тегом "taskPanelPrefabParentTransform"
        GameObject taskPanel = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        if (taskPanel != null)
        {
            // Уничтожаем все дочерние элементы в объекте taskPanel
            foreach (Transform child in taskPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Объект с тегом 'taskPanelPrefabParentTransform' не найден.");
        }

        // Отключаем компоненты Image всех дочерних объектов questTransform
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
                    Debug.LogWarning("Компонент Image не найден на объекте: " + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("questTransform равен null.");
        }

        // Включаем компонент Image на только что созданной кнопке
        Image imageComponent_02 = instantQuestPrefab.GetComponent<Image>();
        imageComponent_02.enabled = true;

        // Удаляем префаб
        Destroy(prefab);
    }

    // Метод, который вызывается при редактировании квеста
    public void QuestButtonEdit()
    {
        foreach (var element in buttonsList.questHolder.materialData)
        {
            if (element.questButton == currentQuest)
            {
                // Обновляем имя и описание квеста
                element.name = name.text;
                element.description = description.text;

                // Получаем компонент TMP_Text из дочернего объекта кнопки квеста
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();

                if (questButtonTextComponent != null)
                {
                    // Обновляем текст на кнопке
                    questButtonTextComponent.text = element.name;
                }
                else
                {
                    Debug.LogWarning("Компонент TMP_Text не найден на дочернем объекте questButton: " + element.questButton.name);
                }
            }
        }

        // Меняем видимость кнопок для редактирования и создания квеста
        questButtonEdit.SetActive(false);
        questButtonCreate.SetActive(true);

        // Удаляем префаб
        Destroy(prefab);
    }
}