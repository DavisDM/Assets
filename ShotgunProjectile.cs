using UnityEngine;

public class ShotgunProjectile : MonoBehaviour
{
    public float speed = 10f;         // Speed of the projectile
    public float damage = 5f;         // Damage dealt by the projectile
    public float lifetime = 2f;       // Time before the projectile is destroyed
    private void Start()
    {
        Destroy(gameObject, lifetime); // Automatically destroy after a set time
    }
    private void Update()
    {
        // Move the projectile
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Assuming the enemy has a script with a TakeDamage method
            other.GetComponent<Enemy>().TakeDamage(damage);

            // Destroy the projectile after dealing damage
            Destroy(gameObject);
        }
    }
}
