using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public Sprite Face;
    public int Value;
    public int Position;
    float TargetPos;
    Vector2 TargetVect;
    public bool Complete;

    // Update is called once per frame
    void Update()
    {
        TargetPos = 4 - (Position * 2);
        TargetVect = new Vector2(0, TargetPos);
        if(transform.position.y < TargetPos - 0.05)
        {
            transform.Translate(Vector2.Lerp(transform.position, TargetVect, Time.deltaTime * 2));
        }
    }
}
