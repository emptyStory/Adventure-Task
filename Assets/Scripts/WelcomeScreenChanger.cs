using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Добавлено пространство имен

public class WelcomeScreenChanger : MonoBehaviour
{
    // Время ожидания перед переключением сцены
    public float waitTime = 5f;

    // Имя следующей сцены
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // Запускаем корутину для переключения сцены
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        // Ждем указанное время
        yield return new WaitForSeconds(waitTime);

        // Переключаем сцену
        SceneManager.LoadScene(nextSceneName);
    }
}
