using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFramework : MonoBehaviour
{
    public GameObject UICamPrefab;
    public GameObject dynCamPrefab;
    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(UICamPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Length - (Camera.allCamerasCount - 1); i++)
        {
            GameObject tempCam = Instantiate(dynCamPrefab);
            tempCam.GetComponent<DynCam>().player = players[i];
        }

        if(players.Length == 1)
        {
            Camera.allCameras[0].rect = new Rect((float)0.8, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.8, 1);
        }
        if (players.Length >= 2)
        {
            Camera.allCameras[0].rect = new Rect((float)0.4, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.4, 1);
            Camera.allCameras[2].rect = new Rect((float)0.6, 0, (float)0.4, 1);
        }
        if (players.Length >= 3)
        {
            Camera.allCameras[1].rect = new Rect(0, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[3].rect = new Rect(0, 0, (float)0.4, (float)0.5);
        }
        if (players.Length == 4)
        {
            Camera.allCameras[2].rect = new Rect((float)0.6, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[3].rect = new Rect((float)0.6, 0, (float)0.4, (float)0.5);
        }
    }


}
