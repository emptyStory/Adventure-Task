using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task")]
// Этот класс представляет собой задание в игре, которое может содержать различные материалы и соответствующие кнопки
public class Task : ScriptableObject
{
    // Список материалов, связанных с заданием
    public List<MaterialData> materialData = new List<MaterialData>();

    // Класс, представляющий материал, связанный с заданием
    [System.Serializable]
    public class MaterialData
    {
        // Название материала
        public string name;

        // Описание материала
        public string description;

        // Кнопка, которая будет отображаться в интерфейсе при взаимодействии с этим материалом
        public GameObject questButton;
    }
}
