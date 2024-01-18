using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeArm : MonoBehaviour
{
    public float rotationSpeed = 50.0f; // Speed at which the arm rotates
    public float damage = 10.0f;        // Damage dealt by the arm

    void Update()
    {
        // Rotate the arm around the player
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Assuming the enemy script has a method to take damage
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
