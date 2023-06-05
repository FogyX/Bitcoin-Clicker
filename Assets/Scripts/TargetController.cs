using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject Explosion;
    public Collider Collider_1;
    public Collider Collider_2;

    [HideInInspector]
    public bool Reached = false;
    bool Index = true;
    PlayerController playerController;
    AnimalEnemyController animalEnemyController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, Mathf.Infinity, 1 << 9)) 
        {

            if (hitinfo.transform.gameObject.transform.CompareTag("Player") && Index) 
            {
                Reached = true;
                Index = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Bomb"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().freezeRotation = true;
            Collider_1.enabled = false;
            Collider_2.enabled = false;
            GameObject Ex = Instantiate(Explosion, other.transform.position, Quaternion.identity);
            Destroy(Ex, 3f);
            Destroy(other.gameObject);
            playerController.failed = true;
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Dying", true);
        }

        if (other.gameObject.CompareTag("Animal"))
        {
            animalEnemyController = FindObjectOfType<AnimalEnemyController>();
            if (!animalEnemyController.Death)
            {
                playerController.failed = true;
                GetComponent<Collider>().enabled = false;
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Dying", true);
                GetComponent<Rigidbody>().isKinematic = true;
                Collider_1.enabled = false;
                Collider_2.enabled = false;
                transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
                transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        if (other.gameObject.CompareTag("Idle Spider"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Collider_1.enabled = false;
            Collider_2.enabled = false;
            transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = true;
            playerController.failed = true;
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Dying", true);
        }
    }
}
