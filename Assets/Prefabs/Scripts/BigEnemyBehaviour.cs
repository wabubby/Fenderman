using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BigEnemyBehaviour : MonoBehaviour
{
    public float SpeedTimer = 1f;
    public float XSpeed = 1.2f;
    public float YSpeed = 1f;
    public float Distance;
    GameObject Player;
    Vector3 NewEnemyPosition;

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
        Distance = Vector3.Distance(Player.transform.position, transform.position);

        if(Player.transform.position.x - transform.position.x < 3 && Player.transform.position.z - transform.position.z < 3){
            XSpeed = 0;
        } else {
            XSpeed = 1;
        }

        NewEnemyPosition.x = transform.position.x + playerDirection.x * XSpeed * SpeedTimer * Time.deltaTime;
        NewEnemyPosition.z = transform.position.z + playerDirection.z * XSpeed * SpeedTimer * Time.deltaTime;;
        //NewEnemyPosition.y = transform.position.y + 4*Time.deltaTime * Mathf.Sin(Time.time*Time.deltaTime);

        float verticalSpeed = 5*Time.deltaTime;
        float directionToPlayer = Mathf.Sign(Player.transform.position.y - transform.position.y);
        // if (Mathf.Abs(Player.transform.position.y - transform.position.y)>0.3f) {
        //     NewEnemyPosition.y += directionToPlayer * verticalSpeed;
        // }
        // if(Player.transform.position.y > transform.position.y + 0.5f && YSpeed < 5)
        //     YSpeed += 0.3f;
        // if(Player.transform.position.y < transform.position.y - 0.5f && YSpeed > -5)
        //     YSpeed -= 0.3f;
        
        //YSpeed = (Player.transform.position.y - transform.position.y) * 2;
        YSpeed = 5f * 2*Mathf.PI * Mathf.Cos(Time.time / 3f * 2*Mathf.PI) / 3f;


        NewEnemyPosition.y = Player.transform.position.y + YSpeed * Time.deltaTime;

        

        transform.position = NewEnemyPosition;
    }
}
