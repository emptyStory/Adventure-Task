using UnityEngine;

public class RectTransformModifier : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform questPanelTransform;

    void Start()
    {
        // �������� RectTransform �������� �������
        rectTransform = GetComponent<RectTransform>();

        // ������� ������ � ����� "questPanelprefabParentTransform"
        GameObject questPanelObject = GameObject.FindGameObjectWithTag("questPanelprefabParentTransform");
        if (questPanelObject != null)
        {
            questPanelTransform = questPanelObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("������ � ����� 'questPanelprefabParentTransform' �� ������!");
        }

        // ������������� ��������� �������� bottom
        SetBottom(-200);
    }

    public void ModifyRectTransforms()
    {
        // �������� �������� bottom �������� RectTransform
        SetBottom(-200);

        // ���������, ��� questPanelTransform ����������
        if (questPanelTransform != null)
        {
            // ������������� ����������� �� Y ��� questPanelTransform
            Vector2 newPos = questPanelTransform.anchoredPosition;
            newPos.y = Mathf.Max(newPos.y + 100, -200); // ������������ y �� -200 � ���������� 100
            questPanelTransform.anchoredPosition = newPos;
        }
    }

    private void SetBottom(float value)
    {
        // �������� �������� bottom RectTransform
        Vector2 offset = rectTransform.offsetMin; // �������� ������� �������
        offset.y = value; // ������������� ����� �������� bottom
        rectTransform.offsetMin = offset; // ��������� �������
    }
}