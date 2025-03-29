using UnityEngine;

public class RotateDiamonds : MonoBehaviour
{
    void Update()
    {
        if (gameObject.tag == "Rotate")
        {
            transform.Rotate(0, 1, 0, Space.World);
        }
    }
}
