using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // ������� �������� ������ � �����
        mainCamera = Camera.main;
    }

    void Update()
    {
        // ���������, ��� ������ ����������
        if (mainCamera != null)
        {
            // ������������ Canvas ���, ����� �� ������� � ������� ������
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}