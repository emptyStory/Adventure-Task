using System;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    // �����, ���������� �� ���������� �������
    private float timeRemaining;

    // �������� ��� ��������� ����������� �������
    public float TimeRemaining => timeRemaining;

    // �������, ������� ����� ������� ��� ���������� �������
    public event Action TimerFinished;

    // ������, ������� ����� ������ �� ���������� �������
    public GameObject timerFinishedPrefab;

    // ������������ ������, � ������� ����� �������� ������ �� ����������
    public Transform parentTransform;

    // ����� ��� ��������� ������� �������
    // ��������� ���������� ����� � ����� � ������������ �� � �������
    public void SetTime(int hours, int minutes)
    {
        timeRemaining = (hours * 3600) + (minutes * 60); // ����������� ����� � �������
    }

    // ����� ��� ������ ������� �������
    // ��������� �������� ��� �������
    public void StartCountdown(MonoBehaviour caller)
    {
        // ��������� �������� ������� �������
        StartCoroutine(CountdownCoroutine());
    }

    // ������� ��� ������� �������
    private IEnumerator CountdownCoroutine()
    {
        // ���� ���������� ����� ������ 0
        while (timeRemaining > 0)
        {
            // ������� 1 �������
            yield return new WaitForSeconds(1f);

            // ��������� ���������� �����
            timeRemaining--;

            // ��� ������� ������� ���������� ����� � �������
            Debug.Log(timeRemaining);
        }

        // ����� ����� �����������, �������� ������� ���������� �������
        TimerFinished?.Invoke();

        // ��������� �������� ����� ���������� �������
        OnTimerFinished();
    }

    // �����, ������� ����������� �� ���������� �������
    private void OnTimerFinished()
    {
        // ������� ��������� � �������
        Debug.Log("Timer finished!");

        // ������� ������, ������� ����� ������������ �� ���������� �������
        GameObject instantTaskPanelPrefab = Instantiate(timerFinishedPrefab, transform.position, transform.rotation);

        // ������������� ������������ ������ ��� �������
        instantTaskPanelPrefab.transform.SetParent(parentTransform);

        // ������������� ������� � ������� �������
        instantTaskPanelPrefab.transform.localPosition = Vector2.zero;
        instantTaskPanelPrefab.transform.localScale = Vector3.one;

        // ���������� ������ �������, ��� ��� �� ������ �� �����
        Destroy(gameObject);
    }

    // ����� ��� ���������� ��������� ������� (���� ��������� �������)
    public void UpdateTimer()
    {
        // ���� ����� ����� ������������ ��� ���������� ��������� ������� �������, ���� �����
    }
}