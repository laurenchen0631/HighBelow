using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefabs;
    public int hp = 3;
    public float rotateSpeed = 60f;
    public float launchPower = 2;
    public float launchInterval = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 1, launchInterval);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefabs, transform.position + 2 * transform.forward, transform.rotation, transform.parent);
        var bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * launchPower, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Destroy(other);
            hp -= 3;
            if (hp <= 0) { Destroy(gameObject); }
        }
    }
}
