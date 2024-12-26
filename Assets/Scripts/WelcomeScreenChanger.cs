using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ��������� ������������ ����

public class WelcomeScreenChanger : MonoBehaviour
{
    // ����� �������� ����� ������������� �����
    public float waitTime = 5f;

    // ��� ��������� �����
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // ��������� �������� ��� ������������ �����
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        // ���� ��������� �����
        yield return new WaitForSeconds(waitTime);

        // ����������� �����
        SceneManager.LoadScene(nextSceneName);
    }
}
