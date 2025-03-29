using UnityEngine;

public class BigEnemyBehaviour : MonoBehaviour
{
    public float SpeedTimer = 1f;
    public float XSpeed = 1.2f;
    public float YSpeed = 1f;
    GameObject Player;
    Vector3 newPosition;

    float a;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //SpeedTimer += 0.01f;
        Vector3 playerVector = Player.transform.position - transform.position;
        Vector3 playerDirection = playerVector.normalized;

        if((Player.transform.position - transform.position).magnitude > 3) {
            newPosition.x = transform.position.x + playerDirection.x * XSpeed * SpeedTimer * Time.deltaTime;
            newPosition.z = transform.position.z + playerDirection.z * XSpeed * SpeedTimer * Time.deltaTime;;
        }

        float playerY = Player.transform.position.y;

        a = Mathf.Lerp(a, playerY, 1f);

        float amp = 0.5f;
        float period = 1f;
        
        newPosition.y = a + amp * Mathf.Sin(2*Mathf.PI * Time.time / period);
        

        transform.position = newPosition;
    }
}
