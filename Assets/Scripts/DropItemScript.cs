using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropItemScript : MonoBehaviour
{
    public float 下落加速度 = 10f;
    public float 旋转角速度 = 30f;
    public float 平行最快速度 = 3f;
    public float 平行最慢速度 = 1f;
    private bool wasDrop = false;
    private Rigidbody2D _rigidbody2D;

    private Vector2 _velocity;

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (wasDrop)
        {
            _velocity += Vector2.down * 下落加速度 * Time.deltaTime;
            transform.localPosition = transform.localPosition + ((Vector3) _velocity) * Time.deltaTime;
        }
    }

    public void Drop()
    {
        float speed = Random.Range(平行最慢速度, 平行最快速度) * (Random.Range(-1f, 1f) > 0 ? 1 : -1);
        if (speed > 0)
        {
            _rigidbody2D.angularVelocity = 旋转角速度;
        }
        else
        {
            _rigidbody2D.angularVelocity = -旋转角速度;
        }
        Debug.Log("speed"+speed);
        _velocity = new Vector2(speed, 0);


        wasDrop = true;
        StartCoroutine(DropTimer());
    }

    IEnumerator DropTimer()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}