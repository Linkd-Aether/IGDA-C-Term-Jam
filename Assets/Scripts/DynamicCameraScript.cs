using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraScript : MonoBehaviour
{
    public GameObject UICamPrefab;
    public GameObject dynCamPrefab;
    public int players;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(new Camera());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players - (Camera.allCamerasCount - 1); i++)
        {
            Instantiate(dynCamPrefab);
        }

        if(players == 1)
        {
            Camera.allCameras[0].rect = new Rect((float)0.8, 0, (float)0.2, 1);
            Camera.allCameras[1].rect = new Rect(0, 0, (float)0.8, 1);
        }
    }


}
