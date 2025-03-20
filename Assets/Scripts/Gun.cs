using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bombPrefab;

    public float throwVelocity;

    public bool carryPlayerSpeed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            ThrowABomb();
        }
    }

    void ThrowABomb() {
        Vector3 rbVelocityAdd = Vector3.zero;
        if (carryPlayerSpeed && GetComponent<CharacterController>()) {
            rbVelocityAdd = GetComponent<CharacterController>().velocity;
        }
        
        GameObject bomb = Instantiate(bombPrefab, Camera.main.transform.position + 
                    Camera.main.transform.forward * 2f, 
                    Camera.main.transform.rotation);
        bomb.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward.normalized * throwVelocity + rbVelocityAdd;
    }
}
