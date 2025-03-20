using UnityEngine;

public class CameraControl : MonoBehaviour
{
    const float SENSITIVITY_MULTIPLAYER = 60f;

    [SerializeField] float sensX;
    [SerializeField] float sensY;

    Camera cam;

    float mouseDX;
    float mouseDY;
    float xRotation;
    float yRotation;

    void Awake() { cam = GetComponentInChildren<Camera>(); }

    void LateUpdate() {
        DoMouseInput();

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void DoMouseInput() {
        mouseDX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        mouseDY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;

        yRotation += mouseDX * sensX * SENSITIVITY_MULTIPLAYER;
        xRotation -= mouseDY * sensY * SENSITIVITY_MULTIPLAYER;

        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
