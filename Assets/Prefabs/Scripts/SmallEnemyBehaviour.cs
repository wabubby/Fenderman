using UnityEditor;
using UnityEngine;

public class SmallEnemyBehaviour : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public LayerMask groundMask;
    public LayerMask playerMask;

    GameObject Player;
    Vector3 NewEnemyPos;
    bool OnGround;
    bool TouchingPlayer;
    Quaternion FourNum;
    new Rigidbody rigidbody;
    
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
        TouchingPlayer = Physics.CheckSphere(transform.position, transform.localScale.x * 0.5f + 0.02f, playerMask);

        // transform.LookAt(Player.transform);
        if(distanceToPlayer > 2 && OnGround) {
            rigidbody.MovePosition(transform.position + playerDirection * speed * Time.deltaTime);
        }

        if(TouchingPlayer){
            Player.GetComponent<PlayerHealth>().TakeDamage();
            DestroyImmediate(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            NewEnemyPos.x = Random.Range(-100f, 100f);
            NewEnemyPos.y = -20;
            NewEnemyPos.z = Random.Range(-100f, 100f);
            transform.position = NewEnemyPos;
            transform.rotation = FourNum;
        }
        // Enemy.transform.position = UnityEngine.Vector3.MoveTowards(Enemy.transform.position, Player.transform.position, speed);
        // controller.Move((PlayerPos * Time.deltaTime).normalized * speed * PlayerDist/10);
        


    }
}
