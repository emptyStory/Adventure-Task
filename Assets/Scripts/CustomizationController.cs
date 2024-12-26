using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationController : MonoBehaviour
{
    public Image headIcon;
    public Image chestIcon;
    public Image legsIcon;
    public Image shoesIcon;

    public SkinnedMeshRenderer head;
    public SkinnedMeshRenderer chest;
    public SkinnedMeshRenderer legs;
    public SkinnedMeshRenderer shoes;

    public List<CustomizationItem> headItems;
    public List<CustomizationItem> chestItems;
    public List<CustomizationItem> legsItems;
    public List<CustomizationItem> shoesItems;

    private int currentHeadIndex = 0;
    private int currentChestIndex = 0;
    private int currentLegsIndex = 0;
    private int currentShoesIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        if (headItems.Count > 0)
        {
            headIcon.sprite = Sprite.Create(headItems[currentHeadIndex].iconTexture, new Rect(0, 0, headItems[currentHeadIndex].iconTexture.width, headItems[currentHeadIndex].iconTexture.height), new Vector2(0.5f, 0.5f));
            head.sharedMesh = headItems[currentHeadIndex].mesh;
        }

        if (chestItems.Count > 0)
        {
            chestIcon.sprite = Sprite.Create(chestItems[currentChestIndex].iconTexture, new Rect(0, 0, chestItems[currentChestIndex].iconTexture.width, chestItems[currentChestIndex].iconTexture.height), new Vector2(0.5f, 0.5f));
            chest.sharedMesh = headItems[currentHeadIndex].mesh;
        }
            
        if (legsItems.Count > 0)
        {
            legsIcon.sprite = Sprite.Create(legsItems[currentLegsIndex].iconTexture, new Rect(0, 0, legsItems[currentLegsIndex].iconTexture.width, legsItems[currentLegsIndex].iconTexture.height), new Vector2(0.5f, 0.5f));
            legs.sharedMesh = headItems[currentHeadIndex].mesh;
        }

        if (shoesItems.Count > 0)
        {
            shoesIcon.sprite = Sprite.Create(shoesItems[currentShoesIndex].iconTexture, new Rect(0, 0, shoesItems[currentShoesIndex].iconTexture.width, shoesItems[currentShoesIndex].iconTexture.height), new Vector2(0.5f, 0.5f));
            shoes.sharedMesh = headItems[currentHeadIndex].mesh;
        }
    }

    public void changeHeadItemLeft()
    {
        currentHeadIndex = (currentHeadIndex - 1 + headItems.Count) % headItems.Count;
        UpdateIcons();
    }

    public void changeHeadItemRight()
    {
        currentHeadIndex = (currentHeadIndex + 1) % headItems.Count;
        UpdateIcons();
    }

    public void changeChestItemLeft()
    {
        currentChestIndex = (currentChestIndex - 1 + chestItems.Count) % chestItems.Count;
        UpdateIcons();
    }

    public void changeChestItemRight()
    {
        currentChestIndex = (currentChestIndex + 1) % chestItems.Count;
        UpdateIcons();
    }

    public void changeLegsItemLeft()
    {
        currentLegsIndex = (currentLegsIndex - 1 + legsItems.Count) % legsItems.Count;
        UpdateIcons();
    }

    public void changeLegsItemRight()
    {
        currentLegsIndex = (currentLegsIndex + 1) % legsItems.Count;
        UpdateIcons();
    }

    public void changeShoesItemLeft()
    {
        currentShoesIndex = (currentShoesIndex - 1 + shoesItems.Count) % shoesItems.Count;
        UpdateIcons();
    }

    public void changeShoesItemRight()
    {
        currentShoesIndex = (currentShoesIndex + 1) % shoesItems.Count;
        UpdateIcons();
    }
}
