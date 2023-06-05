using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject Explosion;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Idle Spider") || other.gameObject.CompareTag("Fire"))
        {
            GameObject EX = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(EX, 3f);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Target"))
        {
            GameObject EX = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(EX, 3f);
            Destroy(gameObject);

        }

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
