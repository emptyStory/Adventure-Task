using UnityEngine;
using System;
using TMPro;
using static Task;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

public class TaskPanelManager : MonoBehaviour
{
    // Массив для хранения всех объектов, которые являются родителями для кнопок
    private GameObject[] taskPanelPrefabParents;

    // Переменная для доступа к списку кнопок
    private DatabaseManager buttonsList;
    private GameObject buttonsListGameObject;

    // Переменные для ввода данных о задаче (название, описание и таймер)
    [SerializeField] private Task task;
    public new TMP_Text name; // Поле для ввода имени задачи
    public TMP_Text description; // Поле для ввода описания задачи

    public GameObject prefab;
    public GameObject taskButton;

    public GameObject currentTask; // Текущая задача, которая редактируется

    public GameObject taskButtonCreate; // Кнопка для создания задачи
    public GameObject taskButtonEdit; // Кнопка для редактирования задачи

    private void Start()
    {
        // Ищем все объекты с тегом "taskPanelPrefabParentTransform"
        taskPanelPrefabParents = GameObject.FindGameObjectsWithTag("taskPanelPrefabParentTransform");

        // Получаем объект SceneManager и доступ к базе данных кнопок
        buttonsListGameObject = GameObject.FindGameObjectWithTag("SceneManager");
        buttonsList = buttonsListGameObject.GetComponent<DatabaseManager>();
    }

    // Метод для создания новой задачи
    public void TaskButtonCreate()
    {
        GameObject taskTransform;
        // Находим объект с тегом "taskPanelPrefabParentTransform" для добавления кнопки
        taskTransform = GameObject.FindGameObjectWithTag("taskPanelPrefabParentTransform");

        // Создаем новый экземпляр кнопки задачи и устанавливаем её в родительский объект
        GameObject instantTaskPrefab = Instantiate(taskButton, transform.position, transform.rotation);
        instantTaskPrefab.transform.SetParent(taskTransform.transform);
        instantTaskPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPrefab.transform.localScale = new Vector3(1, 1, 1);

        // Создаем новый объект данных для задачи
        Task.MaterialData newMaterialData = new Task.MaterialData
        {
            name = name.text, // Имя задачи
            description = description.text, // Описание задачи
            questButton = instantTaskPrefab // Кнопка для задачи
        };

        // Добавляем новую задачу в список
        buttonsList.activeObject.materialData.Add(newMaterialData);

        // Удаляем префаб
        Destroy(prefab);
    }

    // Метод для редактирования существующей задачи
    public void TaskButtonEdit()
    {
        // Ищем задачу в списке и обновляем её данные
        foreach (var element in buttonsList.taskHolder.materialData)
        {
            if (element.questButton == currentTask)
            {
                element.name = name.text;
                element.description = description.text;

                // Обновляем текст на кнопке
                TMP_Text questButtonTextComponent = element.questButton.GetComponentInChildren<TMP_Text>();
                if (questButtonTextComponent != null)
                {
                    questButtonTextComponent.text = element.name; // Обновляем текст кнопки
                }
                else
                {
                    Debug.LogWarning("Компонент TMP_Text не найден на дочернем объекте questButton: " + element.questButton.name);
                }
            }
        }

        // Меняем видимость кнопок для редактирования и создания задачи
        taskButtonEdit.SetActive(false);
        taskButtonCreate.SetActive(true);

        // Удаляем префаб
        Destroy(prefab);
    }
}