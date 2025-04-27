using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp_Manager : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private RectTransform targetObject; // ������ �� RectTransform �������
    [SerializeField] private float animationSpeed = 1f; // �������� ��������
    [SerializeField] private float sizeAmplitude = 0.2f; // ��������� ��������� ������� (20% �� ���������)
    [SerializeField] private GameObject parentObject; // ������ �� ������������ ������ ��� �����������

    private Vector2 originalSize; // �������� ������ �������
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (targetObject != null)
        {
            originalSize = targetObject.sizeDelta;
        }
        else
        {
            Debug.LogWarning("Target object is not assigned in LevelUp_Manager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null) return;

        // ����������� ������ � ������ ��������
        timer += Time.deltaTime * animationSpeed;

        // ��������� ������� �� ��������� (�������� �� -1 �� 1)
        float sinValue = Mathf.Sin(timer);

        // ����������� � �������� �� (1 - amplitude) �� (1 + amplitude)
        float scaleFactor = 1f + sinValue * sizeAmplitude;

        // ��������� ����� ������
        targetObject.sizeDelta = originalSize * scaleFactor;
    }

    // ����� ��� ��������� ������ �������� �������
    public void SetTargetObject(RectTransform newTarget)
    {
        // ���������� ������ ����������� �������, ���� �� ���
        if (targetObject != null)
        {
            targetObject.sizeDelta = originalSize;
        }

        targetObject = newTarget;
        originalSize = newTarget.sizeDelta;
        timer = 0f;
    }

    // ����� ��� �������� � ����������� ������������� �������
    public void Close()
    {
        if (parentObject != null && parentObject.activeInHierarchy)
        {
            Destroy(parentObject);
            // ��� ���� ������ ������ ������, � �� ����������:
            // parentObject.SetActive(false);
        }
    }
}