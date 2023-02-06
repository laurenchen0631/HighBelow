using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletDetection : MonoBehaviour
{
    //public GameObject player;
     private ThirdPersonController player;
    public TextMeshProUGUI dodgeHint;

    public delegate void DetectionAction();
    public DetectionAction onDetect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        dodgeHint.gameObject.SetActive(false);
    }
     
    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
            onDetect?.Invoke();
    }
}
