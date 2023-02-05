using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 60 * Time.deltaTime);
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefabs, transform.position + transform.forward, transform.rotation);
        var bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * 3, ForceMode.Impulse);
    }
}
