using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Класс для хранения данных об объектах, которые нужно сохранить
[System.Serializable]
public class SaveData
{
    public List<ObjectData> objects = new List<ObjectData>(); // Список данных об объектах
    public float someFloatVariable; // Переменная с плавающей точкой
    public int[] someIntArray; // Массив целых чисел
    public List<QuestData> quests = new List<QuestData>(); // Список данных о квестах
}

// Класс для хранения данных о каждом объекте (например, его имени, позиции и вращении)
[System.Serializable]
public class ObjectData
{
    public string objectName; // Имя объекта
    public Vector3 position; // Позиция объекта
    public Quaternion rotation; // Вращение объекта
    public string parentName; // Имя родительского объекта (если есть)
}

// Класс для хранения данных о квестах
[System.Serializable]
public class QuestData
{
    public string questName; // Имя квеста
    public List<MaterialData> materialData; // Список материалов, связанных с квестом
}

// Класс для хранения данных о материалах квеста
[System.Serializable]
public class MaterialData
{
    public string name; // Имя материала
    public string description; // Описание материала
}

// Главный класс для сохранения и загрузки данных игры
public class SaveLoadManager : MonoBehaviour
{
    // Список префабов объектов для инстанцирования при загрузке
    public List<GameObject> prefabObjects;

    // Массив объектов, трансформации которых нужно сохранять
    public GameObject[] objectsToSave;

    // Переменные для хранения данных, которые могут быть сохранены
    public float someFloatVariable;
    public int[] someIntArray;

    // Ссылка на ваш экземпляр ScriptableObject Quest
    public Quest questInstance;

    // Путь для сохранения файла
    private string saveFilePath;

    // Ссылка на объект для управления данными в игре (например, управление базой данных объектов)
    private DatabaseManager databaseManager;

    void Start()
    {
        // Устанавливаем путь для сохранения файла
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Находим объект DatabaseManager в сцене
        databaseManager = FindObjectOfType<DatabaseManager>();

        // Загружаем данные игры при старте
        LoadGame();
    }

    // Метод, который вызывается при выходе из игры, чтобы сохранить данные
    void OnApplicationQuit()
    {
        SaveGame();
    }

    // Метод для сохранения данных игры
    public void SaveGame()
    {
        // Создаем объект для сохранения данных
        SaveData saveData = new SaveData();

        // Сохраняем данные о трансформации объектов из массива objectsToSave
        foreach (GameObject obj in objectsToSave)
        {
            if (obj != null)
            {
                // Создаем новый объект данных о текущем объекте
                ObjectData objectData = new ObjectData
                {
                    objectName = obj.name, // Имя объекта
                    position = obj.transform.position, // Позиция объекта
                    rotation = obj.transform.rotation, // Вращение объекта
                    parentName = obj.transform.parent != null ? obj.transform.parent.name : null // Имя родительского объекта (если есть)
                };

                // Добавляем данные об объекте в список для сохранения
                saveData.objects.Add(objectData);
            }
        }

        // Сохраняем данные о квестах, если экземпляр квеста существует
        if (questInstance != null)
        {
            QuestData questData = new QuestData { questName = questInstance.name };

            // Добавляем материалы из квеста в данные
            foreach (var material in questInstance.materialData)
            {
                questData.materialData.Add(new MaterialData { name = material.name, description = material.description });
            }

            // Добавляем данные о квесте в список
            saveData.quests.Add(questData);
        }

        // Сохраняем другие переменные
        saveData.someFloatVariable = someFloatVariable;
        saveData.someIntArray = someIntArray;

        // Преобразуем данные в формат JSON
        string json = JsonUtility.ToJson(saveData, true);

        // Записываем данные в файл
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Игра сохранена!");
    }

    // Метод для загрузки данных игры
    public void LoadGame()
    {
        // Проверяем, существует ли файл с сохранением
        if (File.Exists(saveFilePath))
        {
            // Читаем данные из файла
            string json = File.ReadAllText(saveFilePath);

            // Преобразуем JSON в объект SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // Очищаем текущие префабы
            databaseManager.ClearPrefabs();

            // Инстанцируем объекты на основе данных из сохранения
            foreach (ObjectData objectData in saveData.objects)
            {
                // Ищем префаб по имени объекта
                GameObject prefab = prefabObjects.Find(p => p.name == objectData.objectName);

                if (prefab != null)
                {
                    // Находим родительский объект (если существует)
                    GameObject parentObject = GameObject.Find(objectData.parentName);

                    // Создаем объект с сохраненной позицией и вращением
                    GameObject obj = Instantiate(prefab, objectData.position, objectData.rotation);

                    // Если найден родительский объект, устанавливаем родителя
                    if (parentObject != null)
                    {
                        obj.transform.SetParent(parentObject.transform);
                    }

                    // Присваиваем тег для сохраненных объектов
                    obj.tag = "Saveable";

                    // Добавляем объект в базу данных
                    databaseManager.AddPrefab(obj);
                    Debug.Log($"Создан объект {obj.name} на позиции {objectData.position} с вращением {objectData.rotation} под родителем {objectData.parentName}");
                }
                else
                {
                    Debug.LogWarning($"Не найден префаб для {objectData.objectName}");
                }
            }
        }
    }
}