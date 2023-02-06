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
    private bool bulletTime = false;

    public delegate void DetectionAction();
    public DetectionAction onAttackEnter;
    public DetectionAction onAttackExit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        dodgeHint.gameObject.SetActive(false);
    }
     
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
            onAttackEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
            onAttackExit?.Invoke();
    }

    private IEnumerator DodgeHintAction()
    {
        dodgeHint.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        dodgeHint.gameObject.SetActive(false);
    }

    private IEnumerator SlowTimeAction()
    {
        dodgeHint.gameObject.SetActive(false);
        bulletTime = true;
        Time.timeScale = 0.2f;
        player.MoveSpeed *= 2;
        yield return new WaitForSecondsRealtime(3);
        player.MoveSpeed /= 2;
        Time.timeScale = 1f;
        bulletTime = false;
    }
}
