using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && IsInView(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    bool IsInView(GameObject gameObject)
    {
        Camera cam = Camera.main;
        Vector3 pointOnScreen = cam.WorldToViewportPoint(gameObject.transform.position);

        // Is in view?
        return pointOnScreen.x > 0 && pointOnScreen.x < 1 && pointOnScreen.y > 0 && pointOnScreen.y < 1;
    }
}
