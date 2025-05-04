using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapObject : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ¿ÛÑª
            FindObjectOfType<LifeCount>().LoseLife();

            // »÷ÍË
            Playermovement player = collision.GetComponent<Playermovement>();
            if (player != null)
            {
                player.TakeDamage(); // ²¥·Å»÷ÍËÐ§¹û
            }
        }
    }
}