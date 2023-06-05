using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public float Time;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.childCount > 0 && transform.GetChild(0).gameObject.activeInHierarchy)
        {
            StartCoroutine(waitingToDisable(Time));
        }
        else if(transform.childCount > 0 && !transform.GetChild(0).gameObject.activeInHierarchy)
        {
            StartCoroutine(waitingToEnsable(Time));
        }
    }

    IEnumerator waitingToDisable(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator waitingToEnsable(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
