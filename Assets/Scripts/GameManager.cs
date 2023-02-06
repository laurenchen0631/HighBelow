using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int enenmyCount = 0;
    private GameObject player;
    private PlayerManager playerManager;
    private float duration = 0;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        enenmyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        player = GameObject.FindWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.timeScale;
        timeText.text = $"Time: {Round(duration / 100, 2)}";
        enenmyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (playerManager.hp == 0 || enenmyCount == 0)
        {
            Time.timeScale = 0;
            menu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        } 
    }

    float Round(float n, int d)
    {
        float unit = Mathf.Pow(10, Mathf.Max(0, d));
        return Mathf.Round(n * unit) / unit;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
