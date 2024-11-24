using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public List<ObjectData> objects = new List<ObjectData>();
    public float someFloatVariable;
    public int[] someIntArray;
    public List<QuestData> quests = new List<QuestData>();
}

[System.Serializable]
public class ObjectData
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
    public string parentName;
}

[System.Serializable]
public class QuestData
{
    public string questName;
    public List<MaterialData> materialData; // Заменили массив на список
}

[System.Serializable]
public class MaterialData
{
    public string name;
    public string description;
}

public class SaveLoadManager : MonoBehaviour
{
    public List<GameObject> prefabObjects; // Префабы объектов для инстанцирования
    public GameObject[] objectsToSave; // Объекты, трансформации которых нужно сохранять
    public float someFloatVariable;
    public int[] someIntArray;

    // Ссылка на ваш экземпляр ScriptableObject Quest
    public Quest questInstance;

    private string saveFilePath;
    private DatabaseManager databaseManager;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        databaseManager = FindObjectOfType<DatabaseManager>();
        LoadGame();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        /*
        // Сохраняем инстанцированные префабы из PrefabManager
        foreach (GameObject obj in databaseManager.buttons)
        {
            ObjectData objectData = new ObjectData
            {
                objectName = obj.name.Replace("(Clone)", ""),
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                parentName = obj.transform.parent != null ? obj.transform.parent.name : null
            };
            saveData.objects.Add(objectData);
        }
        */

        // Сохраняем трансформации объектов, указанных в objectsToSave
        foreach (GameObject obj in objectsToSave)
        {
            if (obj != null)
            {
                ObjectData objectData = new ObjectData
                {
                    objectName = obj.name,
                    position = obj.transform.position,
                    rotation = obj.transform.rotation,
                    parentName = obj.transform.parent != null ? obj.transform.parent.name : null
                };
                saveData.objects.Add(objectData);
            }
        }

        // Сохраняем данные о квестах из уже существующего экземпляра
        if (questInstance != null)
        {
            QuestData questData = new QuestData { questName = questInstance.name };
            foreach (var material in questInstance.materialData)
            {
                questData.materialData.Add(new MaterialData { name = material.name, description = material.description });
            }
            saveData.quests.Add(questData);
        }

        saveData.someFloatVariable = someFloatVariable;
        saveData.someIntArray = someIntArray;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            databaseManager.ClearPrefabs();

            foreach (ObjectData objectData in saveData.objects)
            {
                GameObject prefab = prefabObjects.Find(p => p.name == objectData.objectName);
                if (prefab != null)
                {
                    GameObject parentObject = GameObject.Find(objectData.parentName);
                    GameObject obj = Instantiate(prefab, objectData.position, objectData.rotation);

                    if (parentObject != null)
                    {
                        obj.transform.SetParent(parentObject.transform);
                    }

                    obj.tag = "Saveable";
                    databaseManager.AddPrefab(obj);
                    Debug.Log($"Instantiated {obj.name} at {objectData.position} with rotation {objectData.rotation} under parent {objectData.parentName}");
                }
                else
                {
                    Debug.LogWarning($"Prefab not found for {objectData.objectName}");
                }
            }
        }
    }
}