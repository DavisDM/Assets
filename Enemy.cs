using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float hitPoints;
    public float damage;
    public float speed;
    // Removed attackRange and chaseRange as they are not used in this context

   
    private GameObject player;
    private Rigidbody2D rb;

    public GameObject shardPrefab;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
        {
            ChasePlayer();
        }


  void ChasePlayer()
{
    if (player != null)
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(player.transform.position, transform.position);

        // Only move if the enemy is above a certain distance from the player
        if (distance > 0.05f) // Adjust the distance as needed
        {
            rb.MovePosition(rb.position + speed * Time.deltaTime * direction);
        }
    }
}

    public float GetDamageValue()
    {
        return damage;
    }


    public void TakeDamage(float amount)
    {
        hitPoints -= amount;
        if (hitPoints <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // Handle enemy death here (e.g., play animation, remove from game, etc.)
        Destroy(gameObject);
        Instantiate(shardPrefab, transform.position, Quaternion.identity);
    }
}
