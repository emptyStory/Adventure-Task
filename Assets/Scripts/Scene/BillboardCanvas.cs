using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Находим основную камеру в сцене
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Проверяем, что камера существует
        if (mainCamera != null)
        {
            // Поворачиваем Canvas так, чтобы он смотрел в сторону камеры
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}