using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool PingPongMovement = false;
    public GameObject ConfettiPartical;
    public GameObject Explosion;
    public Collider Collider_1;
    public Collider Collider_2;
    GameObject Target;
    Animator animator;
    TargetController targetController;
    float speed = 3f;
    RaycastHit hitinfo;
    AnimalEnemyController animalEnemyController;


    [HideInInspector]
    public bool Completed = false;

    [HideInInspector]
    public bool failed = false;

    private void Awake()
    {
        targetController = FindObjectOfType<TargetController>();
        animator = GetComponent<Animator>();
        Target = FindObjectOfType<TargetController>().gameObject;
    }

    private void Update()
    {
        if (PingPongMovement)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, Mathf.Infinity, 1 << 9))
            {
                if (hitinfo.transform.gameObject.transform.CompareTag("Pin") || hitinfo.transform.gameObject.transform.CompareTag("Platform"))
                {
                    animator.SetBool("Idle", false);
                    animator.SetBool("Run", true);
                    animator.SetBool("Jump", false);
                    Vector3 Pos = new Vector3(hitinfo.point.x, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, Pos, speed * Time.deltaTime);
                }
            }
        }

        if (targetController.Reached && animator.GetBool("Jump") == false)
        {
            if (targetController.gameObject.transform.position.x < 0) 
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            }
            else if (targetController.gameObject.transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            animator.SetBool("Jump", false);
            PingPongMovement = false;
            Vector3 Pos = new Vector3(targetController.gameObject.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, Pos, speed * Time.deltaTime);

        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitinfo) && !Completed)
        {
            if (transform.position.y - hitinfo.transform.gameObject.transform.position.y > .5f)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Idle", false);
                animator.SetBool("Jump", true);
            }
            else if (transform.position.y - hitinfo.transform.gameObject.transform.position.y <= .3f && animator.GetBool("Run") == false)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Idle", true);
            }
        }

        if (GameObject.FindGameObjectsWithTag("Pin").Length == 0 && !failed && !Completed)
        {
            PingPongMovement = true;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Pin") || other.gameObject.CompareTag("Platform")) && PingPongMovement)
        {

            if (transform.rotation.y < 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }
            else if (transform.rotation.y > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
            }
        }

        if (other.gameObject.CompareTag("Target") && !failed && !Completed)
        {
            Instantiate(ConfettiPartical);
            targetController.Reached = false;
            PingPongMovement = false;
            animator = other.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
            transform.rotation = Quaternion.Euler(0, 180, 0);
            other.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Win", true);
            GameObject newPlayer = Instantiate(other.gameObject.transform.GetChild(0).gameObject, transform.position, transform.rotation);
            newPlayer.GetComponent<Animator>().SetBool("Win", true);
            Destroy(gameObject);
            Completed = true;
        }

        if ((other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Bomb")) || other.gameObject.CompareTag("Fire") && !failed)
        {
            targetController.Reached = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Collider_1.enabled = false;
            Collider_2.enabled = false;
            if (other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Obstacle")) 
            {
                GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
                Destroy(Ex, 3f);
                Destroy(other.gameObject);
                failed = true;
            }

            other.gameObject.GetComponent<Collider>().enabled = failed;
            GameObject newPlayer = Instantiate(Target.transform.GetChild(0).gameObject, transform.position, transform.rotation);
            newPlayer.GetComponent<Animator>().SetBool("Dying", true);
            Destroy(gameObject);
            failed = true;
        }

        if (other.gameObject.CompareTag("Animal") || other.gameObject.CompareTag("Idle Spider") && !failed)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Collider_1.enabled = false;
            Collider_2.enabled = false;

            if (other.gameObject.CompareTag("Animal"))
            {
                animalEnemyController = FindObjectOfType<AnimalEnemyController>();
                if (!animalEnemyController.Death)
                {
                    targetController.Reached = false;
                    GameObject newPlayer = Instantiate(Target.transform.GetChild(0).gameObject, transform.position, other.transform.rotation);
                    newPlayer.GetComponent<Animator>().SetBool("Dying", true);
                    Destroy(gameObject);
                    failed = true;
                }
            }
            else
            {
                targetController.Reached = false;
                GameObject newPlayer = Instantiate(Target.transform.GetChild(0).gameObject, transform.position, other.transform.rotation);
                newPlayer.GetComponent<Animator>().SetBool("Dying", true);
                Destroy(gameObject);
                failed = true;
            }
            failed = true;
        }
    }
}
