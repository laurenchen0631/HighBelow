using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Vector3 start;

    private void Start()
    {
        start = transform.position;
        GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - start).magnitude;
        if (distance >= 50)
        {
            Destroy(gameObject);
        }
    }
}
