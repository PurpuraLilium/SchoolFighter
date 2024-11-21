using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;

    public float playerSpeed = 0.8f;

    public Vector2 playerDirection;

    private bool IsWalking;
    private bool PlayerFacingRight = true;

    private Animator playeranimator;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        playeranimator = GetComponent<Animator>();

    }
    
    void Update()
    {
        PlayerMove();

        UpdateAnimator();
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

        playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);

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
}
