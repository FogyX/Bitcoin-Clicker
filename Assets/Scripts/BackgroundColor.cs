using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    public List<Color> BackgoundColors;

    void Awake()
    {
        int ColorIndex = Random.Range(0, BackgoundColors.Count);
        GetComponent<MeshRenderer>().material.color = BackgoundColors[ColorIndex];
    }
}
