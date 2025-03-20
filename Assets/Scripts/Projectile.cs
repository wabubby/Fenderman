using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    enum MovementState {
        None, // starting value, undertermined
        Gliding, // cannot be effected by anything, is kinematic
        Flying, // Bullet is flying through the aaiir and is not kinematic
        Collided // Bullet has collided and is not kinematc
    }

    [Header("General")]
    [Tooltip("Radius of this projectile's collision detection")] public float rayCastCheckRadius;
    [Tooltip("LifeTime of the projectile")] public float MaxLifeTime = 300f;

    
    [Header("Movement")]
    [Tooltip("Starts Kinematic")] public bool startsGliding = true;
    [Tooltip("Downward acceleration from gravity")] public float GravityDownAcceleration = 0f;
    [Tooltip("Percent of inherited from weapon")] public float InheritWeaponVelocityRatio = 0f;
    [Tooltip("Default speed (may be overriden by weapon)")] public float speed = 10f;


    [Header("Speed Up type projectiles")]
    [Tooltip("Projectile speeds up after shot")] public bool isTypeSpeedUp = false;
    [Tooltip("Duration to reach top speed")] public float speedUpDuration;
    [Tooltip("Curve of starting speed to top speed")] public AnimationCurve speedUpCurve;


    [Header("Damage")]
    [Tooltip("Obviously...")] public float Damage = 40f;
    [Tooltip("Explosion Object Prefab")] public GameObject ExplosionPrefab;


    [Header("VFX and SFX")]
    [Tooltip("Component where clips will be played")] public AudioSource audioSource;
    [Tooltip("Audioclip played on impact")] public AudioClip[] ImpactSfxClips;
    [Tooltip("Impact particle prefabs")] public GameObject[] ImpactParticleEffects;
    [Tooltip("Transform where trail effects spawn")] public Transform BulletTrailTransform;
    [Tooltip("Bullet trail prefabs")]  public GameObject[] BulletTrails;

    [Header("Slow Mo")]
    [Tooltip("Time Scale after first collision")]  public bool collisionSloMo = false;
    [Tooltip("Time Scale after first collision")]  public float collisionSloMoTimeScale = 0.2f;
    [Tooltip("Slo-mo duration after first collision")]  public float collisionSloMoDuration = 0.1f;

    MovementState movementState; // current movement state
    float distanceTraveled; // used to find out when to delete the bullet
    float timeAtShoot; // Time at shoot
    bool isSpedUp; // is the projectile at full speed
    float m_SlowTimer; // how long has the bullet been slow
    bool hasCollided; // has the bullet collided
    bool exploding; // is the projectile exploding
    private GameObject[] bulletTrailObjects;

    Rigidbody rb;
    Vector3 velocity;

    void Awake() {
        // Rigid body shit
        rb = GetComponent<Rigidbody>();

        // Bullet Trail Transform
        if (! BulletTrailTransform) { // if no bullet trail transform defined, set bullet trail to transform
            BulletTrailTransform = transform;
        }

        // Bullet Trail objects
        bulletTrailObjects = new GameObject[BulletTrails.Length];
        for (int i = 0; i < BulletTrails.Length; i++){
            GameObject bulletTrail = BulletTrails[i];
            bulletTrailObjects[i] = Instantiate(bulletTrail, BulletTrailTransform.position, transform.rotation, transform);
        }

    }

    // Start is called before the first frame update
    void Start() {
        // time at first frame
        timeAtShoot = Time.time;

        // gliding
        if (startsGliding) {
            setGliding(true);
        } else {
            setGliding(false);
        }

        // exploding
        if (ExplosionPrefab != null) {
            exploding = true;
            Exploding explodingComponent = gameObject.AddComponent<Exploding>();
            explodingComponent.explosionPrefab = ExplosionPrefab;
        }
    }
    
    void FixedUpdate() {
        if (movementState == MovementState.Gliding) {
            distanceTraveled += velocity.magnitude * Time.deltaTime;

            // bullet gravity
            velocity = applyGravity(velocity);

            // speed up
            velocity = applySpeedUp(velocity);

        } else if (movementState == MovementState.Flying) {
            distanceTraveled += rb.linearVelocity.magnitude * Time.deltaTime;
            
            // bullet gravity
            rb.linearVelocity = applyGravity(rb.linearVelocity);

            // speed up
            rb.linearVelocity = applySpeedUp(rb.linearVelocity);
        }


        if (movementState == MovementState.Gliding) { // is the bullet flying through the air
            // set rotation in direction of velocity for raycast
            // transform.rotation = Quaternion.Euler(velocity);

            // check if object will hit another this update. If so, disable flight.
            bool foundHit = false;
            int numValidHits = 0;

            if (Mathf.Abs(velocity.magnitude * Time.deltaTime) > 0f) {
                RaycastHit[] hits;
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;

                hits = Physics.SphereCastAll(transform.position, rayCastCheckRadius, 
                    velocity.normalized, velocity.magnitude * Time.deltaTime);

                foreach (RaycastHit hit in hits) {
                    if (hit.collider != null) {
                        if (CanHit(hit.collider))  {
                            numValidHits += 1;
                        }
                        if (CanHit(hit.collider) && hit.distance < closestHit.distance) {
                            foundHit = true;
                            closestHit = hit;
                        }
                    }
                }
            } else {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, -1);
                foreach (Collider collider in colliders) {
                    if (collider != null) {
                        if (CanHit(collider)) {
                            numValidHits += 1;
                            foundHit = true;
                        }
                    }
                }
            }
            
            
            // if collision, set state to flying! (enables rigidbody)
            if (foundHit) {
                setGliding(false);
            }

            if (movementState == MovementState.Gliding) { // if no hits, then move the projectile
                transform.position += velocity * Time.deltaTime;
            }
        }
    }

    Vector3 applyGravity(Vector3 velocity) {
        Vector3 newVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
        if (GravityDownAcceleration > 0) {
            newVelocity += Vector3.down * GravityDownAcceleration * Time.deltaTime;
        }
        return newVelocity;
    }
    Vector3 applySpeedUp(Vector3 velocity) {
        Vector3 newVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
        if (isTypeSpeedUp && !isSpedUp) {
            newVelocity += getSpeedUpVelocity() * Time.deltaTime * transform.forward;

            // once at full speed, set speed to speed
            if (newVelocity.magnitude > speed) {
                isSpedUp = true;
                newVelocity = speed * transform.forward;
            }
        }
        return newVelocity;
    }

    void Update() {

        // destroy bullet if time lasted longer than duration or time alive exceeds 5 minutes
        if ((distanceTraveled > 500f) || (Time.time - timeAtShoot >= MaxLifeTime)) {
            Destroy(gameObject);
        }

        // Trail YeeDelete
        if (movementState == MovementState.Collided) {
            // if the velocity's magnitude is less than 2, start a timer to delete the trail
            if (rb.linearVelocity.magnitude < 2) {
                m_SlowTimer += Time.deltaTime;
            } else {
                m_SlowTimer = 0;
            }

            if (m_SlowTimer >= 1f) {
                // if have been idle for a while(probably on the ground), destroy trail object
                for (int i = 0; i < bulletTrailObjects.Length; i++){
                    Destroy(bulletTrailObjects[i]);
                }
            }

            if (m_SlowTimer >= 30f) {
                // if have been idle for a while(probably on the ground), destroy self
                Destroy(gameObject);
            }
        }
    }

    
    void setGliding(bool gliding) {
        // movement state is the same dumbass

        if (gliding && movementState == MovementState.Gliding) {
            return; 
        }

        if (gliding) {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            
            if (movementState == MovementState.Flying) { // if previously flying, transfer velocity
                velocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
                foreach (GameObject bulletTrail in bulletTrailObjects) {
                    // bulletTrail.GetComponent<ParticleSystem>().emmi
                }
            }

            movementState = MovementState.Gliding;
        } else {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.isKinematic = false;

            if (movementState == MovementState.Gliding) { // if previously gliding, transfer velocity
                rb.linearVelocity = new Vector3(velocity.x, velocity.y, velocity.z); // transfer velocity
                velocity = Vector3.zero; // set flight velocity to 0 because it's not flying no more
            }
            

            movementState = MovementState.Flying;
        }
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
        
        if (isTypeSpeedUp) velocity = Vector3.zero; // Vector3.zero; // set velocity to zero
        else velocity = velocity = transform.forward * newSpeed; // instantly set velocity

        if (!startsGliding) {
            rb.linearVelocity = new Vector3(velocity.x, velocity.y, velocity.z);
        }
    }

    public void addVelocity(Vector3 addVelocity) {
        velocity += addVelocity;
    }

    float getSpeedUpVelocity() {
        // distance at this point

        float t = Mathf.Clamp((Time.time - timeAtShoot) / speedUpDuration, 0f, 1f); // the x value of the curve
        float a = speedUpCurve.Evaluate(t) * speed; // the y value of this point
        return a;
    }

    bool CanHit(Collider collider) {
        return true;
    }

    void OnCollisionEnter(Collision collision) {
        if (movementState == MovementState.Flying) { // first collision

            // if (ShooterOwner != null && collision.gameObject != ShooterOwner) { // do not take health from the owner
            //     // do damage if object has health
            //     Health collidedObjectHealth = collision.gameObject.GetComponent<Health>();
            //     if (collidedObjectHealth) { // has health component
            //         collidedObjectHealth.Oof(Damage);
            //     }
            // }
            movementState = MovementState.Collided;

            Impact();
        }
        
    }

    void Impact() {
        // become true physics object and get gravity

        rb.useGravity = true;
        hasCollided = true;
        PlayImpactParticleEffects();
        PlayImpactSFX();
        ImpactSloMo();

        AfterImpact();
    }

    void AfterImpact() {
        if (exploding) {
            Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void PlayImpactParticleEffects() {
        foreach (GameObject particleEffect in ImpactParticleEffects) {
            Instantiate(particleEffect, transform.position, transform.rotation);
        }
    }
    void PlayImpactSFX() {
        if (ImpactSfxClips.Length > 0) {
            PlayRandomPitchVolume(audioSource, GetRandomSound(ImpactSfxClips), 0.9f, 1.1f, 0.8f, 1.2f);
        }
    }
    void ImpactSloMo() {
        // if (collisionSloMo) {
        //     timeManager.DoSlowMo(collisionSloMoTimeScale, collisionSloMoDuration);
        // }
    }

    public AudioClip GetRandomSound(AudioClip[] sfxs) {
        return sfxs[UnityEngine.Random.Range(0, sfxs.Length)];
    }
    public void PlayRandomPitchVolume(AudioSource audioSource, AudioClip sfx, float minPitch, float maxPitch, float minVol, float maxVol) {
        audioSource.pitch = (UnityEngine.Random.Range(minPitch, maxPitch));
        audioSource.volume = (UnityEngine.Random.Range(minVol, maxVol));
        audioSource.PlayOneShot(sfx);
    }

}