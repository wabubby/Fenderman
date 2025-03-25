using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sticky : MonoBehaviour
{
    [HideInInspector]
    public bool stuck;
    Transform collidedTransform;
    Vector3 localPosition;
    Rigidbody rb;

    public LayerMask CollisionMask;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision) {
        if (!stuck && ( CollisionMask & (1 << collision.gameObject.layer)) != 0) {
            stuck = true;

            collidedTransform = collision.transform;
            localPosition = transform.position - collidedTransform.position;

            rb.isKinematic = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        
    }

    void FixedUpdate() {
        if (stuck) {
            if (collidedTransform) {

                transform.position = localPosition + collidedTransform.position;
            }
        }
    }
}