using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CharacterProgressControll : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("how much to increase the money for completing the quest")]
    public int questMoneyIncreaser;
    [Tooltip("how much to increase the exp for completing the quest")]
    public int questExpIncreaser;
    [Tooltip("Duration of money increase animation in seconds")]
    public float moneyIncreaseDuration = 1f;
    [Tooltip("Duration of exp increase animation in seconds")]
    public float expIncreaseDuration = 1f;

    [Header("Level Up Effects")]
    [Tooltip("Prefab to instantiate when leveling up")]
    public GameObject levelUpPrefab;
    [Tooltip("Duration of level up prefab scale animation")]
    public float levelUpPrefabScaleDuration = 0.5f;
    [Tooltip("Offset for level up prefab position")]
    public Vector3 levelUpPrefabOffset = new Vector3(0, 1, 0);

    [Tooltip("money")]
    public int money;
    [Tooltip("exp")]
    public int exp;
    [Tooltip("current level")]
    public int level = 1;
    [Tooltip("exp required for next level")]
    public int expToNextLevel = 100;

    public Slider characterLevelSlider;
    public TMP_Text characterLevelValue;
    public TMP_Text characterMoneyValue;

    public Coroutine moneyAnimationCoroutine;
    public Coroutine expAnimationCoroutine;

    private bool isAddingExp = false;
    private int previousLevel = 1;

    void Start()
    {
        previousLevel = level;
        characterLevelSlider.minValue = 0;
        characterLevelSlider.maxValue = 1;
        characterLevelSlider.value = (float)(exp % expToNextLevel) / expToNextLevel;
        characterLevelValue.text = level.ToString();
    }

    public void AddMoneyAndExp(int moneyToAdd, int expToAdd)
    {
        if (isAddingExp) return;

        StartCoroutine(AddMoneyAndExpCoroutine(moneyToAdd, expToAdd));
    }

    private IEnumerator AddMoneyAndExpCoroutine(int moneyToAdd, int expToAdd)
    {
        isAddingExp = true;

        // Анимация денег
        int targetMoney = money + moneyToAdd;
        if (characterMoneyValue != null)
        {
            if (moneyAnimationCoroutine != null)
            {
                StopCoroutine(moneyAnimationCoroutine);
            }
            moneyAnimationCoroutine = StartCoroutine(AnimateMoneyIncrease(
                money,
                targetMoney,
                moneyIncreaseDuration,
                characterMoneyValue));
        }
        money = targetMoney;

        // Анимация опыта
        if (expAnimationCoroutine != null)
        {
            StopCoroutine(expAnimationCoroutine);
        }
        expAnimationCoroutine = StartCoroutine(AnimateExpIncrease(
            exp,
            expToAdd,
            expIncreaseDuration));

        yield return expAnimationCoroutine;
        isAddingExp = false;
    }

    private IEnumerator AnimateMoneyIncrease(int startValue, int endValue, float duration, TMP_Text moneyText)
    {
        float elapsed = 0f;
        int currentValue = startValue;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            currentValue = (int)Mathf.Lerp(startValue, endValue, progress);
            moneyText.text = currentValue.ToString();
            yield return null;
        }

        moneyText.text = endValue.ToString();
    }

    private IEnumerator AnimateExpIncrease(int currentExp, int expToAdd, float duration)
    {
        float elapsed = 0f;
        int targetExp = currentExp + expToAdd;
        int startExp = currentExp;
        int levelsGained = 0;

        // Начальное значение слайдера
        float startSliderValue = (float)(currentExp % expToNextLevel) / expToNextLevel;
        characterLevelSlider.value = startSliderValue;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            // Текущий отображаемый опыт
            int displayExp = (int)Mathf.Lerp(startExp, targetExp, progress);

            // Вычисляем сколько уровней нужно добавить
            int newLevels = (displayExp / expToNextLevel) - ((startExp + levelsGained * expToNextLevel) / expToNextLevel);

            // Если появились новые уровни
            if (newLevels > 0)
            {
                for (int i = 0; i < newLevels; i++)
                {
                    levelsGained++;
                    level++;
                    characterLevelValue.text = level.ToString();

                    // Сброс слайдера при повышении уровня
                    characterLevelSlider.value = 0f;

                    // Вызываем эффект повышения уровня
                    if (levelUpPrefab != null)
                    {
                        StartCoroutine(SpawnLevelUpPrefab());
                    }

                    yield return new WaitForSeconds(0.2f); // Небольшая задержка между повышениями уровня
                }
            }

            // Обновляем слайдер
            float currentExpInLevel = (float)(displayExp % expToNextLevel) / expToNextLevel;
            characterLevelSlider.value = currentExpInLevel;

            yield return null;
        }

        // Финальное обновление
        exp = targetExp;
        characterLevelSlider.value = (float)(exp % expToNextLevel) / expToNextLevel;
        previousLevel = level;
    }

    private IEnumerator SpawnLevelUpPrefab()
    {
        // Позиция для появления префаба (над персонажем или другим объектом)
        Vector3 spawnPosition = transform.position + levelUpPrefabOffset;

        // Инстанциируем префаб
        GameObject levelUpInstance = Instantiate(
            levelUpPrefab,
            spawnPosition,
            Quaternion.identity);

        // Сохраняем исходный масштаб
        Vector3 originalScale = levelUpInstance.transform.localScale;

        // Устанавливаем начальный масштаб (0)
        levelUpInstance.transform.localScale = Vector3.zero;

        // Анимация увеличения масштаба
        float elapsed = 0f;
        while (elapsed < levelUpPrefabScaleDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / levelUpPrefabScaleDuration);

            // Плавное увеличение масштаба
            float scaleValue = Mathf.SmoothStep(0f, 1f, progress);
            levelUpInstance.transform.localScale = originalScale * scaleValue;

            yield return null;
        }

        // Убедимся, что масштаб установлен точно в исходный
        levelUpInstance.transform.localScale = originalScale;
    }
}