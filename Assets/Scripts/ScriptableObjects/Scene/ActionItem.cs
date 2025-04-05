using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actions", menuName = "ActionItem")]
// ����� Quest ������������ ����� ������� � ����, ������� ����� ��������� ��������� ���������� � ������.
public class ActionItem : ScriptableObject
{
    // ������, � ������� �������� ��� ��������
    public List<ActionItemsData> actionItemsData = new List<ActionItemsData>();

    // �����, �������������� ��������, ��������� � ��������
    [System.Serializable]
    public class ActionItemsData
    {
        // �������� ��������
        public string itemName;

        // ����� �������� �� �����
        public float waitTimeAtPoint;

        // ������ ������������ ���������, ��������� � ���� ���������
        public List<string> animations = new List<string>();

        // ������ GameObjects, ��������� � ���� ���������
        public List<GameObject> associatedGameObjects = new List<GameObject>();

        // ������ Transforms, ��������� � GameObjects ����� ��������
        public List<Transform> gameObjectsPlace = new List<Transform>();

        // ������ ������, ��������� � ���� ���������
        public List<AudioClip> soundClips = new List<AudioClip>();

        // ������ �������, ��������� � ���� ���������
        public List<string> texts = new List<string>();

        public int textDuration;

    }
}