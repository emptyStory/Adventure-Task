using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
// Класс Quest представляет собой задание в игре, которое может содержать несколько материалов и задачи.
public class Quest : ScriptableObject
{
    // Список, в котором хранятся все материалы, связанные с этим заданием
    public List<MaterialData> materialData = new List<MaterialData>();

    // Класс, представляющий материал, связанный с заданием
    [System.Serializable]
    public class MaterialData
    {
        // Название материала
        public string name;

        // Описание материала
        public string description;

        // Кнопка, которая будет отображаться в интерфейсе для этого материала
        public GameObject questButton;

        // Задача, связанная с этим материалом
        public Task task;
    }
}