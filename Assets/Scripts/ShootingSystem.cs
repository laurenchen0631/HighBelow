using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingSystem : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject player;
    public TextMeshProUGUI hitHint;
    private IEnumerator hitAction;

    public Vector3 offset;
    public bool followPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartShooting();
    }

    public void StartShooting()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void LateUpdate()
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
            if (player.GetComponent<WorldController>().isRotating)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            var bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
            bullet.GetComponent<Bullet>().onHit += ShowHit;
            bullet.transform.Rotate(Vector3.right, 90);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.AddRelativeForce(Vector3.forward * 10, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ShowHit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hitAction != null)
                StopCoroutine(hitAction);
            hitAction = ShowHitAction();
            StartCoroutine(hitAction);
        }
        
    }

    private IEnumerator ShowHitAction()
    {
        hitHint.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitHint.gameObject.SetActive(false);
    }
}
