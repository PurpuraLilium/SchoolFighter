using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    //verifica se est� vivo
    public bool isDead;

   

    //verifica se est� olhando para a direita
    public bool facingRight;
    public bool previousDirectionR;

    //variavel para armazenar posi��o do player
    private Transform target;

    //variaveis para movimenta��o
    private float enemySpeed = 0.4f;
    private float currentSpeed;

    private bool isWalking;

    private float horizontalForce;
    private float verticalForce;

    private float walkTimer;

    //Variaveis para ataque
    private float attackRate = 1f;
    private float nextAttack;

    //variaveis para dano
    public int maxHealth;
    private int currentHealth;

    public float staggerTime = 0.5f;
    private float damageTimer;
    public bool isDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //busca o player e armazena a posi��o
        target = FindAnyObjectByType<PlayerController>().transform;

        currentSpeed = enemySpeed;
    }

    void Update()
    {
        //verifica se o player est� a direita/esquerda e vira o inimigo para o lado que o player est�
        if (target.position.x < transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;  
        }

        if (facingRight && !previousDirectionR)
        {
            transform.Rotate(0, 180, 0);
            previousDirectionR = true;
        }
        if (!facingRight && previousDirectionR)
        {
            transform.Rotate(0, -180, 0);
            previousDirectionR = false;
        }

        //iniciar o timer do caminhar horizontal

        walkTimer += Time.deltaTime;

        //gerenciar anima��o
        if(horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        //gerencia o tempo de stagger
        if(isDamage && !isDead)
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

        UptadeAnimator();

    }

    private void FixedUpdate()
    {
        //MOVIMENTA��O
        //Variavel para armezanar a distancia com o player
        Vector3 targetDistance = target.position - transform.position;

        //mathf.Abs sendo usado para tornar o segundo valor sempre positivo
        horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

        //Entre 1 e 2 seg , ser� feita uma defini��o de dire��o vertical
        if (walkTimer >= Random.Range(1f, 2f))
        {
            verticalForce = Random.Range(-1,2);

            walkTimer = 0;
        }

        //parar a movimenta��o quando estiver perto do player
        if(Mathf.Abs(targetDistance.x) < 0.15)
        {
            horizontalForce = 0;
        }

        //aplica velocidade e faz com que se movimente

        rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

        //ATAQUE
        //Ataca se estiver perto do player a depender do tempo
        if (Mathf.Abs(targetDistance.x) < 0.2f && Mathf.Abs(targetDistance.y) < 0.05 && Time.time > nextAttack)
        {
            animator.SetTrigger("Attack");

            zeroSpeed();

            nextAttack = Time.time + attackRate;
        }

    }

    void UptadeAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void takeDamage(int damage)
    {
        if (!isDead) 
        {
            isDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("HitDamage");
        }
    }

    void zeroSpeed()
    {
        currentSpeed = 0;
    }

    void resetSpeed()
    {
        currentSpeed = enemySpeed;
    }
}
