using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartShooting();
    }

    public void StartShooting()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        float x = Screen.width / 2f;
        float y = Screen.height / 2f;

        var ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        transform.LookAt(ray.direction * 100);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            var bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
            bullet.transform.Rotate(Vector3.right, 90);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 30, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
