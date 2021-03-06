using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public List<PlayerManager> playerManagers { get; private set; }
    private void Start() {
        playerManagers = new List<PlayerManager>();
    }

    static public void AddPlayer(PlayerManager player) {
        playerManagers.Add(player);
    }
}
