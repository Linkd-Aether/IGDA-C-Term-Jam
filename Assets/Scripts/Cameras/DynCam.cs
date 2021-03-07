using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynCam : MonoBehaviour
{
    public PlayerManager player;
    private UICanvasManager canvasManager;


    public void InitLayers(int playerValue) {
        canvasManager = GetComponentInChildren<UICanvasManager>();
        canvasManager.SetPlayer(playerValue);

        Camera camera = GetComponent<Camera>();
        for (int i = 0; i < 4; i++) {
            if (i != playerValue) {
                camera.cullingMask &=  ~(1 << LayerMask.NameToLayer($"UILayer{i}"));
            }
        }
    }

    void Update()
    {
        if (player.spawned) {
            transform.position = player.mob.transform.position + new Vector3(0,0,-10);
        }
    }

    public void ResizeCanvas(int playerCount) {
        canvasManager.ResizeCanvas(playerCount);
    }
}
