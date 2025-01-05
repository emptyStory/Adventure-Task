using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "StoreItem")]
// ����� Quest ������������ ����� ������� � ����, ������� ����� ��������� ��������� ���������� � ������.
public class StoreItem : ScriptableObject
{
    // ������, � ������� �������� ��� ���������, ��������� � ���� ��������
    public List<StoreItemsData> storeItemsData = new List<StoreItemsData>();

    // �����, �������������� ��������, ��������� � ��������
    [System.Serializable]
    public class StoreItemsData
    {
        // �������� ���������
        public string itemName;

        // �������� ���������
        public string itemDescription;

        // ������� ������
        public Sprite itemImage;

    }
}