using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavoir : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public LayerMask groundMask;
    public LayerMask playerMask;

    GameObject Player;
    Vector3 NewEnemyPos;
    bool OnGround;
    bool TouchingPlayer;
    quaternion FourNum;
    Rigidbody rigidbody;
    
    void Start() {
        Player = GameObject.Find("Player");
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 playerVector = Player.transform.position - transform.position;
        Vector3 playerDirection = playerVector.normalized;
        float distanceToPlayer = playerVector.magnitude;

        OnGround = Physics.CheckSphere(transform.position, .5f, groundMask);
        TouchingPlayer = Physics.CheckSphere(transform.position, 0.6f, playerMask);

        // transform.LookAt(Player.transform);
        if(distanceToPlayer > 2 && OnGround) {
            rigidbody.MovePosition(transform.position + playerDirection * speed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            NewEnemyPos.x = UnityEngine.Random.Range(-100f, 100f);
            NewEnemyPos.y = -20;
            NewEnemyPos.z = UnityEngine.Random.Range(-100f, 100f);
            transform.position = NewEnemyPos;
            transform.rotation = FourNum;
        }
        // Enemy.transform.position = UnityEngine.Vector3.MoveTowards(Enemy.transform.position, Player.transform.position, speed);
        // controller.Move((PlayerPos * Time.deltaTime).normalized * speed * PlayerDist/10);
        


    }
}
