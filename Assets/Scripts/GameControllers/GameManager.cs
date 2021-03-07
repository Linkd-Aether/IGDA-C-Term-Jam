using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Components & References
    static public List<PlayerManager> playerManagers { get; private set; }

    private Spawner[] spawners;
    private Transform playerList;
    private Transform enemyList;


    private void Start() {
        playerManagers = new List<PlayerManager>();

        spawners = GetComponentsInChildren<Spawner>();
        playerList = transform.Find("Players");
        enemyList = transform.Find("Enemies");
    }

    private void OnPlayerJoined(PlayerInput playerInput) {
        PlayerManager player = playerInput.gameObject.GetComponent<PlayerManager>();
        player.ConnectSpawner(spawners[playerManagers.Count]);
        player.transform.parent = playerList;
        playerManagers.Add(player);
        player.gameObject.name = $"Player {playerManagers.Count}";
    }
}
