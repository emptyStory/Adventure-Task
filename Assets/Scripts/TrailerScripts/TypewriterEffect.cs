using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Text Settings")]
    public TMP_Text textMeshPro; // ������ �� ��������� TextMeshPro
    public float typingSpeed = 0.05f; // �������� ������ (� �������� ����� ���������)

    [TextArea(3, 10)]
    public string fullText; // ������ �����, ������� ����� ���������� ����������

    [Header("Objects To Activate")]
    public List<GameObject> objectsToEnableAfterTyping; // ������ �������� ��� ��������� ����� ������

    private string currentText = "";
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // �������, ������� ����� �������� �� ������
    public void StartTyping()
    {
        // ���� ����� ��� ����������, �� �������� ������
        if (isTyping) return;

        // ������� ���������� �����
        currentText = "";
        textMeshPro.text = currentText;

        // ��������� �������� � �������� ������
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;

        // ���������� ��������� �������
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textMeshPro.text = currentText;

            // ���� ��������� ����� ����� ����������� ���������� �������
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // ���������� ��� ������� �� ������ ����� ���������� ������
        ActivateObjects();
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToEnableAfterTyping)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("������ � ������ objectsToEnableAfterTyping �� ��������!", this);
            }
        }
    }

    // �����������: ������� ��� ���������� �������
    public void StopTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textMeshPro.text = fullText;
            isTyping = false;

            // ���������� ������� ���� ���� ������ ��������
            ActivateObjects();
        }
    }
}