using System;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    // �����, ���������� �� ���������� �������
    private float timeRemaining;

    // �����, ����� ��� ������� ������
    private DateTime timerStartTime;

    // �������� ��� ��������� ����������� �������
    public float TimeRemaining => timeRemaining;

    // �������, ������� ����� ������� ��� ���������� �������
    public event Action TimerFinished;

    // ������, ������� ����� ������ �� ���������� �������
    public GameObject timerFinishedPrefab;

    // ������������ ������, � ������� ����� �������� ������ �� ����������
    public Transform parentTransform;

    // ����� ��� ��������� ������� �������
    public void SetTime(int hours, int minutes)
    {
        timeRemaining = (hours * 3600) + (minutes * 60); // ����������� ����� � �������
        timerStartTime = DateTime.Now; // ���������� ����� ������
    }

    // ����� ��� ������ ������� �������
    public void StartCountdown(MonoBehaviour caller)
    {
        // ��������� �������� ������� �������
        StartCoroutine(CountdownCoroutine());
    }

    // ������� ��� ������� �������
    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            Debug.Log(timeRemaining);
        }

        TimerFinished?.Invoke();
        OnTimerFinished();
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer finished!");
        characterDamage();

        GameObject instantTaskPanelPrefab = Instantiate(timerFinishedPrefab, transform.position, transform.rotation);
        instantTaskPanelPrefab.transform.SetParent(parentTransform);
        instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
        instantTaskPanelPrefab.transform.localScale = Vector3.one;

        Destroy(gameObject);
    }

    public void characterDamage()
    {
        var characterProgressControllHolder = GameObject.FindGameObjectWithTag("CharacterProgressControll");
        var characterProgressControllScript = characterProgressControllHolder.GetComponent<CharacterProgressControll>();

        float currentSliderValue = characterProgressControllScript.characterLifeSlider.value;
        float newSliderValue = currentSliderValue - 10;

        if (newSliderValue < 0)
        {
            newSliderValue = 0;
        }

        characterProgressControllScript.characterLifeSlider.value = newSliderValue;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            CheckTimerStatus();
        }
    }

    private void CheckTimerStatus()
    {
        // �������� ������� �����
        DateTime currentTime = DateTime.Now;
        Debug.Log(currentTime);

        // ��������� ��������� ����� � ������� ������� �������
        TimeSpan elapsedTime = currentTime - timerStartTime;

        // ��������� ���������� ����� �� ��������� ����� � ��������
        timeRemaining -= (float)elapsedTime.TotalSeconds;

        // ���� ������ ����������, �������� ������� ����������
        if (timeRemaining <= 0)
        {
            timeRemaining = 0; // ������������� ���������� ����� � 0
            TimerFinished?.Invoke();
            OnTimerFinished();
        }
        else
        {
            // ��������� ����� ������ �� ������� (��� ���������� ������)
            timerStartTime = DateTime.Now;
        }
    }
}