using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject CannonBall;
    public bool OneShoot = true;
    RaycastHit hitinfo;
    bool Allow = true;
    GameObject Ball;
    Vector3 Target;

    void Start()
    {
        
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo) && Allow)
        {
            if (hitinfo.transform.gameObject.transform.CompareTag("Player") || hitinfo.transform.gameObject.transform.CompareTag("Target") || hitinfo.transform.gameObject.transform.CompareTag("Animal") || hitinfo.transform.gameObject.transform.CompareTag("Idle Spider") && Allow)
            {
                Ball = Instantiate(CannonBall, transform.position, Quaternion.identity);
                Target = hitinfo.point;
                Destroy(Ball, 1f);

                if (OneShoot)
                {
                    Allow = false;
                }
            }
        }

        if (Ball != null)
        {
            Ball.transform.position = Vector3.MoveTowards(Ball.transform.position, Target, 75 * Time.deltaTime);
        }
    }
}
