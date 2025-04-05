using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;

public class ShowFPS : MonoBehaviour
{
    public static float fps;
    private int maxFPS = 60;

    [Header("Frame Settings")]

    int MaxRate = 9999;

    public float TargetFrameRate = 60.0f;

    float currentFrameTime;

    private void Awake ()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {

            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
            Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
            t = Time.realtimeSinceStartup;
        }
    }

    private void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;

        // Создаем стиль для текста
        GUIStyle style = new GUIStyle();
        style.fontSize = 120; // Увеличиваем размер шрифта в 5 раз
        style.normal.textColor = Color.white; // Цвет текста

        // Получаем размер текста
        Vector2 textSize = style.CalcSize(new GUIContent("FPS: " + (int)fps));

        // Вычисляем позицию для центровки
        float xPos = (Screen.width - textSize.x) / 2;
        float yPos = (Screen.height - textSize.y) / 2;

        // Отображаем текст по центру экрана
        GUI.Label(new Rect(xPos, yPos, textSize.x, textSize.y), "FPS: " + (int)fps, style);
    }
}