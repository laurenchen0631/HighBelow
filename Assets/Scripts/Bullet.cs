using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRb;
    public float maxDistance = 30;
    public GameObject controlSystem;
    private Vector3 start;

    public delegate void HitAction(Collider other);
    public HitAction onHit;

    private void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (transform.position - start).magnitude;
        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        onHit?.Invoke(other);
        if (!other.CompareTag("Sensor"))
            Destroy(gameObject);
    }
}
