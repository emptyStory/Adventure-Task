using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StoreItem;

public class ButtonHandler : MonoBehaviour
{
    public StoreItem storeItem; // ������ �� ��� ScriptableObject
    public List<GameObject> itemButtonsList = new List<GameObject>();

    public Text itemNameText; // UI ������� ��� ����������� �����
    public Text itemDescriptionText; // UI ������� ��� ����������� ��������
    public RawImage itemImage; // UI ������� ��� ����������� �����������

    private void OnButtonClick(StoreItem.StoreItemsData item)
    {
        // ���������� ��� �������� ������ �� GameObject � �������
        GameObject buttonGameObject;
        // �������� GameObject, �� ������� ����� ���� ���������
        buttonGameObject = gameObject;

        // ��������� UI ��������
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemImage.texture = item.itemImage;
    }
}