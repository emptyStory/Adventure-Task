using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "ActionItem")]
// Класс Quest представляет собой задание в игре, которое может содержать несколько материалов и задачи.
public class ActionItem : ScriptableObject
{
    // Список, в котором хранятся все действия
    public List<ActionItemsData> actionItemsData = new List<ActionItemsData>();

    // Класс, представляющий материал, связанный с заданием
    [System.Serializable]
    public class ActionItemsData
    {
        // Название действия
        public string itemName;

        // Время ожидания на точке
        public float waitTimeAtPoint;

        // Список анимационных триггеров, связанных с этим элементом
        public List<string> animations = new List<string>();

        // Список GameObjects, связанных с этим элементом
        public List<GameObject> associatedGameObjects = new List<GameObject>();

        // Список Transforms, связанных с GameObjects этого элемента
        public List<Transform> gameObjectsPlace = new List<Transform>();

        // Список звуков, связанных с этим элементом
        public List<AudioClip> soundClips = new List<AudioClip>();

        // Список текстов, связанных с этим элементом
        public List<string> texts = new List<string>();

        public int textDuration;

    }
}