using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ThirdPersonController))]
[RequireComponent(typeof(WorldController))]
public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController playerController;
    private WorldController worldController;
    [SerializeField] private BulletDetection attackDetector;
    [SerializeField] private TextMeshProUGUI dodgeHint;
    [SerializeField] private TextMeshProUGUI hpText;

    public float dodgeDuration = 0.5f;
    public float invincibleDuration = 0.35f;

    public float dodgeSpeedFactor = 3;
    public float dodgeSppedChangeFactor = 2f;
    public float bulletTimeFactor = 4;

    private bool isDodging = false;
    private bool isInvincible = false;
    private bool isInBulletTime = false;

    private int attackInRange = 0;

    public ParticleSystem bloodEffect;
    public int hp = 5;

    // Start is called before the first frame update
    void Start()
    {
        hp = 5;
        playerController = GetComponent<ThirdPersonController>();
        worldController = GetComponent<WorldController>();
        input = GetComponent<StarterAssetsInputs>();
        attackDetector.onAttackEnter += HandleAttackEnter;
        attackDetector.onAttackExit += HandleAttackLeave;
        hpText.text = string.Join(" ", Enumerable.Repeat("<3", hp));
    }

    private void Update()
    {
        DodgeHint();
        Dodge();
        hpText.text = string.Join(" ", Enumerable.Repeat("<3", hp));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            attackInRange -= 1;
            if (!isInvincible && !worldController.isRotating)
            {
                StartCoroutine(DamageAction(1));
            }
        }
    }

    private void DodgeHint()
    {
        if (attackInRange > 0 && !dodgeHint.IsActive())
        {
            dodgeHint.gameObject.SetActive(true);
        }
        if ((attackInRange == 0 || isInBulletTime) && dodgeHint.IsActive())
        {
            dodgeHint.gameObject.SetActive(false);
        }
    }

    private void Dodge()
    {
        if (input.dodge && !isDodging)
        {
            input.dodge = false;
            if (!worldController.isRotating)
            {
                Debug.Log(attackInRange);
                StartCoroutine(DodgeAction());
            }
        }
    }

    private void HandleAttackEnter()
    {
        attackInRange += 1;
    }

    private void HandleAttackLeave()
    {
        attackInRange -= 1;
    }

    private IEnumerator DodgeAction()
    {
        bool shouldSlowTime = attackInRange > 0;

        isDodging = true;
        isInvincible = true;
        var moveSpeed = playerController.MoveSpeed;
        var SpeedChangeRate = playerController.SpeedChangeRate;
        
        if (shouldSlowTime)
        {
            playerController.MoveSpeed *= bulletTimeFactor;
            isInBulletTime = true;
            Time.timeScale = 1 / bulletTimeFactor;
            yield return new WaitForSecondsRealtime(3);
            isInvincible = false;
            isInBulletTime = false;
            Time.timeScale = 1f;
        }
        else
        {
            playerController.MoveSpeed *= dodgeSpeedFactor;
            playerController.SpeedChangeRate = dodgeSppedChangeFactor;
            yield return new WaitForSeconds(invincibleDuration);
            isInvincible = false;
            yield return new WaitForSeconds(Mathf.Min(dodgeDuration - invincibleDuration, 0));
        }

        playerController.MoveSpeed = moveSpeed;
        playerController.SpeedChangeRate = SpeedChangeRate;
        isDodging = false;
    }

    private IEnumerator DodgeHintAction()
    {
        dodgeHint.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        dodgeHint.gameObject.SetActive(false);
    }

    private IEnumerator DamageAction(int damage)
    {
        hp -= 1;
        bloodEffect.gameObject.transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        bloodEffect.Play();
        isInvincible = true;
        yield return new WaitForSeconds(1);
        isInvincible = false;
    }
}
