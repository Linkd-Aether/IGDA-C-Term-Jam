using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public Sprite Face;
    public int Value;
    public int Position;
    Mob target;
    float TargetPos;
    Vector2 TargetVect;
    public bool Complete;

    void Start()
    {
        transform.localPosition = new Vector2(0, -7);
        TargetPos = 10.25F - (Position * 3.25F);
        TargetVect = new Vector2(0, transform.localPosition.y + TargetPos);
        CreateCard();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y < TargetVect.y - 0.1)
        {
            transform.localPosition = (Vector2.Lerp(transform.localPosition, TargetVect, Time.deltaTime * 3));
        }
    }

    public void AssignTarget(Mob newTarget)
    {
        target = newTarget;
        if (newTarget is PlayerMob)
        {
            Face = target.GetComponentInChildren<SpriteRenderer>().sprite;

            int streak = target.GetComponentInParent<PlayerManager>().streak;
            float baseValue = Mathf.Max((streak * streak / 2), 4.5F);
            int unInflatedValue = Mathf.RoundToInt(baseValue + Random.Range(-Mathf.Sqrt(baseValue), Mathf.Sqrt(baseValue) * BountyGen.Intensity));
            Value = unInflatedValue * 1000;
        }
    }

    void CreateCard()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Face;
        transform.GetChild(1).GetComponent<DisplayNumber>().DisplayMoney(Value);
    }
}
