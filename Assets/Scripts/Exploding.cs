using UnityEngine;
using System.Collections;

public class Exploding : MonoBehaviour
{
    [HideInInspector]
    public bool activated;

    [HideInInspector]
    public float timeActivated;
    float timer;

    public GameObject explosionPrefab;

    // Update is called once per frame
    void Update() {
        if (activated) {
            timer -= Time.deltaTime;
        }
    }

    public void Explode() {
        // Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public IEnumerator ExplodeIn(float duration) {
        activated = true;
        timer = duration;

        timeActivated = Time.time;

        yield return new WaitForSeconds(duration);

        Explode();
    }
}