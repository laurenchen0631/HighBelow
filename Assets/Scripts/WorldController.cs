using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    private GameObject player;
    public GameObject environment;

    public bool isRotating = false;
    public float rotationDuration = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool hitUnderGround = Mathf.Abs(hit.moveDirection.y + 1) < 0.1;
        if (!hitUnderGround && !isRotating)
        {
            Vector3 axis = Vector3.Cross(hit.gameObject.transform.up, Vector3.up);
            //Debug.Log(hit.gameObject.transform.up);
            Debug.Log(axis);
            isRotating = true;
            player.GetComponent<ThirdPersonController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            StartCoroutine(RotateWorld(axis, hit.gameObject, hit.gameObject.transform.InverseTransformPoint(hit.point)));
        }
    }

    private IEnumerator RotateWorld(Vector3 axis, GameObject wall, Vector3 local)
    {
        //var fromAngle = environment.transform.localRotation;
        //var toAngle = Quaternion.Euler(environment.transform.localRotation.eulerAngles + axis * 90);
        float lastTime = 0;
        for (float t = 0; t < rotationDuration; t += Time.deltaTime)
        {
            environment.transform.Rotate(axis, 90 * Time.deltaTime / rotationDuration, Space.World);
            player.transform.position = wall.transform.TransformPoint(local);
            lastTime = t;
            yield return null;
        }
        Debug.Log(lastTime);

        environment.transform.eulerAngles = new Vector3(
            Mathf.Round(environment.transform.eulerAngles.x / 90) * 90,
            Mathf.Round(environment.transform.eulerAngles.y / 90) * 90,
            Mathf.Round(environment.transform.eulerAngles.z / 90) * 90
        );

        player.transform.position = wall.transform.TransformPoint(local);
        player.transform.position = new Vector3(player.transform.position.x, -0.5f, player.transform.position.z);
        isRotating = false;
        player.GetComponent<ThirdPersonController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
