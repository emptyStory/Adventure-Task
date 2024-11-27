using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterProgressControll : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("how much to increase the money for completing the task")]
    public int taskMoneyIncreaser;
    [Tooltip("how much to increase the exp for completing the task")]
    public int taskExpIncreaser;

    [Tooltip("how much to increase the money for completing the quest")]
    public int questMoneyIncreaser;
    [Tooltip("how much to increase the exp for completing the quest")]
    public int questExpIncreaser;

    [Tooltip("money")]
    public int money;
    [Tooltip("exp")]
    public int exp;

    public Slider characterLevelSlider;
    public TMP_Text characterLevelValue;

    public TMP_Text characterMoneyValue;
}
