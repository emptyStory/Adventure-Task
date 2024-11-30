using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StoreItem;

public class ButtonHandler : MonoBehaviour
{
    public StoreItem storeItem; // Ссылка на наш ScriptableObject
    public List<GameObject> itemButtonsList = new List<GameObject>();

    public Text itemNameText; // UI элемент для отображения имени
    public Text itemDescriptionText; // UI элемент для отображения описания
    public RawImage itemImage; // UI элемент для отображения изображения

    private void OnButtonClick(StoreItem.StoreItemsData item)
    {
        // Обновляем UI элементы
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemImage.texture = item.itemImage;
    }
}