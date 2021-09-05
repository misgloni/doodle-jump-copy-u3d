using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float 子弹速度 = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("子弹被创建");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * 子弹速度;
        StartCoroutine(FlyTimer());
    }

    IEnumerator FlyTimer()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
    
    private void FixedUpdate()
    {
        
    }
}
