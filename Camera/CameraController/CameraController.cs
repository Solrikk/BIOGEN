using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 50f;
    public Transform playerBody;
    public float smoothTime = 0.1f;

    private float xRotation = 0f;
    private float currentXRotation = 0f;
    private float rotationVelocityX;
    private float rotationVelocityY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        currentXRotation = Mathf.SmoothDamp(currentXRotation, -targetMouseY, ref rotationVelocityX, smoothTime);
        xRotation += currentXRotation;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        float smoothMouseX = Mathf.SmoothDamp(0, targetMouseX, ref rotationVelocityY, smoothTime);
        playerBody.Rotate(Vector3.up * smoothMouseX);
    }
}
