using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public GameObject deadZombieAsset;
    [Header("General")]
    public float maxHealth = 100f;
    public float health = 100f;
    public float damage = 5f;
    public float biteCooldown = 2f;
    public float moveSpeed = 0.5f;
    [Header("Idle")]
    public float idleTravelTime = 1f;
    public float idleStandTime = 2f;
    [Header("Look")]
    public float visionDist = 7f;
    public float visionClippingDist = 0.3f;

    //Idle
    private bool isDead = false;
    private bool isIdleStand = false;
    private bool isIdleTravel = false;
    private Vector3 idleTravelTarget;
    // Attack
    private bool isAttacking = false;
    private bool isBiteCooldown = false;
    // Look
    private int rayCount = 15;
    private float rayAngle = 90f;
    private GameObject detectedPlayer;

    private PolygonCollider2D collider;
    private Rigidbody2D body;
    private SpriteRenderer renderer;
    private Animator animator;


    void Start()
    {
        collider = GetComponent<PolygonCollider2D>();
        body = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Update()
    {
        if (!isDead)
        {
            if ( health <= 0)
            {
                collider.enabled = false;
                renderer.enabled = false;
                animator.enabled = false;
                deadZombieAsset.SetActive(true);
                isDead = true;
            }
            else if (LookForPlayer())
            {
                isAttacking = true;
                MoveTowards(detectedPlayer.transform.position);
            }
            else
            {
                isAttacking = false;
                if (!isIdleStand && !isIdleTravel)
                {
                    StartCoroutine(IdleStandRoutine());
                }
                else if (isIdleTravel)
                {
                    MoveTowards(idleTravelTarget);
                }
            }
        }
    }

    private void MoveTowards(Vector2 target)
    {
        animator.enabled = true;
        Vector2 direction = (Vector3)target - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        Vector2 newPos = body.position;
        float speed = Mathf.Lerp(0, moveSpeed, Time.deltaTime);
        newPos += direction.normalized * speed;
        body.position = newPos;
    }

    private bool LookForPlayer()
    {
        // Calculate the angle between rays
        float andgleStep = rayAngle / rayCount;

        // Loop throught each ray
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the current angle of the ray
            float angle = transform.eulerAngles.z - rayAngle / 2f + andgleStep * i + 90f;

            // Convert Euler angle to Radians
            float angleRadians = angle * Mathf.Deg2Rad;

            // Calculate the direction of ray
            Vector2 direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

            // Cast the ray and get hit result
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + direction * visionClippingDist, direction, visionDist);

            // For debug only: draw lines for every ray
            if (hit)
            {
                Debug.DrawLine((Vector2)transform.position + direction * visionClippingDist, (Vector2)transform.position + direction * visionDist, Color.red);
            }
            else
            {
                Debug.DrawLine((Vector2)transform.position + direction * visionClippingDist, (Vector2)transform.position + direction * visionDist, Color.white);
            }

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                detectedPlayer = hit.collider.gameObject;
                return true;
            }
        }
        detectedPlayer = null;
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderObject = collision.gameObject;
        if (colliderObject.CompareTag("Bullet"))
        {
            health -= colliderObject.GetComponent<BulletController>().damage;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject colliderObject = collision.gameObject;
        if (colliderObject.CompareTag("Player") && !isBiteCooldown)
        {
            colliderObject.GetComponent<PlayerController>().health -= damage;
            colliderObject.GetComponent<PlayerController>().UpdateHealth();
            StartCoroutine(BiteCooldownRoutine());
        }
    }

    private IEnumerator BiteCooldownRoutine()
    {
        isBiteCooldown = true;
        yield return new WaitForSeconds(biteCooldown);
        isBiteCooldown = false;
    }

    private IEnumerator IdleStandRoutine()
    {
        isIdleStand = true;
        animator.enabled = false;
        yield return new WaitForSeconds(idleStandTime);
        isIdleStand = false;
        if (!isAttacking) StartCoroutine(IdleTravelRoutine());
    }

    private IEnumerator IdleTravelRoutine()
    {
        isIdleTravel = true;
        idleTravelTarget = Random.onUnitSphere;
        idleTravelTarget *= 100f;
        yield return new WaitForSeconds(idleTravelTime);
        isIdleTravel = false;
        if (!isAttacking) StartCoroutine(IdleStandRoutine());
    }
}
