using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRangedController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight;
    private bool previousDirectionR;

    private bool isDead;

    private Transform target;

    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private float verticalForce, horizontalForce;

    private bool isWalking;

    private float walkTimer;

    public int maxHealth;
    public int currentHealth;

    private float staggerTime = 0.5f;
    private bool isDamage;
    private float damageTimer;

    private float atkRate = 1f;
    private float nextAtk;

    public Sprite enemyImage;

    public GameObject projectile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        target = FindAnyObjectByType<PlayerController>().transform;

        currentHealth = maxHealth;
        currentSpeed = enemySpeed;
    }

    void Update()
    {
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        if (facingRight && !previousDirectionR)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionR = true;
        }

        if (!facingRight && previousDirectionR)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionR = false;
        }

        walkTimer += Time.deltaTime;

        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        if (isDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            zeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isDamage = false;
                damageTimer = 0;

                resetSpeed();
            }
        }

        updateAnimator();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDistance = target.position - this.transform.position;

            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 0.1f)
            {
                horizontalForce = 0;
            }

            if (Mathf.Abs(targetDistance.y) < 0.05f)
            {
                verticalForce = 0;
            }

            if (!isDamage)
            {
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

            }

            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f & Time.time > nextAtk)
            {
                animator.SetTrigger("Attack");
                zeroSpeed();

                nextAtk = Time.time + atkRate;
            }

        }
    }

    public void Shoot()
    {
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();

        if (facingRight)
        {
            shotPhysics.AddForceX(80f);
        }
        else
        {
            shotPhysics.AddForceX(-80f);
        }
    }

    void updateAnimator()
    {
        animator.SetBool("IsWalking", isWalking);
    }

    void zeroSpeed()
    {
        currentSpeed = 0;
    }

    void resetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void disableEnemy()
    {
        gameObject.SetActive(false);
    }

    public void takeDamage(int damage)
    {
        isDamage = true;

        currentHealth -= damage;

        animator.SetTrigger("HitDamage");

        FindAnyObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

        if (currentHealth <= 0)
        {
            isDead = true;

            rb.linearVelocity = Vector2.zero;

            animator.SetTrigger("Death");
        }
    }


}
