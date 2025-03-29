using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bombPrefab;
    public float throwVelocity;
    public bool carryPlayerSpeed;

    public bool FireMode;

    public GameObject HitEffectPrefab;
    public LineRenderer BulletTrailPrefab;

    public float LastShootTime;

    public PlayerController player;

    void Update()
    {
        
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) return;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) LastShootTime = 0;

        if (Time.time - LastShootTime < 0.2f) return;

        if (Input.GetMouseButton(1)) {
            ThrowABomb();
        } else {
            ShootACast();
        }
        LastShootTime = Time.time;
    }

    void ShootACast() {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo);

        if (hit) {
            if (hitInfo.collider.gameObject.TryGetComponent(out StickyBomb bomb)) {
                bomb.Explode();
            } 
            // else if (hitInfo.collider.gameObject.TryGetComponent(out SmallEnemyBehaviour enemy)) {
            //     // put your health down thing here
            // }

            // HIT EFFECTS 
            Instantiate(HitEffectPrefab, hitInfo.point, transform.rotation);
            LineRenderer line = Instantiate(BulletTrailPrefab).GetComponent<LineRenderer>();
            line.SetPosition(0, transform.position);
            line.SetPosition(1, hitInfo.point);
        }
    }

    void ThrowABomb() {
        
        Debug.Log(player.velocity);

        GameObject bomb = Instantiate(bombPrefab, Camera.main.transform.position + 
                    Camera.main.transform.forward * 2f, 
                    Camera.main.transform.rotation);
        bomb.GetComponent<Rigidbody>().linearVelocity = Camera.main.transform.forward.normalized * throwVelocity + player.velocity + Vector3.up*throwVelocity/2;
    }
}