using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    // ������ ������ ��� �������� ������
    public GameObject questPanelPrefab;

    // ������ ������ ��� �������� ������
    public GameObject taskPanelPrefab;

    // ������������ ������, � ������� ����� ���������������� ������ � ������
    public Transform parentTransform;

    // ��������� ������ � �������
    public Quest questHolder;

    // ��������� ������ � �������
    public Task taskHolder;

    // ������ �����, ������� ����������� ������
    public List<Task> questTasksHolder;

    // �������� ������, � ������� ������ ���������������
    public Task activeObject;

    // ����� ��� ���������� ������ � �����
    // ������������ ������ ������ ������ � ��������� ��� � ������������ ������
    public void addQuest()
    {
        // ������� ����� ������ ��� ������ ������
        GameObject instantQuestPanelPrefab = Instantiate(questPanelPrefab, transform.position, transform.rotation);

        // ������������� ������������ ������ ��� ������
        instantQuestPanelPrefab.transform.SetParent(parentTransform);

        // ������������� ��������� ������� � ������� ��� ������
        instantQuestPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantQuestPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    // ����� ��� ���������� ������ � �����
    // ������������ ������ ������ ������ � ��������� ��� � ������������ ������
    public void addTask()
    {
        // ������� ����� ������ ��� ������ ������
        GameObject instantTaskPanelPrefab = Instantiate(taskPanelPrefab, transform.position, transform.rotation);

        // ������������� ������������ ������ ��� ������
        instantTaskPanelPrefab.transform.SetParent(parentTransform);

        // ������������� ��������� ������� � ������� ��� ������
        instantTaskPanelPrefab.transform.localPosition = new Vector2(0, 0);
        instantTaskPanelPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    // ����� ��� ���������� ������� � ������ (���� �� ������������)
    public void AddPrefab(GameObject prefab)
    {
        // ����� ����� �������� ������ � ������, ���� �����������
        //buttons.Add(prefab);
    }

    // ����� ��� ������� ������ �������� (���� �� ������������)
    public void ClearPrefabs()
    {
        // ����� ����� �������� ������ ��������, ���� �����������
        //buttons.Clear();
    }
}