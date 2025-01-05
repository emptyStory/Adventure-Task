using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static StoreItem;

public class ButtonHandler : MonoBehaviour
{
    // Ссылка на ваш ScriptableObject
    public StoreItem storeItem;

    // Публичные переменные для отображения информации о товаре
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemImage;

    // Функция для отображения товара по индексу
    public void ShowItem(int index)
    {
        if (storeItem.storeItemsData.Count > index && index >= 0)
        {
            StoreItem.StoreItemsData itemData = storeItem.storeItemsData[index];

            // Обновляем текстовые поля и изображение
            itemNameText.text = itemData.itemName;
            itemDescriptionText.text = itemData.itemDescription;
            itemImage.sprite = itemData.itemImage; // Изменено на sprite
        }
        else
        {
            Debug.LogWarning("Индекс товара вне диапазона.");
        }
    }

    private void Start()
    {
        // Отображаем первый элемент при старте
        ShowItem(0);
    }
}