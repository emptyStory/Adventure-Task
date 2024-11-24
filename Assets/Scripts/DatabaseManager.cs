using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    public GameObject questPanelPrefab; //The prefab of the button is stored here, which implies a quest
    public GameObject taskPanelPrefab;
    public Transform parentTransform; //The position of the parent is stored here, within which quests will be instantiated

    //public List<GameObject> buttons = new List<GameObject>(); //A list that contains all the buttons that are instantiated on the stage

    public Quest questHolder;
    public Task taskHolder;

    public List<Task> questTasksHolder;
    public Task activeObject;

    public void addQuest() //This function instantiates the prefab of the button on the scene
    {
        GameObject instantQuestPanelPrefab = Instantiate(questPanelPrefab, transform.position, transform.rotation);
        instantQuestPanelPrefab.transform.SetParent(parentTransform);
        instantQuestPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantQuestPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    public void addTask() //This function instantiates the prefab of the button on the scene
    {
        GameObject instantTaskPanelPrefab = Instantiate(taskPanelPrefab, transform.position, transform.rotation);
        instantTaskPanelPrefab.transform.SetParent(parentTransform);
        instantTaskPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    public void AddPrefab(GameObject prefab)
    {
        //buttons.Add(prefab);
    }

    public void ClearPrefabs()
    {
        //buttons.Clear();
    }

    
}
