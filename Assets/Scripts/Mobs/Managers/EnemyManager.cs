using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MobManager
{

    protected override void Awake() {
        base.Awake();

        mobPrefab = (GameObject) Resources.Load("Prefabs/Mobs/Mobs/EnemyMob");
        respawnWait = 15f;
    }
}
