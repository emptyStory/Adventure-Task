using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    // Префаб панели для создания квеста
    public GameObject questPanelPrefab;

    // Префаб панели для создания задачи
    public GameObject taskPanelPrefab;

    // Родительский объект, в который будут инстанцироваться квесты и задачи
    public Transform parentTransform;

    // Хранитель данных о квестах
    public Quest questHolder;

    // Хранитель данных о задачах
    public Task taskHolder;

    // Список задач, которые принадлежат квесту
    public List<Task> questTasksHolder;

    // Активный объект, с которым сейчас взаимодействуют
    public Task activeObject;

    // Метод для добавления квеста в сцену
    // Инстанцирует префаб панели квеста и добавляет его в родительский объект
    public void addQuest()
    {
        // Создаем новый объект для панели квеста
        GameObject instantQuestPanelPrefab = Instantiate(questPanelPrefab, transform.position, transform.rotation);

        // Устанавливаем родительский объект для панели
        instantQuestPanelPrefab.transform.SetParent(parentTransform);

        // Устанавливаем локальную позицию и масштаб для панели
        instantQuestPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantQuestPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    // Метод для добавления задачи в сцену
    // Инстанцирует префаб панели задачи и добавляет его в родительский объект
    public void addTask()
    {
        // Создаем новый объект для панели задачи
        GameObject instantTaskPanelPrefab = Instantiate(taskPanelPrefab, transform.position, transform.rotation);

        // Устанавливаем родительский объект для панели
        instantTaskPanelPrefab.transform.SetParent(parentTransform);

        // Устанавливаем локальную позицию и масштаб для панели
        instantTaskPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    // Метод для добавления префаба в список (пока не используется)
    public void AddPrefab(GameObject prefab)
    {
        // Здесь можно добавить префаб в список, если понадобится
        //buttons.Add(prefab);
    }

    // Метод для очистки списка префабов (пока не используется)
    public void ClearPrefabs()
    {
        // Здесь можно очистить список префабов, если понадобится
        //buttons.Clear();
    }
}