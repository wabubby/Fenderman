using UnityEngine;

[RequireComponent(typeof(Sticky))]
[RequireComponent(typeof(Exploding))]
public class StickyBomb : MonoBehaviour
{

    public float explodeDelay;
    public bool explodeOnStick;
    public Sticky sticky;
    public Exploding exploding;


    [Header("blinking")]
    public MeshRenderer mr;
    public Material blinkMaterial;
    public float blinkDelay;

    Material defaultMaterial;

    float lastBlinkTime;

    // Start is called before the first frame update
    void Start() {
        defaultMaterial = mr.material;

        if (!explodeOnStick) {
            StartCoroutine(exploding.ExplodeIn(explodeDelay));
        }
    }

    // Update is called once per frame
    void Update() {
        if (explodeOnStick && sticky.stuck && !exploding.activated) {
            StartCoroutine(exploding.ExplodeIn(explodeDelay));
            lastBlinkTime = Mathf.NegativeInfinity;
        }
        
        if (exploding.activated) {
            // blinking
            float timeUntilExplosion = explodeDelay - (Time.time - exploding.timeActivated);
            float spb = blinkDelay * timeUntilExplosion / explodeDelay;

            bool blink = (Time.time - lastBlinkTime) > spb;

            if (blink) {
                lastBlinkTime = Time.time;
                mr.material = blinkMaterial;
            } else {
                mr.material = defaultMaterial;
            }
        }
        
    }

    public void Explode() {
        exploding.Explode();
    }
}
