using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    private float timeRemaining;

    public float TimeRemaining => timeRemaining;

    // �������, ������� ����� ���������� ��� ���������� �������
    public event Action TimerFinished;

    public GameObject timerFinishedPrefab;
    public Transform parentTransform;

    public void SetTime(int hours, int minutes)
    {
        timeRemaining = (hours * 3600) + (minutes * 60);
    }

    public void StartCountdown(MonoBehaviour caller)
    {
        StartCoroutine(CountdownCoroutine());
        //caller.StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f); // ���� 1 �������
            timeRemaining--; // ��������� ���������� �����
            Debug.Log(timeRemaining);
        }

        // �������� ������� �� ��������� �������
        TimerFinished?.Invoke();
        OnTimerFinished();
    }

    private void OnTimerFinished()
    {
        Debug.Log("Timer finished!"); // ��������� � ������� � ���������� �������
        GameObject instantTaskPanelPrefab = Instantiate(timerFinishedPrefab, transform.position, transform.rotation);
        instantTaskPanelPrefab.transform.SetParent(parentTransform);
        instantTaskPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
        Destroy(gameObject);
    }

    public void UpdateTimer()
    {
        // ���� ����� ����� ������������, ���� ���������� �������� ��������� ������� �������
    }
}