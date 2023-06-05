using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEnemyController : MonoBehaviour
{
    public bool thatIsSpider = false;
    public GameObject Explosion;
    public Collider Collider_1;
    public Collider Collider_2;
    Animator animator;

    [HideInInspector]
    public bool Death = false;

    [HideInInspector]
    public bool killIt = false;
    GameObject obstacle;
    RaycastHit hitinfo;
    float speed = 7f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Death)
        {
            if (thatIsSpider)
            {
                StartCoroutine(wait(0f));
            }
            else
            {
                StartCoroutine(wait(1f));
            }

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
                if (obstacle != null)
                    Destroy(obstacle);
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, Mathf.Infinity, 1 << 9) && !killIt) 
        {
            if (hitinfo.transform.gameObject.transform.CompareTag("Player") || hitinfo.transform.gameObject.transform.CompareTag("Target") || hitinfo.transform.gameObject.transform.CompareTag("Idle Spider"))
            {
                Vector3 Pos = new Vector3(hitinfo.point.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, Pos, speed * Time.deltaTime);

                animator.SetBool("Run", true);
                
            }
        }

        if (killIt)
        {
            if (!thatIsSpider)
            {
                animator.SetBool("Death", true);
            }
            animator.SetBool("Run", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Fire"))
        {
            GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
            Destroy(Ex, 3f);
            Collider_1.enabled = false;
            Collider_2.enabled = false;
            if (!thatIsSpider)
            {
                animator.SetBool("Death", true);
            }

            Death = true;
            killIt = true;
        }

        if (other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Obstacle"))
        {
            killIt = true;
            Collider_1.enabled = false;
            Collider_2.enabled = false;
            Destroy(other.gameObject);
            GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
            Destroy(Ex, 3f);

            Death = true;

            if (!thatIsSpider)
            {
                animator.SetBool("Death", true);
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Target"))
        {
            if (other.gameObject.transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            Collider_1.enabled = false;
            Collider_2.enabled = false;
        }

        if (other.gameObject.CompareTag("Idle Spider"))
        {
            if (other.gameObject.transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }

            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
            Destroy(Ex, 3f);
        }


    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        transform.localScale = new Vector3(transform.localScale.x - (Time.deltaTime*2), transform.localScale.y - (Time.deltaTime * 2), transform.localScale.z - (Time.deltaTime * 2));
        if (obstacle != null)
            obstacle.transform.localScale = new Vector3(obstacle.transform.localScale.x - (Time.deltaTime * 2), obstacle.transform.localScale.y - (Time.deltaTime * 2), obstacle.transform.localScale.z - (Time.deltaTime * 2));
    }
}
