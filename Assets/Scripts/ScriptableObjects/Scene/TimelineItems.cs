using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[CreateAssetMenu(fileName = "Timelines", menuName = "TimelineItem")]
public class TimelineItems : ScriptableObject
{
    public List<TimelineItemsData> timelineItemsData = new List<TimelineItemsData>();

    [System.Serializable]
    public class TimelineItemsData
    {
        public string animationTrigger; // Пример свойства

        public float duration; // Длительность

        public PlayableAsset playableAsset; // Новое свойство для хранения PlayableAsset

        // Список текстов, связанных с этим элементом
        public List<string> texts = new List<string>();

        public int textDuration;

    }
}
