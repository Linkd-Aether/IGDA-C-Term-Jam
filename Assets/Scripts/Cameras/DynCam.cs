using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynCam : MonoBehaviour
{
    public PlayerManager player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.playerMob.transform.position + new Vector3(0,0,-10);
    }
}
