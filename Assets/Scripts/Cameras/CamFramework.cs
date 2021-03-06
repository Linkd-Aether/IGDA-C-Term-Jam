using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFramework : MonoBehaviour
{
    public GameObject BountyCamPrefab;
    public GameObject dynCamPrefab;
    public List<PlayerManager> players;
    public int nonPlayerCams;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(BountyCamPrefab).transform.position = new Vector3(0, -10, -10);
        nonPlayerCams = Camera.allCamerasCount;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameManager.playerManagers;

        for (int i = players.Count; i > (Camera.allCamerasCount - nonPlayerCams); i--)
        {
            GameObject tempCam = Instantiate(dynCamPrefab, transform);
            tempCam.GetComponent<DynCam>().player = players[i - 1];
            players[i - 1].playerCam = tempCam;
        }

        if(players.Count == 1)
        {
            Camera.allCameras[0].rect = new Rect((float)0.8, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.8, 1);
        }
        if (players.Count >= 2)
        {
            Camera.allCameras[0].rect = new Rect((float)0.4, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.4, 1);
            Camera.allCameras[2].rect = new Rect((float)0.6, 0, (float)0.4, 1);
        }
        if (players.Count >= 3)
        {
            Camera.allCameras[1].rect = new Rect(0, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[3].rect = new Rect(0, 0, (float)0.4, (float)0.5);
        }
        if (players.Count == 4)
        {
            Camera.allCameras[2].rect = new Rect((float)0.6, (float)0.5, (float)0.4, (float)0.5);
            Camera.allCameras[4].rect = new Rect((float)0.6, 0, (float)0.4, (float)0.5);
        }
    }


}
