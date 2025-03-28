using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float LifeTime = 5f;
    public float Force;
    public float Radius;


    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider collider in colliders) {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(Force, transform.position, Radius);
            rb.AddForce(Vector3.up * Force);
        }

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }

}
