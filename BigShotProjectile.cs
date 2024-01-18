using UnityEngine;


public class BigShotProjectile : MonoBehaviour
{
    public float speed = 1f;
    public float damage = 1f;
    public float lifetime = 1f;
    public float size = 1f;
  
    private void Start()
    {
        transform.localScale = new Vector3(size, size, size);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}