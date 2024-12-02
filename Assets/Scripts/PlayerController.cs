using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

    public float playerSpeed = 0.8f;
    public float currentSpeed;

    public Vector2 playerDirection;

    private bool IsWalking;
    private bool PlayerFacingRight = true;

    private Animator playeranimator;

    private int punchCont;
    private float timeCross = 1.2f;

    private bool comboControl;

    private bool isDead;

   
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        playeranimator = GetComponent<Animator>();

        currentSpeed = playerSpeed;
    }
    
    void Update()
    {
        PlayerMove();

        UpdateAnimator();

        

        if (Input.GetKeyDown(KeyCode.X))
        {
                if (punchCont < 2)
                {
                    playerJab();
                    punchCont++;

                    if (!comboControl)
                    {
                        StartCoroutine(crossController());
                    }
                }

                else if(punchCont >= 2)
                {
                    playerCross();
                    punchCont = 0;
                }
                StopCoroutine(crossController());
        }
    }

    private void FixedUpdate()
    {
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            IsWalking = true;

        }
        else
        {
            IsWalking= false;
        }

        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);

    }

    void PlayerMove()
    {
        //Pega o input do jogador, e cria um Vector2 para usar no PlayerDirection
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerDirection.x < 0 && PlayerFacingRight)
        {
            flip();
        }
        
        else if (playerDirection.x > 0 && PlayerFacingRight == false)
        {
            flip();
        }
    }

    void UpdateAnimator()
    {
        playeranimator.SetBool("IsWalking" , IsWalking);
    }

    void flip()
    {
        PlayerFacingRight = !PlayerFacingRight;

        transform.Rotate(0 , 180  , 0);
    }

    void playerJab()
    {
        playeranimator.SetTrigger("IsJab");
    }
    void playerCross()
    {
        playeranimator.SetTrigger("IsCross");
    }

    IEnumerator crossController()
    {
        comboControl = true;

        yield return new WaitForSeconds(timeCross);

        punchCont = 0;

        comboControl = false;
    }

    void zeroSpeed()
    {
        currentSpeed = 0;
    }

    void resetSpeed()
    {
        currentSpeed = playerSpeed;
    }
}
