using System.Collections;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine() {
        while (line.endColor.a > 0) {
            Color c = line.endColor;
            line.endColor = new Color(c.r, c.g, c.b, Mathf.Clamp01(c.a-Time.deltaTime));
            yield return null;
        }
        Destroy(gameObject);
    }

}
