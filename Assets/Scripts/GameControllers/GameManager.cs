using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<PlayerManager> playerManagers { get; private set; }

    private void Start() {
        playerManagers = new List<PlayerManager>();
    }
}
