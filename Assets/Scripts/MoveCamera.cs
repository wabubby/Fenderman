using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public Transform cameraOrientation;

    void Update()
    {
        transform.position = cameraPosition.position;
        transform.rotation = cameraOrientation.rotation;
    }
}
