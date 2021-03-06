using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwap : MonoBehaviour
{
    public Sprite p1Sprite;
    public Sprite p2Sprite;
    public Sprite p3Sprite;
    public Sprite p4Sprite;

    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.playerManagers.Count)
        {
            case 1:
                render.sprite = p1Sprite;
                break;
            case 2:
                render.sprite = p2Sprite;
                break;
            case 3:
                render.sprite = p3Sprite;
                break;
            case 4:
                render.sprite = p4Sprite;
                break;
        }
    }
}
