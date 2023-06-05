using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    RaycastHit hitinfo;
    float speed = 75f;
    bool done = false;
    GameObject Target;
    bool killed = false;
    Vector3 TargetPos;


    void Start()
    {
        
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo) && !killed)
        {
            if (hitinfo.transform.gameObject.transform.CompareTag("Player") || hitinfo.transform.gameObject.transform.CompareTag("Target") || hitinfo.transform.gameObject.transform.CompareTag("Animal"))
            {
                done = true;
                Target = hitinfo.transform.gameObject;
                TargetPos = hitinfo.point;
            }
        }

        if (done && Target != null) 
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Animal") || other.gameObject.CompareTag("Player"))
        {
            killed = true;
        }
    }
}
