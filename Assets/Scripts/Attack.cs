using UnityEngine;

public class Attack : MonoBehaviour
{

    public int damage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ao colidir, salva o inimgo colido  na variavel
        EnemyMeleeController enemy = collision.GetComponent<EnemyMeleeController>();

        if (enemy != null)
        {
            //inimigo recebe dano

            enemy.takeDamage(damage);
        }

    }
}
