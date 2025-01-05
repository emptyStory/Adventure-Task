using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static StoreItem;

public class ButtonHandler : MonoBehaviour
{
    // ������ �� ��� ScriptableObject
    public StoreItem storeItem;

    // ��������� ���������� ��� ����������� ���������� � ������
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemImage;

    // ������� ��� ����������� ������ �� �������
    public void ShowItem(int index)
    {
        if (storeItem.storeItemsData.Count > index && index >= 0)
        {
            StoreItem.StoreItemsData itemData = storeItem.storeItemsData[index];

            // ��������� ��������� ���� � �����������
            itemNameText.text = itemData.itemName;
            itemDescriptionText.text = itemData.itemDescription;
            itemImage.sprite = itemData.itemImage; // �������� �� sprite
        }
        else
        {
            Debug.LogWarning("������ ������ ��� ���������.");
        }
    }

    private void Start()
    {
        // ���������� ������ ������� ��� ������
        ShowItem(0);
    }
}