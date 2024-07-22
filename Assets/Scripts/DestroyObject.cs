using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyDuration = 3;

    void Start()
    {
        DestroyObject(gameObject, destroyDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
