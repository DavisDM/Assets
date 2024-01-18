using UnityEngine;

public class Shard : MonoBehaviour
{
    public int experienceValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.GainExperience(experienceValue);
                Destroy(gameObject);
            }
            
        }
    }
}
