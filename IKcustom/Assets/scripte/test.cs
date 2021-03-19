using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public GameObject enfant;
    public GameObject sousenfant;

    // Start is called before the first frame update
    void Start()
    {
        enfant.transform.rotation = Quaternion.Euler(0, 0, 45);
        sousenfant.transform.rotation = Quaternion.Euler(0, 90, 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
