using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeScript : MonoBehaviour
{
    public Vector3 localPosition;
    
    // Start is called before the first frame update
    private void Awake()
    {
        localPosition = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = localPosition;
    }
}
