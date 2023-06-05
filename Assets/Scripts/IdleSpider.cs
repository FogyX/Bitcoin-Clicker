using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSpider : MonoBehaviour
{
    bool Death = false;
    GameObject obstacle;
    public GameObject Explosion;

    void Start()
    {
        
    }

    void Update()
    {
        if (Death)
        {
            StartCoroutine(wait(1f));

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
                Destroy(obstacle);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Bomb"))
        {
            GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
            Destroy(Ex, 3f);

            Death = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetBool("Death", true);
            obstacle = other.gameObject;
        }

        if (other.gameObject.CompareTag("Animal"))
        {
            Destroy(gameObject);
        }
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Target") ))
        {
            GetComponent<Animator>().SetBool("Run", false);
            GetComponent<Animator>().SetBool("Attack", true);

            if (other.gameObject.CompareTag("Idle Spider"))
            {
                GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
                Destroy(Ex, 3f);
            }
        }
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale = new Vector3(transform.localScale.x - (Time.deltaTime * 2), transform.localScale.y - (Time.deltaTime * 2), transform.localScale.z - (Time.deltaTime * 2));
        obstacle.transform.localScale = new Vector3(obstacle.transform.localScale.x - (Time.deltaTime * 2), obstacle.transform.localScale.y - (Time.deltaTime * 2), obstacle.transform.localScale.z - (Time.deltaTime * 2));
    }

}
