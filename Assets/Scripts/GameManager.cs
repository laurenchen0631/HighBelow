using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int enenmyCount = 0;
    private GameObject player;
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        enenmyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        player = GameObject.FindWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        enenmyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
