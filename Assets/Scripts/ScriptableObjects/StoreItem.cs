using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "StoreItem")]
//  ласс Quest представл€ет собой задание в игре, которое может содержать несколько материалов и задачи.
public class StoreItem : ScriptableObject
{
    // —писок, в котором хран€тс€ все материалы, св€занные с этим заданием
    public List<StoreItemsData> storeItemsData = new List<StoreItemsData>();

    //  ласс, представл€ющий материал, св€занный с заданием
    [System.Serializable]
    public class StoreItemsData
    {
        // Ќазвание материала
        public string itemName;

        // ќписание материала
        public string itemDescription;

        //  арточа товара
        public Sprite itemImage;

    }
}