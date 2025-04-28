using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp_Manager : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private List<RectTransform> targetObjects = new List<RectTransform>(); // ������ �������� ��� ��������
    [SerializeField] private float animationSpeed = 1f; // �������� ��������
    [SerializeField] private float sizeAmplitude = 0.2f; // ��������� ��������� ������� (20% �� ���������)
    [SerializeField] private GameObject parentObject; // ������ �� ������������ ������ ��� �����������

    private Dictionary<RectTransform, Vector2> originalSizes = new Dictionary<RectTransform, Vector2>(); // ������� ��� �������� �������� ��������
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // �������������� ������������ ������� ��� ���� ��������
        foreach (var target in targetObjects)
        {
            if (target != null)
            {
                originalSizes[target] = target.sizeDelta;
            }
            else
            {
                Debug.LogWarning("One of target objects is null in LevelUp_Manager!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObjects.Count == 0) return;

        // ����������� ������ � ������ ��������
        timer += Time.deltaTime * animationSpeed;

        // ��������� ������� �� ��������� (�������� �� -1 �� 1)
        float sinValue = Mathf.Sin(timer);

        // ����������� � �������� �� (1 - amplitude) �� (1 + amplitude)
        float scaleFactor = 1f + sinValue * sizeAmplitude;

        // ��������� ����� ������ �� ���� ��������
        foreach (var target in targetObjects)
        {
            if (target != null && originalSizes.ContainsKey(target))
            {
                target.sizeDelta = originalSizes[target] * scaleFactor;
            }
        }
    }

    // ����� ��� ���������� ������ �������� �������
    public void AddTargetObject(RectTransform newTarget)
    {
        if (newTarget == null) return;

        if (!targetObjects.Contains(newTarget))
        {
            targetObjects.Add(newTarget);
            originalSizes[newTarget] = newTarget.sizeDelta;
        }
    }

    // ����� ��� �������� �������� �������
    public void RemoveTargetObject(RectTransform targetToRemove)
    {
        if (targetToRemove == null) return;

        if (targetObjects.Contains(targetToRemove))
        {
            // ��������������� ������������ ������ ����� ���������
            if (originalSizes.ContainsKey(targetToRemove))
            {
                targetToRemove.sizeDelta = originalSizes[targetToRemove];
                originalSizes.Remove(targetToRemove);
            }
            targetObjects.Remove(targetToRemove);
        }
    }

    // ����� ��� ������� ���� ������� ��������
    public void ClearAllTargets()
    {
        // ��������������� ������������ ������� ����� ��������
        foreach (var target in targetObjects)
        {
            if (target != null && originalSizes.ContainsKey(target))
            {
                target.sizeDelta = originalSizes[target];
            }
        }

        targetObjects.Clear();
        originalSizes.Clear();
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