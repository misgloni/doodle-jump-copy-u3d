using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryScript : MonoBehaviour
{
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y)
        {
            Destroy(gameObject);
        }
    }
}
