using UnityEngine;

[CreateAssetMenu(fileName = "CustomizationItem", menuName = "Customization/Item")]
public class CustomizationItem : ScriptableObject
{
    public Texture2D iconTexture;
    public Mesh mesh;
}
