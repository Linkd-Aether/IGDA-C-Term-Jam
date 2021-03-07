using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFramework : MonoBehaviour
{
    public GameObject BountyCamPrefab;
    public GameObject dynCamPrefab;
    public List<PlayerManager> players;
    public List<DynCam> playerCameras;
    public int nonPlayerCams;


    void Awake()
    {
        Instantiate(BountyCamPrefab).transform.position = new Vector3(0, -10, -10);
        nonPlayerCams = Camera.allCamerasCount;
    }

    public void AddCamera()
    {
        players = GameManager.playerManagers;
        int playerCount = players.Count;

        for (int i = playerCount; i > (Camera.allCamerasCount - nonPlayerCams); i--)
        {
            GameObject tempCam = Instantiate(dynCamPrefab, transform);
            DynCam dynCam = tempCam.GetComponent<DynCam>();
            dynCam.player = players[i - 1];
            dynCam.InitLayers(i - 1);
            playerCameras.Add(dynCam);
            players[i - 1].playerCam = tempCam.GetComponent<Camera>();
        }

        if(playerCount == 1)
        {
            Camera.allCameras[0].rect = new Rect((float)0.8, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.8, 1);
        }
        if (playerCount >= 2)
        {
            Camera.allCameras[0].rect = new Rect((float)0.4, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.4, 1);
            Camera.allCameras[1].orthographicSize = 7;
            Camera.allCameras[2].rect = new Rect((float)0.6, 0, (float)0.4, 1);
            Camera.allCameras[2].orthographicSize = 7;
        }
        if (playerCount >= 3)
        {
            Camera.allCameras[1].rect = new Rect(0, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[1].orthographicSize = 5;
            Camera.allCameras[3].rect = new Rect(0, 0, (float)0.4, (float)0.5);
        }
        if (playerCount == 4)
        {
            Camera.allCameras[2].rect = new Rect((float)0.6, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[2].orthographicSize = 5;
            Camera.allCameras[4].rect = new Rect((float)0.6, 0, (float)0.4, (float)0.5);
        }

        foreach (DynCam dynCam in playerCameras) {
            dynCam.ResizeCanvas(playerCount);
        }
    }


}
