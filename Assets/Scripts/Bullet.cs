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
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - start).magnitude;
        if (distance >= 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        Destroy(gameObject);
    }
}
