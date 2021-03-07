﻿using System.Collections;
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

    public CamFramework camFramework; // Shouldn't be public but is for now !!!


    private void Start() {
        playerManagers = new List<PlayerManager>();

        playerList = transform.Find("Players");
        enemyList = transform.Find("Enemies");

        spawners = playerList.GetComponentsInChildren<Spawner>();
        foreach (Spawner spawner in spawners) {
            spawner.FixStyleForPlayer();
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput) {
        PlayerManager player = playerInput.gameObject.GetComponent<PlayerManager>();
        player.ConnectSpawner(spawners[playerManagers.Count]);
        player.transform.parent = playerList;
        playerManagers.Add(player);
        player.gameObject.name = $"Player {playerManagers.Count}";
        camFramework.AddCamera();
    }
}
