using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public Vector3 offset;
    public bool followPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        StartShooting();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartShooting()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        transform.LookAt(ray.GetPoint(1000));

        if (followPlayer)
        {
            transform.position = player.transform.TransformPoint(offset);
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            var bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
            bullet.transform.Rotate(Vector3.right, 90);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 10, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
