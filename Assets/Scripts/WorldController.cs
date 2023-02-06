using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldController : MonoBehaviour
{
    private GameObject player;
    public GameObject environment;
    public LayerMask platform;

    public bool isRotating = false;
    public float rotationDuration = 1f;
    private float rotationCooldown = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        rotationCooldown = Mathf.Max(0, rotationCooldown - Time.unscaledDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        int layer = 1 << hit.gameObject.layer;
        bool hitUnderGround = hit.gameObject.transform.up.y >= 0.95;

        if ((layer & platform) > 0 && !hitUnderGround && !isRotating && rotationCooldown <= 0)
        {
            Vector3 axis = Vector3.Cross(hit.gameObject.transform.up, Vector3.up);
            if (axis != Vector3.zero)
            {
                isRotating = true;
                player.GetComponent<ThirdPersonController>().enabled = false;
                player.GetComponent<CharacterController>().enabled = false;
                float angle = Mathf.Round(Vector3.Angle(Vector3.up, hit.gameObject.transform.up));
                StartCoroutine(RotateWorld(axis, angle, hit.gameObject, hit.gameObject.transform.InverseTransformPoint(hit.point)));
            }
        }
    }

    private IEnumerator RotateWorld(Vector3 axis, float angle, GameObject wall, Vector3 local)
    {
        

        for (float t = 0; t < rotationDuration; t += Time.deltaTime)
        {
            environment.transform.Rotate(axis, angle * Time.deltaTime / rotationDuration, Space.World);
            player.transform.position = wall.transform.TransformPoint(local);
            yield return null;
        }

        environment.transform.eulerAngles = new Vector3(
            Mathf.Round(environment.transform.eulerAngles.x / angle) * angle,
            Mathf.Round(environment.transform.eulerAngles.y / angle) * angle,
            Mathf.Round(environment.transform.eulerAngles.z / angle) * angle
        );

        player.transform.position = wall.transform.TransformPoint(local);
        player.GetComponent<ThirdPersonController>().CancelMove();

        rotationCooldown = 0.5f;
        isRotating = false;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
