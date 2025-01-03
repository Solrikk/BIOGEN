using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // Скорость движения камеры

    void Update()
    {
        // Получаем оси ввода (горизонтальная/вертикальная) с клавиатуры (WASD или стрелки)
        float horizontal = Input.GetAxis("Horizontal"); // A,D или стрелки ←,→
        float vertical = Input.GetAxis("Vertical");   // W,S или стрелки ↑,↓

        // Формируем вектор направления
        Vector3 direction = new Vector3(horizontal, vertical, 0f);

        // Двигаем камеру
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
