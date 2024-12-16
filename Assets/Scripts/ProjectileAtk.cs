using UnityEngine;

public class ProjectileAtk : MonoBehaviour
{
    public int damage;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            player.TakeDamage(damage);

            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
