using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynCam : MonoBehaviour
{
    public PlayerManager player;

    // Update is called once per frame
    void Update()
    {
        if (player.spawned) {
            transform.position = player.mob.transform.position + new Vector3(0,0,-10);
        }
    }
}
