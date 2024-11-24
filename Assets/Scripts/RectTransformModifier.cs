using UnityEngine;

public class RectTransformModifier : MonoBehaviour
{
    // ��������� �� RectTransform �������� �������
    private RectTransform rectTransform;

    // ��������� �� RectTransform ������� ������ ������ (���� �� ������)
    private RectTransform questPanelTransform;

    void Start()
    {
        // �������� RectTransform �������� �������
        rectTransform = GetComponent<RectTransform>();

        // ���� ������ � ����� "questPanelprefabParentTransform"
        GameObject questPanelObject = GameObject.FindGameObjectWithTag("questPanelprefabParentTransform");

        // ���������, ������ �� ������ � ������ �����
        if (questPanelObject != null)
        {
            // �������� RectTransform ���������� �������
            questPanelTransform = questPanelObject.GetComponent<RectTransform>();
        }
        else
        {
            // ���� ������ �� ������, ������� ��������������
            Debug.LogWarning("������ � ����� 'questPanelprefabParentTransform' �� ������!");
        }

        // ������������� ��������� �������� bottom (������ �����)
        SetBottom(-200);
    }

    // ����� ��� ��������� RectTransform
    public void ModifyRectTransforms()
    {
        // ������������� �������� bottom (������ �����) ��� �������� �������
        SetBottom(-200);

        // ���������, ��� �� ������ ������ � RectTransform ������ ������
        if (questPanelTransform != null)
        {
            // �������� ������� ������� ������ ������
            Vector2 newPos = questPanelTransform.anchoredPosition;

            // ������������ �������� y ������� ������, ����� ��� �� �������� �� ������� -200
            newPos.y = Mathf.Max(newPos.y + 100, -200); // ���������� 100 � ������������ ����� ��������� -200

            // ��������� ����� �������� ������� ������ ������
            questPanelTransform.anchoredPosition = newPos;
        }
    }

    // ����� ��� ��������� �������� bottom (������ �����)
    private void SetBottom(float value)
    {
        // �������� ������� ������� RectTransform
        Vector2 offset = rectTransform.offsetMin;

        // ������������� ����� �������� ��� ������� ����� (bottom)
        offset.y = value;

        // ��������� ��������� ������
        rectTransform.offsetMin = offset;
    }
}