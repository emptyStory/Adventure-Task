using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task")]
public class Task : ScriptableObject
{
    public List<MaterialData> materialData = new List<MaterialData>(); // Используем список вместо массива

    [System.Serializable]
    public class MaterialData
    {
        public string name;
        public string description;

        public GameObject questButton;
    }
}